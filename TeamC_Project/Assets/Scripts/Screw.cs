using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticlaManager))]
public class Screw : MonoBehaviour
{
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

    // Start is called before the first frame update
    void Start()
    {
        inputManager = GameObject.Find("InputManager").GetComponent<InputManager>();
        particlaManager = GetComponent<ParticlaManager>();
    }

    // Update is called once per frame
    void Update()
    {
        IsUseScrew = inputManager.GetR_Button();

        if (IsUseScrew)
        {
            Debug.Log("スクリュー使用中");
            screw.transform.position = transform.position;
            GenerateScrew();
        }
        else
        {
            StopScrew();
        }
    }

    /// <summary>
    /// スクリューの生成
    /// </summary>
    private void GenerateScrew()
    {
        if (existScrew) return;

        //スクリューパーティクルの生成
        screw = particlaManager.GenerateParticleInChildren();
        screw.transform.rotation = transform.rotation;
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
    }

    /// <summary>
    /// 敵のスタン回復処理を実行
    /// </summary>
    private void EnemyStartRecovery()
    {
        List<GameObject> enemies = transform.GetChild(0).GetComponent<ScrewCollision>().GetEnemies();

        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].GetComponent<SetUpScrew>().LeaveScrew();
        }
    }
}
