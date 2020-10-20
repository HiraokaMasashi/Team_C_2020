using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticlaManager))]
public class Screw : MonoBehaviour
{
    enum Mode
    {
        NORMAL,
        SCREW,
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

    //[SerializeField]
    //private float rotateSpeed = 1.0f;
    //private float step;

    //private bool isRotationBack;

    // Start is called before the first frame update
    void Start()
    {
        inputManager = GameObject.Find("InputManager").GetComponent<InputManager>();
        particlaManager = GetComponent<ParticlaManager>();
        //isRotationBack = false;
        currentMode = Mode.NORMAL;
    }

    // Update is called once per frame
    void Update()
    {
        //if (inputManager.GetR_ButtonDown())
        //{
        //    currentMode = Mode.SCREW;
        //    //コルーチンで回転
        //    StartCoroutine(RotationUseScrew());
        //}

        //if (currentMode == Mode.SCREW)
        //{
        IsUseScrew = inputManager.GetR_Button();

        if (IsUseScrew)
        {
            //if (isRotationBack)
            //{
            GenerateScrew();
            screw.transform.position = transform.position;
            EnemyStanMove();
            //}
        }
        else
        {
            StopScrew();
            //StartCoroutine(RotationDefault());
        }
        //}
    }

    /// <summary>
    /// スクリューの生成
    /// </summary>
    private void GenerateScrew()
    {
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
        if (!existScrew) return;

        //パーティクルの生成を止める
        particlaManager.StopParticle(screw);
        //あたり判定をはずす
        screw.GetComponent<BoxCollider>().enabled = false;
        existScrew = false;
        EnemyStartRecovery();
        screw.transform.parent = null;
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
        foreach (var e in enemies)
        {
            e.GetComponent<SetUpScrew>().StanMove(transform.position);
        }
    }

    //private void RotationUseScrew()
    //{
    //    step = rotateSpeed * Time.deltaTime;
    //    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, 180), step);
    //    if (transform.rotation.eulerAngles.z <= 190)
    //    {
    //        isRotationBack = true;
    //    }
    //}

    //private void RotationNoUseScrew()
    //{
    //    step = rotateSpeed * Time.deltaTime;
    //    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, 0), step);
    //    if (transform.rotation.eulerAngles.z <= 10)
    //    {
    //        isRotationBack = false;
    //        currentMode = Mode.NORMAL;
    //    }
    //}

    //private IEnumerator RotationUseScrew()
    //{
    //    float rate = 0;

    //    while (true)
    //    {
    //        rate += Time.deltaTime / 10;
    //        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, 180), rate);

    //        yield return null;
    //    }
    //}

    //private IEnumerator RotationDefault()
    //{
    //    float rate = 0;

    //    while (true)
    //    {
    //        rate += Time.deltaTime / 10;
    //        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, 0), rate);

    //        yield return null;
    //    }
    //}
}
