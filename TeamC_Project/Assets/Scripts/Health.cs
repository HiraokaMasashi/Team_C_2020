using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ParticleManager))]
public class Health : MonoBehaviour
{
    [SerializeField]
    private int hp = 1;
    private int maxHp;
    [SerializeField]
    private Text hpText;

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

        particleManager = GetComponent<ParticleManager>();
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
    }
}
