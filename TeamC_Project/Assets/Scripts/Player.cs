using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //[SerializeField]
    //private GameObject bulletPrefab;

    private InputManager inputManager;

    [SerializeField]
    private Vector3 minPosition;
    [SerializeField]
    private Vector3 maxPosition;

    private Screw screw;
    private ChargeBullet chargeBullet;
    private BulletController bulletController;

    [SerializeField]
    private float shotInterval = 2.0f;
    private float elapsedTime;

    [SerializeField]
    private float moveSpeed = 4.0f;

    private GameManager gameManager;

    public bool IsRapidFire
    {
        get;
        private set;
    } = false;//連射中か

    // Start is called before the first frame update
    void Start()
    {
        inputManager = GameObject.Find("InputManager").GetComponent<InputManager>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        screw = GetComponent<Screw>();
        chargeBullet = GetComponent<ChargeBullet>();
        bulletController = GetComponent<BulletController>();

        elapsedTime = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameManager.IsGameStart) return;

        Move();
        ShotBullet();
    }

    private void ShotBullet()
    {
        elapsedTime += Time.deltaTime;

        //通常状態以外には撃てない
        if (screw.GetMode() != Screw.Mode.NORMAL) return;

        Vector3 shotPosition = transform.position + Vector3.up;
        if (inputManager.GetA_ButtonDown())
        {
            if (elapsedTime < shotInterval) return;

            elapsedTime = 0.0f;
            bulletController.GenerateBullet(chargeBullet.GetChargeMode(), shotPosition, Vector3.up, 200.0f, 3.0f);
            chargeBullet.ResetCharge();
        }

        if (inputManager.GetA_Button())
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime < shotInterval) return;

            elapsedTime = 0.0f;
            bulletController.GenerateBullet(ChargeBullet.ChargeMode.STAGE_1, shotPosition, Vector3.up, 200.0f, 3.0f);
            chargeBullet.ResetCharge();
            IsRapidFire = true;
        }

        if (inputManager.GetA_ButtonUp())
        {
            if (!IsRapidFire) return;

            IsRapidFire = false;
            elapsedTime = 0.0f;
        }
    }

    //移動の処理
    private void Move()
    {
        //横軸の値を返す
        float x = inputManager.GetL_Stick_Horizontal();

        //縦軸の値を返す
        float y = inputManager.GetL_Stick_Vertical();
        if (screw.GetMode() == Screw.Mode.SCREW) y = 0;

        //移動制御＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊今回追加
        Vector3 nextPosition = transform.position + new Vector3(x, y, 0) * Time.deltaTime * moveSpeed;
        //移動できる範囲をMathf.Clampで範囲指定して制御
        nextPosition = new Vector3(
            Mathf.Clamp(nextPosition.x, minPosition.x, maxPosition.x),
            Mathf.Clamp(nextPosition.y, minPosition.y, maxPosition.y),
            nextPosition.z);
        //現在位置にnextPositionを＋
        transform.position = nextPosition;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            GetComponent<Health>().HitDeath();
            other.GetComponent<Health>().HitDeath();
        }
    }
}