using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Health playerHealth;//プレイヤーの体力スクリプト
    [SerializeField]
    private Health bossHealth;//ボスの体力スクリプト

    [SerializeField]
    private FadeScene fadeScene;//フェードスクリプト

    [SerializeField]
    private float changeSceneTime = 2.0f;

    public enum ResultMode
    {
        NONE,
        GAMECLEAR,
        GAMEOVER,
    }
    private static ResultMode result;//ゲームリザルト

    public bool IsGameStart
    {
        get;
        private set;
    } = false;//ゲームが開始しているか
    private bool isEnd;//ゲームが終わっているか

    // Start is called before the first frame update
    void Start()
    {
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
        isEnd = false;
        result = ResultMode.NONE;
    }

    // Update is called once per frame
    void Update()
    {
        GameStart();
        ChangeResultScene();
    }

    /// <summary>
    /// ゲームの開始処理
    /// </summary>
    private void GameStart()
    {
        if (IsGameStart) return;

        if (!fadeScene.IsFadeIn)
        {
            IsGameStart = true;
        }
    }

    /// <summary>
    /// リザルトへのシーン移行
    /// </summary>
    private void ChangeResultScene()
    {
        if (!isEnd)
        {
            SetResultGameOver();
            SetResultGameClear();
        }
        else
        {
            changeSceneTime -= Time.deltaTime;
            if (changeSceneTime > 0.0f) return;

            fadeScene.ChangeNextScene("Result");
        }
    }

    private void SetResultGameOver()
    {
        if (result != ResultMode.NONE) return;

        if (playerHealth.IsDead)
        {
            result = ResultMode.GAMEOVER;
            isEnd = true;
        }
    }

    private void SetResultGameClear()
    {
        if (bossHealth == null) return;
        if (result != ResultMode.NONE) return;

        if (bossHealth.IsDead)
        {
            result = ResultMode.GAMECLEAR;
            isEnd = true;
        }
    }

    /// <summary>
    /// ゲームリザルトの取得
    /// </summary>
    /// <returns></returns>
    public static ResultMode GetResult()
    {
        return result;
    }

    /// <summary>
    /// ボス体力スクリプトの設定
    /// </summary>
    /// <param name="bossEnemy"></param>
    public void SetBossEnemy(GameObject bossEnemy)
    {
        bossHealth = bossEnemy.GetComponent<Health>();
    }
}
