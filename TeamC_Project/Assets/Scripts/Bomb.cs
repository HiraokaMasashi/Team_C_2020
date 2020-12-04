using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    private GameObject player;//プレイヤー
    private ParticleManager particleManager;//パーティクルマネージャー
    private NormalBoss boss;//ボス
    private SetUpScrew setUpScrew;

    private Vector3 direction;//進行方向
    [SerializeField]
    private float moveSpeed = 0.5f;//移動速度

    [SerializeField]
    private Vector3 destroyZone = new Vector3(16, -11, 0);//死亡範囲

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        particleManager = GetComponent<ParticleManager>();
        boss = GameObject.Find("BombBoss(Clone)").GetComponent<NormalBoss>();
        setUpScrew = GetComponent<SetUpScrew>();

        direction = player.transform.position - transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //スタン中はreturn
        if (setUpScrew.IsStan) return;

        Move();
        if (GetIsDestroy())
            DisConnect();
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
    /// 現在位置を調べる
    /// </summary>
    /// <returns></returns>
    private bool GetIsDestroy()
    {
        bool isDestroy = false;

        if (transform.position.x <= -destroyZone.x || transform.position.x >= destroyZone.x
            || transform.position.y <= destroyZone.y)
            isDestroy = true;

        return isDestroy;
    }

    /// <summary>
    /// 死亡処理
    /// </summary>
    private void DisConnect()
    {
        boss.RemoveBomb(gameObject);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        //プレイヤーの弾に当たったら
        if (other.gameObject.tag == "PlayerBullet")
        {
            //弾が貫通弾でなければ、弾を削除する
            if (!other.gameObject.GetComponent<Bullet>().IsPenetrate)
                other.gameObject.GetComponent<Bullet>().Disconnect();

            //スクリューにヒットしていたら、リストから削除
            GameObject[] screws = GameObject.FindGameObjectsWithTag("Screw");
            for (int i = screws.Length - 1; i >= 0; i--)
            {
                int length = screws[i].GetComponent<ScrewCollision>().GetObjects().Count;
                for (int j = length - 1; j >= 0; j--)
                {
                    screws[i].GetComponent<ScrewCollision>().RemoveObject(j);
                }
            }

            //爆風エフェクトを再生し、削除
            GameObject particle = particleManager.GenerateParticle();
            particle.transform.position = transform.position;
            particle.transform.rotation = Quaternion.Euler(-90, 0, 0);
            particleManager.OncePlayParticle(particle);
            DisConnect();
        }

        //プレイヤーまたは、爆風エフェクトに当たったら
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Blast")
        {
            //スクリューにヒットしていたら、リストから削除
            GameObject[] screws = GameObject.FindGameObjectsWithTag("Screw");
            for (int i = screws.Length - 1; i >= 0; i--)
            {
                int length = screws[i].GetComponent<ScrewCollision>().GetObjects().Count;
                for (int j = length - 1; j >= 0; j--)
                {
                    screws[i].GetComponent<ScrewCollision>().RemoveObject(j);
                }
            }

            //爆風エフェクトを再生し、削除
            GameObject particle = particleManager.GenerateParticle();
            particle.transform.position = transform.position;
            particle.transform.rotation = Quaternion.Euler(-90, 0, 0);
            particleManager.OncePlayParticle(particle);
            DisConnect();
        }
    }
}
