using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private Vector3 destroyZone;//死亡範囲
    [SerializeField]
    private float destroyZoneMinY = -11.0f;//y軸での生存範囲最低値

    private ParticleManager particleManager;

    private Vector3 direction;//進行方向
    [SerializeField]
    private float moveSpeed = 1.0f;//移動速度

    /// <summary>
    /// 攻撃力
    /// </summary>
    public int Attack
    {
        get;
        set;
    } = 1;

    private void Start()
    {
        particleManager = GetComponent<ParticleManager>();
    }

    private void FixedUpdate()
    {
        Move();
        if (GetIsDestroy())
            Disconnect();
    }

    /// <summary>
    /// 移動処理
    /// </summary>
    private void Move()
    {
        Vector3 position = transform.position;
        position += direction.normalized * Time.deltaTime * moveSpeed;
        transform.position = position;
    }

    /// <summary>
    /// 死亡時に呼ぶ処理
    /// </summary>
    public void Disconnect()
    {
        if (transform.childCount == 0) return;

        GameObject particle = null;
        //子オブジェクトについているパーティクルを切り離す
        //なければ行わない
        foreach (Transform child in transform)
        {
            if (child.name.Contains("Trail"))
            {
                particle = child.gameObject;
                break;
            }
        }
        if (particle == null) return;

        particleManager.StopParticle(particle);
        particle.transform.parent = null;
        Destroy(gameObject);
    }

    /// <summary>
    /// 現在位置を調べる
    /// </summary>
    /// <returns></returns>
    private bool GetIsDestroy()
    {
        bool isDestroy = false;

        if (transform.position.x <= -destroyZone.x || transform.position.x >= destroyZone.x
            || transform.position.y >= destroyZone.y || transform.position.y <= destroyZoneMinY)
            isDestroy = true;

        return isDestroy;
    }

    /// <summary>
    /// 進行方向の設定
    /// </summary>
    /// <param name="dir"></param>
    public void SetDirection(Vector3 dir)
    {
        direction = dir;
    }

    private void OnTriggerEnter(Collider other)
    {
        //プレイヤーの弾がエネミーにヒットしたら、またはエネミーの弾がプレイヤーにヒットしたら
        if ((other.transform.tag == "Player" && transform.tag == "EnemyBullet")
            || (other.transform.tag == "Enemy" && transform.tag == "PlayerBullet"))
        {
            //ヒットしたのがシールドを持ったステージ3のボスだったらreturn
            if (other.transform.name.Contains("Boss3") && other.transform.childCount != 0) return;

            //攻撃力分のダメージ
            Health health = other.transform.GetComponent<Health>();
            health.Damage(Attack);
            //ヒットエフェクトを再生
            GameObject particle = particleManager.GenerateParticle(1);
            particle.transform.position = other.ClosestPointOnBounds(transform.position);
            particle.transform.rotation = Quaternion.Euler(-90.0f, 0.0f, 0.0f);
            particleManager.OncePlayParticle(particle);

            //エネミーが死亡したら、スコアを加算する
            if (health.IsDead && other.transform.tag == "Enemy")
            {
                //スクリューにヒットしている最中のオブジェクトなら、リストから削除する
                GameObject[] screws = GameObject.FindGameObjectsWithTag("Screw");
                for (int i = screws.Length - 1; i >= 0; i--)
                {
                    List<GameObject> hitScrewObjects = screws[i].GetComponent<ScrewCollision>().GetObjects();
                    for (int j = hitScrewObjects.Count - 1; j >= 0; j--)
                    {
                        if (hitScrewObjects[j] == gameObject)
                            screws[i].GetComponent<ScrewCollision>().RemoveObject(j);
                    }
                }
            }

            if (other.gameObject.name.Contains("Boss"))
                Disconnect();

            if (transform.tag == "EnemyBullet")
                Disconnect();
        }

        if(transform.tag == "PlayerBullet" && other.transform.tag == "Shield")
        {
            //ヒットエフェクトを再生
            GameObject particle = particleManager.GenerateParticle(1);
            particle.transform.position = other.ClosestPointOnBounds(transform.position);
            particle.transform.rotation = Quaternion.Euler(-90.0f, 0.0f, 0.0f);
            particleManager.OncePlayParticle(particle);
        }

        if (other.transform.tag == "PlayerBullet" && transform.tag == "EnemyBullet")
            Disconnect();
    }
}
