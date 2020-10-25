using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticlaManager))]
public class Health : MonoBehaviour
{
    [SerializeField]
    private int hp = 1;

    private ParticlaManager particlaManager;

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
        particlaManager = GetComponent<ParticlaManager>();
    }

    private void Update()
    {
        DeathEffect();
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
        Destroy(gameObject);
    }

    /// <summary>
    /// 衝突時に呼ぶ死亡処理
    /// </summary>
    public void HitDeath()
    {
        hp = 0;
    }

    /// <summary>
    /// ダメージ処理
    /// </summary>
    /// <param name="damage">ダメージ量</param>
    public void Damage(int damage)
    {
        hp -= damage;
        if (hp <= 0) hp = 0;
    }
}
