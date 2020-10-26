using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticlaManager))]
public class Screw : MonoBehaviour
{
    //プレイヤーの状態
    public enum Mode
    {
        NORMAL,
        SCREW,
        ROTATION_SCREW,
        ROTATION_NORMAL,
    }
    private Mode currentMode;

    /// <summary>
    /// スクリューを使用中か
    /// </summary>
    public bool IsUseScrew
    {
        get;
        private set;
    } = false;

    private bool isExistScrew = false;//スクリューが存在しているか

    private InputManager inputManager;
    private ParticlaManager particlaManager;

    private GameObject screw;//スクリュー

    [SerializeField, Tooltip("回転速度倍率")]
    private float magnificationSpeed = 3.0f;
    //回転速度
    private float rotationSpeed = 180.0f;

    //スクリュー使用時の角度
    private float screwRotation = -180.0f;
    //通常時の角度
    private float normalRotation = 0.0f;

    private BoxCollider boxCollider;

    private float minCenterY = 1.0f;
    private float minSizeY = 2.0f;

    private float maxCenterY = 6.0f;
    private float maxSizeY = 12.0f;

    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        inputManager = GameObject.Find("InputManager").GetComponent<InputManager>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        particlaManager = GetComponent<ParticlaManager>();
        currentMode = Mode.NORMAL;
        rotationSpeed *= magnificationSpeed;
    }

    void Update()
    {
        if (!gameManager.IsGameStart) return;

        switch (currentMode)
        {
            case Mode.NORMAL:
                //Rボタンを入力したら回転開始
                if (inputManager.GetR_ButtonDown())
                {
                    currentMode = Mode.ROTATION_SCREW;
                }
                break;

            case Mode.SCREW:
                //Rボタンを押している間、スクリューを生成
                IsUseScrew = inputManager.GetR_Button();

                if (IsUseScrew)
                {
                    GenerateScrew();
                    ChangeBoxSize();
                    screw.transform.position = transform.position + Vector3.up;
                    EnemyStanMove();
                }
                else
                {
                    StopScrew();
                }
                break;

            case Mode.ROTATION_NORMAL:
                //元の回転に戻す
                RotationDefault();
                break;

            case Mode.ROTATION_SCREW:
                //スクリューを出すための回転
                RotationUseScrew();
                break;

            default:
                break;
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
        screw = particlaManager.GenerateParticleInChildren(1);
        //あたり判定を付ける
        boxCollider = screw.GetComponent<BoxCollider>();
        boxCollider.enabled = true;
        particlaManager.StartParticle(screw);
        isExistScrew = true;
    }

    /// <summary>
    /// スクリュー使用時にあたり判定のサイズを調整する
    /// </summary>
    private void ChangeBoxSize()
    {
        if (boxCollider == null) return;

        //あたり判定の調整サイズ
        float addSize = 0.0f;
        addSize += Time.deltaTime;

        float speed = 2.0f; 
        Vector3 center = boxCollider.center;
        center.y += addSize * speed;
        //最大値を超えていたら範囲内に収める
        if (center.y >= maxCenterY) center.y = maxCenterY;
        boxCollider.center = center;

        Vector3 size = boxCollider.size;
        size.y += addSize * 2.0f * speed;
        //最大値を超えていたら範囲内に収める
        if (size.y >= maxSizeY) size.y = maxSizeY;
        boxCollider.size = size;
    }

    /// <summary>
    /// 調整したあたり判定を元に戻す
    /// </summary>
    private void ResetBoxSize()
    {
        if (boxCollider == null) return;

        Vector3 center = boxCollider.center;
        center.y = minCenterY;
        boxCollider.center = center;

        Vector3 size = boxCollider.size;
        size.y = minSizeY;
        boxCollider.size = size;

        boxCollider = null;
    }

    /// <summary>
    /// スクリューの停止
    /// </summary>
    private void StopScrew()
    {
        //パーティクルが生成されたときだけ
        if (screw != null)
        {
            //パーティクルの生成を止める
            particlaManager.StopParticle(screw);
            //あたり判定をはずす
            boxCollider.enabled = false;
            ResetBoxSize();
            isExistScrew = false;
            EnemyStartRecovery();
            screw.transform.parent = null;
            screw = null;
        }
        //元に戻る回転状態
        currentMode = Mode.ROTATION_NORMAL;
    }

    /// <summary>
    /// 敵のスタン回復処理を実行
    /// </summary>
    private void EnemyStartRecovery()
    {
        List<GameObject> enemies = screw.GetComponent<ScrewCollision>().GetEnemies();
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].GetComponent<SetUpScrew>().LeaveScrew();
            screw.GetComponent<ScrewCollision>().RemoveEnemy(i);
            i--;
        }
    }

    /// <summary>
    /// 敵のスタン中の移動
    /// </summary>
    private void EnemyStanMove()
    {
        List<GameObject> enemies = screw.GetComponent<ScrewCollision>().GetEnemies();

        if (enemies == null) return;
        foreach (var e in enemies)
        {
            e.GetComponent<SetUpScrew>().StanMove(transform.position);
        }
    }

    /// <summary>
    /// スクリューを使用するための回転
    /// </summary>
    private void RotationUseScrew()
    {
        //z軸に180度回転させる
        float step = rotationSpeed * Time.deltaTime;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, screwRotation), step);
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
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, normalRotation), step);
        Vector3 euler = transform.rotation.eulerAngles;

        if (euler.z <= normalRotation)
        {
            transform.rotation = Quaternion.Euler(euler.x, euler.y, normalRotation);
            currentMode = Mode.NORMAL;
        }
    }

    /// <summary>
    /// 現在の状態を取得
    /// </summary>
    /// <returns></returns>
    public Mode GetMode()
    {
        return currentMode;
    }
}
