using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private InputManager inputManager;
    private Health health;

    [SerializeField]
    private Vector3 minPosition;//最小移動範囲
    [SerializeField]
    private Vector3 maxPosition;//最大移動範囲

    private ChargeBullet chargeBullet;
    private BulletController bulletController;

    [SerializeField]
    private float shotBulletInterval = 2.0f;//弾の発射可能時間の間隔
    private float shotBulletElapsedTime;//経過時間

    [SerializeField]
    private float playerMoveSpeed = 4.0f;//移動速度

    private GameManager gameManager;
    private SoundManager soundManager;

    //プレイヤーの状態
    public enum Mode
    {
        NORMAL,
        SCREW,
        ROTATION_SCREW,
        ROTATION_NORMAL,
    }
    private Mode currentMode;

    [SerializeField]
    private GameObject screw;
    private GameObject inhaleScrewObject;
    private GameObject shotScrewObject;
    private bool isExistScrew;//スクリューが存在しているか

    private bool isUse;
    [SerializeField]
    private float shotScrewInterval = 2.0f;
    [SerializeField]
    private float shotScrewDestroyTime = 2.0f;
    private float shotScrewElapsedTime;
    private float destroyShotScrewTime;

    private bool isShotScrew;
    private bool isStartScrew;

    [SerializeField, Tooltip("回転速度倍率")]
    private float magnificationSpeed = 3.0f;
    //回転速度
    private float rotationSpeed = 180.0f;
    //スクリュー使用時の角度
    private float screwRotation = -180.0f;
    //通常時の角度
    private float normalRotation = 0.0f;

    [SerializeField]
    private float rotationInterval = 1.0f;
    private float rotationElapsedTime;

    [SerializeField]
    private GameObject drillPrefab;
    private GameObject drill;

    [SerializeField]
    private string[] ses;
    private bool isPlayDrillSE;

    //public bool IsRapidFire
    //{
    //    get;
    //    private set;
    //} = false;//連射中か

    public bool IsEquipmentDrill
    {
        get;
        private set;
    } = false;

    // Start is called before the first frame update
    void Start()
    {
        inputManager = GameObject.Find("InputManager").GetComponent<InputManager>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        health = GetComponent<Health>();
        chargeBullet = GetComponent<ChargeBullet>();
        bulletController = GetComponent<BulletController>();

        //最初は撃てる状態にする
        shotBulletElapsedTime = 0.0f;
        shotScrewElapsedTime = shotScrewInterval;
        isExistScrew = false;
        isUse = false;
        rotationSpeed *= magnificationSpeed;
        rotationElapsedTime = 0.0f;
        soundManager = SoundManager.Instance;
        isPlayDrillSE = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 0) return;
        if (!gameManager.IsGameStart) return;

        Move();
        ShotBullet();
        UseScrew();
        DestroyShotScrew();
    }

    /// <summary>
    /// 弾の発射処理
    /// </summary>
    private void ShotBullet()
    {
        //shotBulletElapsedTime += Time.deltaTime;

        //通常状態以外には撃てない
        if (currentMode != Mode.NORMAL) return;

        if (inputManager.GetA_ButtonDown())
        {
            //if (shotBulletElapsedTime < shotBulletInterval) return;

            //shotBulletElapsedTime = 0.0f;
            InstanceBullet();
        }

        if (inputManager.GetA_Button())
        {
            //ボタン長押し時は、経過時間を早める
            shotBulletElapsedTime += Time.deltaTime;
            if (shotBulletElapsedTime < shotBulletInterval) return;

            shotBulletElapsedTime = 0.0f;
            InstanceBullet();
            //IsRapidFire = true;
        }

        if (inputManager.GetA_ButtonUp())
        {
            //if (!IsRapidFire) return;

            //IsRapidFire = false;
            shotBulletElapsedTime = 0.0f;
        }
    }

    /// <summary>
    /// 弾生成時の処理
    /// </summary>
    private void InstanceBullet()
    {
        if (!chargeBullet.GetCanShot()) return;

        //生成位置
        Vector3 shotPosition = transform.position + Vector3.up;

        bulletController.GenerateBullet(shotPosition, Vector3.up, 3.0f);
        chargeBullet.DecreaseCharge();
        //chargeBullet.ResetCharge();
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

        Vector3 position = transform.position + new Vector3(x, y, 0) * Time.deltaTime * playerMoveSpeed;
        //移動できる範囲をMathf.Clampで範囲指定して制御
        position = new Vector3(
            Mathf.Clamp(position.x, minPosition.x, maxPosition.x),
            Mathf.Clamp(position.y, minPosition.y, maxPosition.y),
            position.z);

        transform.position = position;
    }

    private void UseScrew()
    {
        shotScrewElapsedTime += Time.deltaTime;

        switch (currentMode)
        {
            case Mode.NORMAL:
                //Rボタンを入力したら回転開始
                if (inputManager.GetR_ButtonDown() && !isShotScrew)
                {
                    currentMode = Mode.ROTATION_SCREW;
                    isStartScrew = true;
                }
                //Lボタンを入力したら回転開始
                if (inputManager.GetL_ButtonDown() && !isStartScrew)
                {
                    currentMode = Mode.ROTATION_SCREW;
                    isShotScrew = true;
                }
                break;

            case Mode.SCREW:
                if (isStartScrew)
                {
                    //Rボタンを押している間、スクリューを生成
                    isUse = inputManager.GetR_Button();

                    if (!IsEquipmentDrill)
                    {

                        if (isUse)
                        {
                            GenerateScrew();
                            inhaleScrewObject.transform.position = transform.position + Vector3.up * 10.0f;
                        }
                        else
                            StopScrew();
                    }
                    else
                    {
                        if (drill == null)
                        {
                            IsEquipmentDrill = false;
                            StopScrew();
                            currentMode = Mode.ROTATION_NORMAL;
                        }

                        if (isUse) UseDrill();
                        else StopScrew();
                    }
                }
                else if (isShotScrew)
                {
                    if (!IsEquipmentDrill)
                        ShotScrew();
                    else
                        ShotDrill();
                    RotationInterval();
                }
                break;

            case Mode.ROTATION_NORMAL:
                //元の回転に戻す
                RotationDefault();
                isPlayDrillSE = false;
                break;

            case Mode.ROTATION_SCREW:
                //スクリューを出すための回転
                RotationUseScrew();
                break;

            default:
                break;
        }

        //死亡時にパーティクルを生成していれば切り離す
        if (health.IsDead)
            StopScrew();
    }

    private void ShotScrew()
    {
        if (shotScrewElapsedTime < shotScrewInterval) return;

        if (shotScrewObject != null)
        {
            EnemyRecoveryStart(shotScrewObject);
            screw.GetComponent<ParticleManager>().StopParticle(shotScrewObject);
        }

        shotScrewObject = screw.GetComponent<ParticleManager>().GenerateParticle(1);
        shotScrewObject.transform.position = transform.position;
        shotScrewObject.GetComponent<Screw>().SetScrewType(Screw.ScrewType.SHOT);
        screw.GetComponent<ParticleManager>().StartParticle(shotScrewObject);
        soundManager.PlaySeByName(ses[1]);

        shotScrewElapsedTime = 0.0f;
    }

    private void RotationInterval()
    {
        rotationElapsedTime += Time.deltaTime;
        if (rotationElapsedTime < rotationInterval) return;

        currentMode = Mode.ROTATION_NORMAL;
        rotationElapsedTime = 0.0f;

        if (drill != null)
            drill.GetComponent<BoxCollider>().enabled = false;
        else
            IsEquipmentDrill = false;
    }

    private void DestroyShotScrew()
    {
        if (shotScrewObject == null) return;

        destroyShotScrewTime += Time.deltaTime;

        if (destroyShotScrewTime < shotScrewDestroyTime) return;

        shotScrewObject.GetComponent<BoxCollider>().enabled = false;
        shotScrewObject.GetComponent<Screw>().SetScrewType(Screw.ScrewType.NONE);
        EnemyRecoveryStart(shotScrewObject);
        screw.GetComponent<ParticleManager>().StopParticle(shotScrewObject);
        shotScrewObject = null;
        destroyShotScrewTime = 0.0f;
        soundManager.StopSe();
    }

    private void EnemyRecoveryStart(GameObject screw)
    {
        if (screw == null) return;

        List<GameObject> enemies = screw.GetComponent<ScrewCollision>().GetObjects();
        for (int i = enemies.Count - 1; i >= 0; i--)
        {
            enemies[i].GetComponent<SetUpScrew>().NotRecovery = false;
            screw.GetComponent<ScrewCollision>().RemoveObject(i);
        }
    }

    /// <summary>
    /// スクリューの生成
    /// </summary>
    private void GenerateScrew()
    {
        //パーティクルが存在しないときに1度だけ
        if (isExistScrew) return;

        //スクリューパーティクルの生成
        inhaleScrewObject = screw.GetComponent<ParticleManager>().GenerateParticle(0);
        inhaleScrewObject.GetComponent<Screw>().SetScrewType(Screw.ScrewType.INHALE);
        screw.GetComponent<ParticleManager>().StartParticle(inhaleScrewObject);
        soundManager.PlaySeByName(ses[0]);
        isExistScrew = true;
    }

    /// <summary>
    /// スクリューの停止
    /// </summary>
    private void StopScrew()
    {
        //パーティクルが生成されたときだけ
        if (inhaleScrewObject != null)
        {
            //パーティクルの生成を止める
            EnemyRecoveryStart(inhaleScrewObject);
            screw.GetComponent<ParticleManager>().StopParticle(inhaleScrewObject);
            inhaleScrewObject.GetComponent<BoxCollider>().enabled = false;
            isExistScrew = false;
            inhaleScrewObject = null;
            soundManager.StopSe();
        }

        if (drill != null)
        {
            drill.transform.parent = transform;
            drill.GetComponent<BoxCollider>().enabled = false;
        }

        if (currentMode == Mode.SCREW)
            //元に戻る回転状態
            currentMode = Mode.ROTATION_NORMAL;
    }

    /// <summary>
    /// スクリューを使用するための回転
    /// </summary>
    private void RotationUseScrew()
    {
        //z軸に180度回転させる
        float step = rotationSpeed * Time.deltaTime;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(-30, 0, screwRotation), step);
        Vector3 euler = transform.rotation.eulerAngles;

        if (euler.z >= screwRotation * -1)
        {
            transform.rotation = Quaternion.Euler(euler.x, euler.y, screwRotation * -1);
            currentMode = Mode.SCREW;
        }
    }

    /// <summary>
    /// 通常状態に戻すための回転
    /// </summary>
    private void RotationDefault()
    {
        //z軸に180度回転させる
        float step = rotationSpeed * Time.deltaTime;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(-30, 0, normalRotation), step);
        Vector3 euler = transform.rotation.eulerAngles;

        if (euler.z <= normalRotation)
        {
            transform.rotation = Quaternion.Euler(euler.x, euler.y, normalRotation);
            currentMode = Mode.NORMAL;
            isStartScrew = false;
            isShotScrew = false;
        }
    }

    private void UseDrill()
    {
        if (drill == null) return;

        drill.transform.parent = null;
        drill.GetComponent<BoxCollider>().enabled = true;

        if (isPlayDrillSE) return;
        soundManager.PlaySeByName(ses[2]);
        isPlayDrillSE = true;
    }

    private void ShotDrill()
    {
        if (drill == null) return;

        drill.GetComponent<Drill>().Shot();
        drill.transform.parent = null;
        drill = null;
        soundManager.PlaySeByName(ses[2]);
    }

    public void EquipmentDrill()
    {
        drill = Instantiate(drillPrefab, transform.position - transform.up * 1.5f, Quaternion.identity, transform);
        IsEquipmentDrill = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        //エネミーと衝突した場合は、お互いを死亡させる
        if (other.gameObject.tag == "Enemy")
        {
            health.HitDeath();
            other.GetComponent<Health>().HitDeath();
        }
    }

    public Mode GetCurrentMode()
    {
        return currentMode;
    }
}