using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text;

public class ResultSceneManager : MonoBehaviour
{
    [SerializeField]
    private Text resultText;
    [SerializeField]
    private Text[] scoreTexts;

    private InputManager inputManager;
    [SerializeField]
    private FadeScene fadeScene;

    private ScoreManager scoreManager;
    private int[] scores;
    private int rank;

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
            fadeScene.ChangeNextScene("Title");
        }
    }

    private void DisplayRanking()
    {
        for(int i = 0; i < scoreTexts.Length; i++)
        {
            scoreTexts[i].text = (i + 1) + "位 " + scores[i];
            if (rank == i)
                scoreTexts[i].color = Color.red;
        }
    }
}
