using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

    [SerializeField]
    private Slider[] playerHPGauges;
    [SerializeField]
    private Image[] playerHPImages;
    //private Text hpText;
    [SerializeField]
    private BossHPGauge bossHPGauge;

    private static int stageNumber;

    private GameObject player;


    [SerializeField]
    private Vector3 playerStartPosition = new Vector3(0, -10, 0);
    [SerializeField]
    private Vector3 playerDestinationPosition = new Vector3(0, 0, 0);//プレイヤーの開始地点
    [SerializeField]
    private float speed = 10;//開始地点まで移動させるプレイヤーのスピード
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

    public bool IsPerformance
    {
        get;
        set;
    } = false;

    // Start is called before the first frame update
    void Start()
    {
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
        result = ResultMode.NONE;
        stageNumber = SceneManager.GetActiveScene().buildIndex;
        Debug.Log(stageNumber);

        scoreManager = ScoreManager.Instance;
        scoreManager.InitScore();


        //Playerが生成される処理ではなかったのでScene内のPlayerを使用
        player = GameObject.Find("Player");
        player = GameObject.FindGameObjectWithTag("Player");
        
        player.transform.position = playerStartPosition;

        ///
        //IsGameStart = false;
    }

    // Update is called once per frame
    void Update()
    {
        GameStart();
        DisplayPlayerHP();
        bossHPGauge.DisplayGauge();
        //DisplayBossHP();
        ChangeResultScene();
    }

    /// <summary>
    /// ゲームの開始処理
    /// </summary>
    private void GameStart()
    {
        if (IsGameStart) return;
        if (fadeScene.IsFadeIn) return;

        Vector3 vec = (playerDestinationPosition - player.transform.position).normalized;
        //プレイヤーを目的地まで移動させる
        player.transform.position += vec * speed * Time.deltaTime;

        //Start地点に到着したら
        if (Vector3.Distance(player.transform.position, playerDestinationPosition) <= 0.1f)
        {
            IsGameStart = true;            
        }

    }

    /// <summary>
    /// リザルトへのシーン移行
    /// </summary>
    private void ChangeResultScene()
    {
        if (IsPerformance) return;

        if (!IsEnd)
        {
            SetResultGameOver();
            SetResultGameClear();
        }

        if (IsEnd && !IsPerformance)
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
            bossHPGauge.gameObject.SetActive(false);
            result = ResultMode.GAMECLEAR;
            //IsPerformance = true;
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
        bossHPGauge.SetBossHealth(bossHealth);
        bossHPGauge.gameObject.SetActive(true);
        //bossHPSlider.gameObject.SetActive(true);
        //bossHPSlider.maxValue = bossHealth.Hp;
    }

    private void DisplayPlayerHP()
    {
        //if (hpText == null) return;

        //hpText.text = "HP: " + playerHealth.Hp + " / " + playerHealth.MaxHp;

        //if (playerHealth.Hp <= playerHealth.MaxHp * (1.0f / 3.0f))
        //    hpText.color = Color.red;
        //else if (playerHealth.Hp <= playerHealth.MaxHp * (2.0f / 3.0f))
        //    hpText.color = Color.yellow;
        //else
        //    hpText.color = Color.green;

        if (playerHPGauges.Length == 0) return;

        for (int i = 0; i < playerHealth.MaxHp; i++)
        {
            if (i < playerHealth.Hp)
                playerHPGauges[i].value = 1;
            else
                playerHPGauges[i].value = 0;

            Color hpColor = Color.green;
            if (playerHealth.Hp <= playerHealth.MaxHp * (1.0f / 3.0f))
                hpColor = Color.red;
            else if (playerHealth.Hp <= playerHealth.MaxHp * (2.0f / 3.0f))
                hpColor = Color.yellow;

            playerHPImages[i].color = hpColor;
        }
    }

    //private void DisplayBossHP()
    //{
    //    if (bossHealth == null) return;
    //    if (bossHPSlider.value >= bossHealth.Hp)
    //    {
    //        bossHPSlider.value = bossHealth.Hp;
    //        return;
    //    }

    //    bossHPSlider.value += 1;
    //}

    public static int GetStageNumber()
    {
        return stageNumber;
    }
}
