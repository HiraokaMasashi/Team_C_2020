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
    private float screwUseTime = 3.0f;
    private float screwElapsedTime;

    [SerializeField]
    private float rotationInterval = 1.0f;
    private float rotationElapsedTime;

    [SerializeField]
    private GameObject drillPrefab;
    private GameObject drill;

    [SerializeField]
    private string[] ses;
    private bool isPlayDrillSE;

    public bool IsEquipmentDrill
    {
        get;
        private set;
    } = false;

    private FadeScene fadeScene;

    // Start is called before the first frame update
    void Start()
    {
        inputManager = GameObject.Find("InputManager").GetComponent<InputManager>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        fadeScene = GameObject.Find("FadeScene").GetComponent<FadeScene>();
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

        screwElapsedTime = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 0) return;
        if (fadeScene.IsFadeOut) return;
        if (!gameManager.IsGameStart) return;
        if (gameManager.IsPerformance || gameManager.IsEnd)
        {
            //通常状態でなければ、通常状態に戻す
            if (currentMode != Mode.NORMAL)
            {
                StopScrew_Drill();
                RotationDefault();
            }
            return;
        }

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
        //通常状態以外には撃てない
        if (currentMode != Mode.NORMAL) return;

        if (inputManager.GetA_ButtonDown())
            InstanceBullet();

        if (inputManager.GetA_Button())
        {
            //ボタン長押し時は、経過時間を早める
            shotBulletElapsedTime += Time.deltaTime;
            if (shotBulletElapsedTime < shotBulletInterval) return;

            shotBulletElapsedTime = 0.0f;
            InstanceBullet();
        }

        if (inputManager.GetA_ButtonUp())
            shotBulletElapsedTime = 0.0f;
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
        soundManager.PlaySeByName(ses[3]);
        chargeBullet.DecreaseCharge();
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
                        if (isUse && screwElapsedTime < screwUseTime)
                        {
                            GenerateScrew();
                            inhaleScrewObject.transform.position = transform.position + Vector3.up * 10.0f;
                            screwElapsedTime += Time.deltaTime;
                        }
                        else
                            StopScrew_Drill();
                    }
                    else
                    {
                        if (drill == null)
                        {
                            IsEquipmentDrill = false;
                            StopScrew_Drill();
                            currentMode = Mode.ROTATION_NORMAL;
                        }

                        if (isUse) UseDrill();
                        else StopScrew_Drill();
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
            StopScrew_Drill();
    }

    /// <summary>
    /// スクリューの発射処理
    /// </summary>
    private void ShotScrew()
    {
        //撃てる状態でなければreturn
        if (shotScrewElapsedTime < shotScrewInterval) return;

        //すでに存在していたら
        if (shotScrewObject != null)
        {
            //エネミーの回復処理を実行
            EnemyRecoveryStart(shotScrewObject);
            //パーティクルを停止
            screw.GetComponent<ParticleManager>().StopParticle(shotScrewObject);
        }

        //パーティクルを再生
        shotScrewObject = screw.GetComponent<ParticleManager>().GenerateParticle(1);
        shotScrewObject.transform.position = transform.position;
        shotScrewObject.GetComponent<Screw>().SetScrewType(Screw.ScrewType.SHOT);
        screw.GetComponent<ParticleManager>().StartParticle(shotScrewObject);
        soundManager.PlaySeByName(ses[1], true);

        shotScrewElapsedTime = 0.0f;
    }

    /// <summary>
    /// 通常状態への回転開始処理
    /// </summary>
    private void RotationInterval()
    {
        rotationElapsedTime += Time.deltaTime;
        //回転開始時間まではreturn
        if (rotationElapsedTime < rotationInterval) return;

        currentMode = Mode.ROTATION_NORMAL;
        rotationElapsedTime = 0.0f;
    }

    //射出したスクリューの削除
    private void DestroyShotScrew()
    {
        //射出したスクリューがなければreturn
        if (shotScrewObject == null) return;

        destroyShotScrewTime += Time.deltaTime;
        //削除時間まではreturn
        if (destroyShotScrewTime < shotScrewDestroyTime) return;

        //スクリューのあたり判定をなくす
        shotScrewObject.GetComponent<BoxCollider>().enabled = false;
        shotScrewObject.GetComponent<Screw>().SetScrewType(Screw.ScrewType.NONE);
        //エネミーの回復処理を実行
        EnemyRecoveryStart(shotScrewObject);
        //パーティクルを停止
        screw.GetComponent<ParticleManager>().StopParticle(shotScrewObject);
        shotScrewObject = null;
        destroyShotScrewTime = 0.0f;
        //SEを止める
        soundManager.StopSe();
    }

    /// <summary>
    /// エネミーの回復開始処理
    /// </summary>
    /// <param name="screw"></param>
    private void EnemyRecoveryStart(GameObject screw)
    {
        //スクリューがなければ行わない
        if (screw == null) return;
        //スクリューを中心に整列させる
        float alignmentPositionX, alignmentPositionY;
        float stanTime = screw.GetComponent<Screw>().StanTime;
        if (screw == shotScrewObject)
        {
            alignmentPositionX = shotScrewObject.transform.position.x;
            alignmentPositionY = screw.transform.position.y + 4.0f;
        }
        else
        {
            alignmentPositionX = transform.position.x;
            alignmentPositionY = screw.transform.position.y - 2.0f;
        }

        List<GameObject> enemies = screw.GetComponent<ScrewCollision>().GetObjects();
        int count = 0;
        for (int i = enemies.Count - 1; i >= 0; i--)
        {
            SetUpScrew setUpScrew = enemies[i].GetComponent<SetUpScrew>();
            //整列数が5体以上いたら、新しい列をつくる
            if (count >= 5)
            {
                count = 0;
                alignmentPositionX += 2.0f;
                if (screw != shotScrewObject)
                    alignmentPositionY = screw.transform.position.y - 2.0f;
                else
                    alignmentPositionY = screw.transform.position.y + 4.0f;
            }

            //制限範囲を超える場合は範囲に収める
            if (alignmentPositionX >= setUpScrew.ClapmPosition.x)
                alignmentPositionX = setUpScrew.ClapmPosition.x;
            if (alignmentPositionY >= setUpScrew.ClapmPosition.y)
                alignmentPositionY = setUpScrew.ClapmPosition.y;

            setUpScrew.ReleaseScrew(alignmentPositionX, alignmentPositionY, stanTime);
            screw.GetComponent<ScrewCollision>().RemoveObject(i);
            alignmentPositionY += 2.0f;
            count++;
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
        soundManager.PlaySeByName(ses[0], true);
        isExistScrew = true;
    }

    /// <summary>
    /// スクリュー又はドリルの停止
    /// </summary>
    private void StopScrew_Drill()
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
            screwElapsedTime = 0;
        }

        //ドリルがあれば
        if (drill != null)
        {
            //ドリルを切り離す
            drill.GetComponent<BoxCollider>().enabled = false;
            drill.GetComponent<Drill>().IsThrowAway = true;
            drill.transform.parent = null;
            drill = null;
        }
        //SEを止める
        soundManager.StopSe();

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
            IsEquipmentDrill = false;
        }
    }

    /// <summary>
    /// ドリルの使用
    /// </summary>
    private void UseDrill()
    {
        if (drill == null) return;

        drill.transform.parent = null;
        drill.GetComponent<BoxCollider>().enabled = true;

        if (isPlayDrillSE) return;
        soundManager.PlaySeByName(ses[2], true);
        isPlayDrillSE = true;
    }

    /// <summary>
    /// ドリルの射出
    /// </summary>
    private void ShotDrill()
    {
        if (drill == null) return;

        drill.GetComponent<Drill>().Shot();
        drill.transform.parent = null;
        drill = null;
        soundManager.PlaySeByName(ses[2]);
    }

    /// <summary>
    /// ドリルの装着処理
    /// </summary>
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
            if (!other.gameObject.name.Contains("Boss"))
                other.GetComponent<Health>().HitDeath();
        }
    }

    /// <summary>
    /// 現在の状態を返す
    /// </summary>
    /// <returns></returns>
    public Mode GetCurrentMode()
    {
        return currentMode;
    }
}