using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private Health playerHealth;//プレイヤーの体力スクリプト
    private Health bossHealth;//ボスの体力スクリプト
    [SerializeField]
    private Slider bossHPSlider;

    [SerializeField]
    private FadeScene fadeScene;//フェードスクリプト

    [SerializeField]
    private float changeSceneTime = 2.0f;

    private ScoreManager scoreManager;


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

    public bool IsEnd
    {
        get;
        private set;
    } = false;//ゲームが終わっているか

    // Start is called before the first frame update
    void Start()
    {
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
        result = ResultMode.NONE;

        scoreManager = ScoreManager.Instance;
        scoreManager.InitScore();
    }

    // Update is called once per frame
    void Update()
    {
        GameStart();
        DisplayBossHP();
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
        if (!IsEnd)
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
            IsEnd = true;
            scoreManager.UpdateScoreRanking();
        }
    }

    private void SetResultGameClear()
    {
        if (bossHealth == null) return;
        if (result != ResultMode.NONE) return;

        if (bossHealth.IsDead)
        {
            bossHPSlider.gameObject.SetActive(false);
            result = ResultMode.GAMECLEAR;
            IsEnd = true;
            scoreManager.UpdateScoreRanking();
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
        bossHPSlider.gameObject.SetActive(true);
        bossHPSlider.maxValue = bossHealth.Hp;
    }

    private void DisplayBossHP()
    {
        if (bossHealth == null) return;

        bossHPSlider.value = bossHealth.Hp;
    }
}
