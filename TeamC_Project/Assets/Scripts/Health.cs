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
        if (IsDead)
        {
            Debug.Log("死亡");
            GameObject particle = particlaManager.GenerateParticle();
            if (particle != null)
            {
                particle.transform.position = transform.position;
                particlaManager.OncePlayParticle(particle);
            }
            Destroy(gameObject);
        }
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
