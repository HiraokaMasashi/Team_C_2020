using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

[System.Serializable]
public class ScoreRanking
{
    public int bestScore;
}

public class ScoreManager : MonoBehaviour
{
    private static int totalScore;//合計スコア
    ScoreRanking ranking = new ScoreRanking();
    string filePath;

    [SerializeField]
    private Text scoreText;//表示テキスト
    [SerializeField]
    private Text hiScoreText;//表示テキスト

    private void Awake()
    {
#if UNITY_EDITOR
        filePath = Application.dataPath + "/ranking_data.json";
#elif UNITY_STANDALONE
        filePath = Application.persistentDataPath + "/ranking_data.json";
#endif

        if (File.Exists(filePath))
            LoadScore();
        else
        {
            ranking.bestScore = 0;
            SaveScore();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        totalScore = 0;
    }

    private void Update()
    {
        DisplayScore();
        DisplayHiScore();
    }

    /// <summary>
    /// スコアの表示
    /// </summary>
    private void DisplayScore()
    {
        if (scoreText == null) return;

        scoreText.text = "Score: " + totalScore;
    }

    private void DisplayHiScore()
    {
        if (hiScoreText == null) return;

        UpdateScore();
        hiScoreText.text = "HIScore: " + ranking.bestScore;
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

    /// <summary>
    /// ベストスコアの更新
    /// </summary>
    public void UpdateScore()
    {
        if (totalScore <= ranking.bestScore) return;

        ranking.bestScore = totalScore;
    }

    /// <summary>
    /// スコアデータの保存
    /// </summary>
    public void SaveScore()
    {
        StreamWriter writer;
        string jsonstr = JsonUtility.ToJson(ranking);

        writer = new StreamWriter(filePath);
        writer.Write(jsonstr);
        writer.Flush();
        writer.Close();
    }

    /// <summary>
    /// スコアデータの読込
    /// </summary>
    public void LoadScore()
    {
        StreamReader reader;
        reader = new StreamReader(filePath);
        string data = reader.ReadToEnd();
        reader.Close();

        ranking = JsonUtility.FromJson<ScoreRanking>(data);
    }
}
