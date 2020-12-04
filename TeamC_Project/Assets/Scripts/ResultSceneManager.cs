using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResultSceneManager : MonoBehaviour
{
    [SerializeField]
    private Text resultText;
    [SerializeField]
    private Text[] scoreTexts;
    [SerializeField]
    private Text[] rankTexts;
    [SerializeField]
    private Text hiScoreText;

    private InputManager inputManager;
    [SerializeField]
    private FadeScene fadeScene;

    private ScoreManager scoreManager;
    private int[] scores;
    private int rank;

    private SoundManager soundManager;
    [SerializeField]
    private string bgm;

    [SerializeField]
    private Text getScoreText;

    [SerializeField]
    private string[] seList;
    private int currentNum, beforeNum; //サウンド再生フラグ用

    [SerializeField]
    private Transform[] activeButtons;
    private Transform activeButton;
    private GameObject[] buttonNames;
    private int select;
    private int selectNumber; //現在選択中のメニューナンバー
    [SerializeField]
    private float interval = 0.5f; //カーソル移動のインターバルフレーム
    private float timer;

    private Outline[] outlines; //ボタンの縁
    private Color[] colors; //点滅させる色
    private float colorTimer; //点滅速度管理タイマー
    [SerializeField]
    private int changeTime = 10; //点滅速度
    [SerializeField]
    private float changeSpeed = 5.0f;//カラー変更速度

    // Start is called before the first frame update
    void Start()
    {
        inputManager = GameObject.Find("InputManager").GetComponent<InputManager>();

        if (GameManager.GetResult() == GameManager.ResultMode.GAMECLEAR)
        {
            resultText.text = "Game Clear!";
            if (FadeScene.GetBeforeSceneName() != "Stage3")
                activeButton = activeButtons[0];
            else
                activeButton = activeButtons[1];
        }
        else if (GameManager.GetResult() == GameManager.ResultMode.GAMEOVER)
        {
            resultText.text = "Game Over..";
            activeButton = activeButtons[1];
        }

        activeButton.gameObject.SetActive(true);
        //選択中演出に必要な物の用意
        outlines = new Outline[activeButton.childCount];
        buttonNames = new GameObject[activeButton.childCount];
        for (int i = 0; i < activeButton.childCount; i++)
        {
            outlines[i] = activeButton.GetChild(i).GetComponent<Outline>();
            buttonNames[i] = activeButton.GetChild(i).gameObject;
        }
        colors = new Color[6];
        colors[0] = Color.red;
        colors[1] = Color.yellow;
        colors[2] = Color.white;
        colors[3] = Color.green;
        colors[4] = Color.blue;
        colors[5] = Color.magenta;

        scoreManager = ScoreManager.Instance;
        scores = scoreManager.GetScoreRanking();
        rank = scoreManager.GetRank();
        soundManager = SoundManager.Instance;
        soundManager.PlayBgmByName(bgm);
    }

    // Update is called once per frame
    void Update()
    {
        Select();
        ActiveButton(selectNumber);

        NextScene();
        DisplayRanking();
    }

    private void NextScene()
    {
        string nextScene = "Select";
        if (inputManager.GetA_ButtonDown())
        {
            soundManager.PlaySeByName(seList[1]);
            if (buttonNames[selectNumber].name == "NextStage")
            {
                if (FadeScene.GetBeforeSceneName() == "Stage1")
                    nextScene = "Stage2";
                else if (FadeScene.GetBeforeSceneName() == "Stage2")
                    nextScene = "Stage3";
            }
            else if (buttonNames[selectNumber].name == "Retry")
                nextScene = FadeScene.GetBeforeSceneName();

            fadeScene.ChangeNextScene(nextScene);
        }
    }

    private void DisplayRanking()
    {

        for (int i = 0; i < scoreTexts.Length; i++)
        {
            scoreTexts[i].text = scores[i].ToString();
            if (rank == i)
            {
                scoreTexts[i].color = Color.red;
                rankTexts[i].color = Color.red;
            }
        }

        //ビルドだとテキストの表示がおかしくなるため
        hiScoreText.text = scores[0].ToString();
        if (rank == 0)
        {
            hiScoreText.color = Color.red;
            rankTexts[0].color = Color.red;
        }

        getScoreText.text = scoreManager.GetTotalScore().ToString();
    }

    private void Select()
    {
        currentNum = selectNumber;
        timer += Time.deltaTime;
        float h = inputManager.GetL_Stick_Horizontal();
        float hAbs = Mathf.Abs(h);

        //縦横それぞれの入力を反映
        if (timer >= interval && hAbs > 0.3f)
        {
            select += (int)(1 * (h / hAbs)); //左スティックの入力を１か-1かで取る
            //オーバーフロー処理
            select = (0 < select) ? select : 0;
            select = (activeButton.childCount - 1 > select) ? select : activeButton.childCount - 1;
            timer = 0;
        }

        selectNumber = select;

        if (h == 0)
            timer = interval;
        //1フレーム前と選択番号が違っていればカーソル移動音を流す
        if (currentNum != beforeNum)
            soundManager.PlaySeByName(seList[0]);
        beforeNum = selectNumber;
    }

    private void ActiveButton(int selectNumber)
    {
        //選択されていないボタンはデフォルトカラーに
        foreach (var o in outlines)
            o.effectColor = Color.white;

        //changeTime毎に選択中のボタンの縁を点滅させる
        colorTimer += Time.deltaTime * changeSpeed;
        if (colorTimer > changeTime * colors.Length - 1)
            colorTimer = 0;
        outlines[selectNumber].effectColor = colors[(int)colorTimer / changeTime];

    }
}
