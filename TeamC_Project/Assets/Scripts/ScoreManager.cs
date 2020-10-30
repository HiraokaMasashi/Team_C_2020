using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    private static int totalScore;//合計スコア

    [SerializeField]
    private Text scoreText;//表示テキスト

    // Start is called before the first frame update
    void Start()
    {
        totalScore = 0;
    }

    private void Update()
    {
        DisplayScore();
    }

    /// <summary>
    /// スコアの表示
    /// </summary>
    public void DisplayScore()
    {
        if (scoreText == null) return;

        scoreText.text = "Score: " + totalScore;
    }

    /// <summary>
    /// 合計スコアの取得
    /// </summary>
    /// <returns></returns>
    public static int GetTotalScore()
    {
        return totalScore;
    }

    /// <summary>
    /// スコアの加算
    /// </summary>
    /// <param name="score">加算するスコア</param>
    public void AddScore(int score)
    {
        totalScore += score;
    }
}
