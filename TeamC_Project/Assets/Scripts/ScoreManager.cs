using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text;

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
        filePath = Application.dataPath + "/ranking_data.dat";
#elif UNITY_STANDALONE
        filePath = Application.persistentDataPath + "/ranking_data.dat";
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

    /// <summary>
    /// ハイスコアの表示
    /// </summary>
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
        string jsonstr = JsonUtility.ToJson(ranking);
        //保存データを暗号化する
        byte[] data = Encoding.UTF8.GetBytes(jsonstr);
        data = Cryptor.Encrypt(data);

        FileStream fileStream = File.Create(filePath);
        fileStream.Write(data, 0, data.Length);
    }

    /// <summary>
    /// スコアデータの読込
    /// </summary>
    public void LoadScore()
    {
        byte[] data = null;

        FileStream fileStream = File.OpenRead(filePath);
        data = new byte[fileStream.Length];
        fileStream.Read(data, 0, data.Length);

        //読み込むデータを複合化する
        data = Cryptor.Decrypt(data);
        string jsonstr = Encoding.UTF8.GetString(data);
        ranking = JsonUtility.FromJson<ScoreRanking>(jsonstr);
    }
}
