using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private InputManager inputManager;

    [SerializeField]
    private Vector3 minPosition;//最小移動範囲
    [SerializeField]
    private Vector3 maxPosition;//最大移動範囲

    private Screw screw;//スクリュー
    private ChargeBullet chargeBullet;
    private BulletController bulletController;

    [SerializeField]
    private float shotInterval = 2.0f;//弾の発射可能時間の間隔
    private float elapsedTime;//経過時間

    [SerializeField]
    private float playerMoveSpeed = 4.0f;//移動速度
    [SerializeField]
    private float bulletMoveSpeed = 200.0f;//弾の移動速度

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

        //最初は撃てる状態にする
        elapsedTime = shotInterval;
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameManager.IsGameStart) return;

        Move();
        ShotBullet();
    }

    /// <summary>
    /// 弾の発射処理
    /// </summary>
    private void ShotBullet()
    {
        if (Time.timeScale == 0) return;

        elapsedTime += Time.deltaTime;

        //通常状態以外には撃てない
        if (screw.GetMode() != Screw.Mode.NORMAL) return;

        if (inputManager.GetA_ButtonDown())
        {
            if (elapsedTime < shotInterval) return;

            elapsedTime = 0.0f;
            InstanceBullet();
        }

        if (inputManager.GetA_Button())
        {
            //ボタン長押し時は、経過時間を早める
            elapsedTime += Time.deltaTime;
            if (elapsedTime < shotInterval) return;

            elapsedTime = 0.0f;
            InstanceBullet();
            IsRapidFire = true;
        }

        if (inputManager.GetA_ButtonUp())
        {
            if (!IsRapidFire) return;

            IsRapidFire = false;
            elapsedTime = 0.0f;
        }
    }

    /// <summary>
    /// 弾生成時の処理
    /// </summary>
    private void InstanceBullet()
    {
        //生成位置
        Vector3 shotPosition = transform.position + Vector3.up;

        bulletController.GenerateBullet(chargeBullet.GetChargeMode(), shotPosition, Vector3.up, bulletMoveSpeed, 3.0f);
        chargeBullet.ResetCharge();
    }

    /// <summary>
    /// 移動の処理
    /// </summary>
    private void Move()
    {
        //横軸の値を返す
        float x = inputManager.GetL_Stick_Horizontal();

        //縦軸の値を返す
        float y = inputManager.GetL_Stick_Vertical();
        if (screw.GetMode() == Screw.Mode.SCREW) y = 0;

        //移動制御＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊今回追加
        Vector3 nextPosition = transform.position + new Vector3(x, y, 0) * Time.deltaTime * playerMoveSpeed;
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
        //エネミーと衝突した場合は、お互いを死亡させる
        if (other.gameObject.tag == "Enemy")
        {
            GetComponent<Health>().HitDeath();
            other.GetComponent<Health>().HitDeath();
        }
    }
}