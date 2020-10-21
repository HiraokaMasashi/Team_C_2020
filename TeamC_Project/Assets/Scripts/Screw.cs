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

    private bool existScrew = false;//スクリューが存在しているか

    private InputManager inputManager;
    private ParticlaManager particlaManager;

    private GameObject screw;//スクリュー

    [SerializeField]
    private float rotationSpeed = 1.0f;

    private float screwRotation = -180.0f;
    private float normalRotation = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        inputManager = GameObject.Find("InputManager").GetComponent<InputManager>();
        particlaManager = GetComponent<ParticlaManager>();
        currentMode = Mode.NORMAL;
    }

    void Update()
    {
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
                    screw.transform.position = transform.position;
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
        }
    }

    /// <summary>
    /// スクリューの生成
    /// </summary>
    private void GenerateScrew()
    {
        //パーティクルが存在しないときに1度だけ
        if (existScrew) return;

        //スクリューパーティクルの生成
        screw = particlaManager.GenerateParticleInChildren();
        //あたり判定を付ける
        screw.GetComponent<BoxCollider>().enabled = true;
        particlaManager.StartParticle(screw);
        existScrew = true;
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
            screw.GetComponent<BoxCollider>().enabled = false;
            existScrew = false;
            EnemyStartRecovery();
            screw.transform.parent = null;
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
