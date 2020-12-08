using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectSceneManager : MonoBehaviour
{
    [SerializeField]
    private Transform scenes; //メニュー選択ボタン格納場所

    private int vLayer, hLayer; //メニューの階層
    private int selectNumber; //現在選択中のメニューナンバー
    [SerializeField]
    private float interval = 0.5f; //カーソル移動のインターバルフレーム
    private float vTimer, hTimer; //移動インターバル管理タイマー
    //private float eTimer;

    [SerializeField]
    private InputManager Input;
    [SerializeField]
    private FadeScene sceneManager;

    [SerializeField]
    private string[] sceneNames; //移動先シーンの名前

    private SoundManager soundManager;
    [SerializeField]
    private string bgm;
    [SerializeField]
    private string[] seList;
    private int currentNum, beforeNum; //サウンド再生フラグ用

    private Outline[] outlines; //ボタンの縁
    private Color[] colors; //点滅させる色
    private float colorTimer; //点滅速度管理タイマー
    [SerializeField]
    private int changeTime = 10; //点滅速度
    [SerializeField]
    private float changeSpeed = 5.0f;//カラー変更速度
    [SerializeField]
    private Text summary; //選択中のメニューの説明文
    [SerializeField, TextArea]
    private string[] summaries; //選択中のメニューの説明文

    private ScoreManager scoreManager;
    private int[] scores;

    [SerializeField]
    private Text scoreText;

    [SerializeField]
    private Font scoreFont, sentenceFont;
    private bool endFlag; //ゲーム終了フラグ

    // Start is called before the first frame update
    void Start()
    {
        selectNumber = 0;
        soundManager = SoundManager.Instance;

        //選択中演出に必要な物の用意
        outlines = new Outline[scenes.childCount];
        for (int i = 0; i < scenes.childCount; i++)
        {
            outlines[i] = scenes.GetChild(i).GetComponent<Outline>();
        }
        colors = new Color[6];
        colors[0] = Color.red;
        colors[1] = Color.yellow;
        colors[2] = Color.white;
        colors[3] = Color.green;
        colors[4] = Color.blue;
        colors[5] = Color.magenta;

        scoreManager = ScoreManager.Instance;
        scoreManager.LoadFile("Stage1");
        scores = scoreManager.GetScoreRanking();
        scoreText.enabled = false;

        if (FadeScene.GetBeforeSceneName() != "Title" && FadeScene.GetBeforeSceneName() != "Option")
            soundManager.PlayBgmByName(bgm);
    }

    // Update is called once per frame
    void Update()
    {
        if (sceneManager.IsFadeIn || sceneManager.IsFadeOut)
            return;

        Select(); //カーソルによるメニュー選択

        //Aかメニューボタンで選択中のシーンへ
        if (Input.GetA_ButtonDown() || Input.GetMenu_ButtonDown())
        {
            if (selectNumber == 5)
            {
                if (!endFlag)
                    endFlag = true;
                else
                {
                    soundManager.PlaySeByName(seList[1]);
                    sceneManager.GameQuitFadeOut();
                }
            }
            else
            {
                soundManager.PlaySeByName(seList[1]);
                sceneManager.ChangeNextScene(sceneNames[selectNumber]);
            }
        }

        //Bボタンでタイトルへ
        if (Input.GetB_ButtonDown())
        {
            if (endFlag)
                endFlag = false;
            else if (selectNumber != 4)
            {
                vLayer = 2;
                hLayer = 0;
                selectNumber = 4;
            }
            else if (selectNumber == 4)
            {
                soundManager.PlaySeByName(seList[2]);
                sceneManager.ChangeNextScene("Title");
            }
        }
    }

    private void Select()
    {
        vTimer += Time.deltaTime;
        hTimer += Time.deltaTime;
        float v = -Input.GetL_Stick_Vertical();
        float h = Input.GetL_Stick_Horizontal();
        float vAbs = Mathf.Abs(v);
        float hAbs = Mathf.Abs(h);


        //縦横それぞれの入力を反映
        if (vTimer >= interval && vAbs > 0.3f)
        {
            vLayer += (int)(1 * (v / vAbs)); //左スティックの入力を１か-1かで取る
                                             //オーバーフロー処理
            vLayer = (0 < vLayer) ? vLayer : 0;
            vLayer = (2 > vLayer) ? vLayer : 2;
            vTimer = 0;
        }
        if (hTimer >= interval && hAbs > 0.3f)
        {
            hLayer += (int)(1 * (h / hAbs)); //左スティックの入力を１か-1かで取る
                                             //オーバーフロー処理
            hLayer = (0 < hLayer) ? hLayer : 0;
            hLayer = (2 > hLayer) ? hLayer : 2;
            hTimer = 0;
        }

        //縦と横の番号に応じて選択番号を割り当てる
        switch (vLayer)
        {
            case 0:
                switch (hLayer)
                {
                    case 0:
                        selectNumber = 0;
                        break;
                    case 1:
                        selectNumber = 1;
                        break;
                    case 2:
                        selectNumber = 2;
                        break;
                }
                break;
            case 1:
                selectNumber = 3;
                hLayer = 0;
                break;
            case 2:
                switch (hLayer)
                {
                    case 0:
                        selectNumber = 4;
                        break;
                    case 1:
                        selectNumber = 5;
                        break;
                    case 2:
                        selectNumber = 5;
                        hLayer = 1;
                        break;
                }
                break;
        }

        if (selectNumber != 5)
            endFlag = false;
        if (v == 0)
            vTimer = interval;
        if (h == 0)
            hTimer = interval;

        currentNum = selectNumber;
        //1フレーム前と選択番号が違っていればカーソル移動音を流す
        if (currentNum != beforeNum)
        {
            soundManager.PlaySeByName(seList[0]);
        }
        beforeNum = selectNumber;
    }

    void FixedUpdate()
    {
        ActiveButton(selectNumber); //選択中のボタンの演出
    }

    //選択中のボタンの演出を書く
    private void ActiveButton(int number)
    {
        summary.enabled = true;
        //選択されていないボタンはデフォルトカラーに
        foreach (var o in outlines)
            o.effectColor = Color.white;

        //changeTime毎に選択中のボタンの縁を点滅させる
        colorTimer += Time.deltaTime * changeSpeed;
        if (colorTimer > changeTime * colors.Length - 1)
            colorTimer = 0;
        outlines[selectNumber].effectColor = colors[(int)colorTimer / changeTime];

        //テキストボックスにメニュー概要やスコアを表示
        if (number <= 2)
        {
            summary.font = scoreFont;
            scoreManager.LoadFile(sceneNames[number]);
            scores = scoreManager.GetScoreRanking();

            summary.text =
                "スコア\n\n" +
                "　　　1位\n\n" +
                "　　　2位\n\n" +
                "　　　3位\n\n" +
                "　　　4位\n\n" +
                "　　　5位";
            summary.fontSize = 40;
            scoreText.enabled = true;
            scoreText.text = "\n\n" + scores[0].ToString() + "\n\n" +
                scores[1].ToString() + "\n\n" +
                scores[2].ToString() + "\n\n" +
                scores[3].ToString() + "\n\n" +
                scores[4].ToString();
        }
        else if (endFlag)
        {
            summary.fontSize = 30;
            summary.text = "ゲームを終了してよろしいですか？\nA：はい　　B：いいえ";
        }
        else
        {
            summary.font = sentenceFont;
            summary.text = summaries[number];
            summary.fontSize = 35;
            scoreText.enabled = false;
        }
    }

    private void SetEnvisivle()
    {
        scoreText.enabled = false;
        summary.enabled = false;
    }
}
