﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ParticleManager))]
public class Health : MonoBehaviour
{
    [SerializeField]
    private int hp = 1;
    public int MaxHp
    {
        get;
        private set;
    }

    private ParticleManager particleManager;
    private SoundManager soundManager;
    [SerializeField]
    private string deadSe;
    [SerializeField]
    private string damageSe;

    [SerializeField]
    private GameObject dropPrefab;
    [SerializeField, Tooltip("生成位置の調整")]
    private Vector3 adjustPosition;

    private bool isDamageEffect;
    [SerializeField]
    private MeshRenderer[] meshRenderers;
    [SerializeField]
    private float effectTime = 2.0f;
    private float effectElapsedTime;
    [SerializeField]
    private float effectSpeed = 0.5f;

    private GameManager gameManager;
    [SerializeField]
    private float particleInstanceTime = 0.5f;

    [SerializeField]
    private Vector3 minScale = Vector3.one;

    /// <summary>
    /// 体力
    /// </summary>
    public int Hp
    {
        get { return hp; }
    }

    /// <summary>
    /// 死亡判定
    /// </summary>
    public bool IsDead
    {
        //HPが0以下ならtrueを返す
        get { return hp <= 0; }
    }

    private void Start()
    {
        MaxHp = hp;

        particleManager = GetComponent<ParticleManager>();
        soundManager = SoundManager.Instance;
        isDamageEffect = false;
        effectElapsedTime = 0;

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void Update()
    {
        DamageEffect();
        DeathEffect();
    }

    /// <summary>
    /// 死亡エフェクト
    /// </summary>
    private void DeathEffect()
    {
        if (!IsDead) return;

        if (!transform.name.Contains("Boss"))
        {
            GameObject particle = particleManager.GenerateParticle();
            if (particle != null)
            {
                particle.transform.position = transform.position;
                particle.transform.rotation = Quaternion.Euler(-90.0f, 0.0f, 0.0f);
                particleManager.OncePlayParticle(particle);
            }

            if (deadSe != "")
                soundManager.PlaySeByName(deadSe);
            Destroy(gameObject);
        }
        else
        {
            if (gameManager.IsPerformance) return;

            gameManager.IsPerformance = true;
            GetComponent<Boss>().DestroyOtherObject();
            ExplosionInstance();
            StartCoroutine(BossDeadEffect());
        }
    }


    /// <summary>
    /// 衝突時に呼ぶ死亡処理
    /// </summary>
    public void HitDeath()
    {
        hp = 0;

        if (dropPrefab != null)
            Instantiate(dropPrefab, transform.position + adjustPosition, Quaternion.identity);
    }

    /// <summary>
    /// ダメージ処理
    /// </summary>
    /// <param name="damage">ダメージ量</param>
    public void Damage(int damage)
    {
        hp -= damage;
        if (damageSe != "")
            soundManager.PlaySeByName(damageSe);
        if (hp <= 0)
        {
            hp = 0;

            if (dropPrefab != null)
                Instantiate(dropPrefab, transform.position + adjustPosition, Quaternion.identity);
        }
        else
            isDamageEffect = true;
    }

    private void DamageEffect()
    {
        if (!isDamageEffect || Time.timeScale == 0) return;

        effectElapsedTime += Time.deltaTime;
        if (effectElapsedTime >= effectTime)
        {
            foreach (var mesh in meshRenderers)
            {
                mesh.enabled = true;
            }
            isDamageEffect = false;
            effectElapsedTime = 0.0f;
            return;
        }

        foreach (var mesh in meshRenderers)
        {
            mesh.enabled = !mesh.enabled;
        }
    }

    private void ExplosionInstance(float adjustPositionX = 0.0f, float adjustPositionY = 0.0f)
    {
        GameObject particle = particleManager.GenerateParticle(1);
        particle.transform.position = transform.position + new Vector3(adjustPositionX, adjustPositionY, 0.5f);
        float randomScale = Random.Range(0.5f, 1.0f);
        particle.transform.localScale = new Vector3(randomScale, randomScale, randomScale);
        particle.transform.rotation = Quaternion.Euler(90, 0, 0);
        particleManager.OncePlayParticle(particle);
    }

    private IEnumerator BossDeadEffect()
    {
        Vector3 scale = transform.localScale;
        float elapsedTime = 0;
        while (true)
        {
            scale -= Vector3.one * Time.deltaTime * effectSpeed;
            transform.localScale = scale;
            transform.rotation *= Quaternion.Euler(5 * Time.deltaTime, 0, 10 * Time.deltaTime);

            elapsedTime += Time.deltaTime;
            if (elapsedTime >= particleInstanceTime)
            {
                float x = Random.Range(-0.5f, 0.5f);
                float y = Random.Range(-0.5f, 0.5f);
                ExplosionInstance(x, y);
                elapsedTime = 0;
            }

            if (transform.localScale.x <= minScale.x || transform.localScale.y <= minScale.y || transform.localScale.z <= minScale.z)
            {
                gameManager.IsPerformance = false;
                ExplosionInstance();
                Destroy(gameObject);
                yield break;
            }

            yield return null;
        }

    }
}
