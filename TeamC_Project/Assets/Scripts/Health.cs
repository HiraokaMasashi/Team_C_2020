﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ParticlaManager))]
public class Health : MonoBehaviour
{
    [SerializeField]
    private int hp = 1;
    private int maxHp;
    [SerializeField]
    private Text hpText;

    private ParticlaManager particlaManager;
    private SoundManager soundManager;
    [SerializeField]
    private string deadSe;
    [SerializeField]
    private string damageSe;

    [SerializeField]
    private GameObject debriPrefab;

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
        maxHp = hp;

        particlaManager = GetComponent<ParticlaManager>();
        soundManager = SoundManager.Instance;
    }

    private void Update()
    {
        DeathEffect();
        DisplayHp();
    }

    /// <summary>
    /// 死亡エフェクト
    /// </summary>
    private void DeathEffect()
    {
        if (!IsDead) return;

        GameObject particle = particlaManager.GenerateParticle();
        if (particle != null)
        {
            particle.transform.position = transform.position;
            particlaManager.OncePlayParticle(particle);
        }
        soundManager.PlaySeByName(deadSe);
        Destroy(gameObject);
    }

    /// <summary>
    /// 体力の表示
    /// </summary>
    private void DisplayHp()
    {
        if (hpText == null) return;

        hpText.text = "HP: " + hp + " / " + maxHp;

        if (hp <= maxHp * (1.0f / 3.0f))
            hpText.color = Color.red;
        else if (hp <= maxHp * (2.0f / 3.0f))
            hpText.color = Color.yellow;
        else
            hpText.color = Color.green;
    }

    /// <summary>
    /// 衝突時に呼ぶ死亡処理
    /// </summary>
    public void HitDeath()
    {
        hp = 0;

        if (debriPrefab == null) return;
        Instantiate(debriPrefab, transform.position, Quaternion.identity);
    }

    /// <summary>
    /// ダメージ処理
    /// </summary>
    /// <param name="damage">ダメージ量</param>
    public void Damage(int damage)
    {
        hp -= damage;
        if (transform.tag == "Player")
            soundManager.PlaySeByName(damageSe);
        if (hp <= 0)
        {
            hp = 0;

            if (debriPrefab == null) return;
            Instantiate(debriPrefab, transform.position, Quaternion.identity);
        }
    }
}
