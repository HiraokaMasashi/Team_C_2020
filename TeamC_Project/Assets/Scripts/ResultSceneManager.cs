using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultSceneManager : MonoBehaviour
{
    [SerializeField]
    private Text resultText;
    [SerializeField]
    private Text[] scoreTexts;
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
    private string se;

    // Start is called before the first frame update
    void Start()
    {
        inputManager = GameObject.Find("InputManager").GetComponent<InputManager>();

        if (GameManager.GetResult() == GameManager.ResultMode.GAMECLEAR)
        {
            resultText.text = "Game Clear!";
        }
        else if (GameManager.GetResult() == GameManager.ResultMode.GAMEOVER)
        {
            resultText.text = "Game Over..";
        }

        scoreManager = ScoreManager.Instance;
        scores = scoreManager.GetScoreRanking();
        rank = scoreManager.GetRank();
        soundManager = SoundManager.Instance;
        soundManager.PlayBgmByName(bgm);
    }

    // Update is called once per frame
    void Update()
    {
        NextScene();
        DisplayRanking();
    }

    private void NextScene()
    {
        if (inputManager.GetA_ButtonDown())
        {
            soundManager.PlaySeByName(se);
            fadeScene.ChangeNextScene("Title");
        }
    }

    private void DisplayRanking()
    {
        for(int i = 0; i < scoreTexts.Length; i++)
        {
            scoreTexts[i].text = (i + 1) + "位 " + scores[i].ToString("D5");
            if (rank == i)
                scoreTexts[i].color = Color.red;
        }

        //ビルドだとテキストの表示がおかしくなるため
        hiScoreText.text = "1位 " + scores[0].ToString("D5");
        if (rank == 0) hiScoreText.color = Color.red;
    }
}
