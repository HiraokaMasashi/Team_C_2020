using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private Vector3 destroyZone;

    private SetUpScrew setupScrew;

    private BulletController bulletController;
    private float shotInterval;
    private float elapsedTime;
    private GameObject player;

    enum MoveMode
    {
        NORMAL,
        INTERSEPTION,
    }
    private MoveMode currentMode;

    // Start is called before the first frame update
    void Start()
    {
        setupScrew = GetComponent<SetUpScrew>();
        currentMode = MoveMode.NORMAL;

        bulletController = GetComponent<BulletController>();
        shotInterval = Random.Range(3, 6);
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (setupScrew.IsStan) return;

        //// 自分自身のtransformを取得
        //Transform myTransform = this.transform;

        ////Translate関数を使用
        ////Translate(float x,float y,float z,Space relativeTo = Space.Self)
        //myTransform.Translate(0, moveSpeed, 0, Space.World);

        switch (currentMode)
        {
            case MoveMode.NORMAL:
                NormalMove();
                break;

            case MoveMode.INTERSEPTION:
                InterseptionMove();
                break;

            default:
                break;
        }

        ShotBullet();

        if (transform.position.y <= destroyZone.y)
        {
            Destroy(gameObject);
        }
    }

    private void NormalMove()
    {
        Vector3 position = transform.position;
        position += Vector3.down * moveSpeed * Time.deltaTime;
        transform.position = position;
    }

    private void InterseptionMove()
    {

    }

    private void ShotBullet()
    {
        if (player == null) return;

        elapsedTime += Time.deltaTime;

        if (elapsedTime >= shotInterval)
        {
            Vector3 position = transform.position + Vector3.down;
            Vector3 direction = player.transform.position - transform.position;
            bulletController.GenerateBullet(ChargeBullet.ChargeMode.STAGE_1, position, direction, 300.0f, 3.0f, "Enemy");
            elapsedTime = 0;
        }
    }
}
