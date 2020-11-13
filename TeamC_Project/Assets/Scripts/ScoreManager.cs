using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text;

[System.Serializable]
public class ScoreRanking
{
    public int[] bestScores;
    public int rank;
}

public class ScoreManager : SingletonMonoBehaviour<ScoreManager>
{
    private int totalScore;//合計スコア
    private int hiScore;
    ScoreRanking ranking = new ScoreRanking();
    string path;
    string fileName = "ranking_data.dat";

    private Text scoreText;//表示テキスト
    private Text hiScoreText;//表示テキスト

    protected override void Awake()
    {
        base.Awake();

        path = Application.persistentDataPath + "/"  + fileName;

        if (File.Exists(path))
            LoadScore();
        else
        {
            InitScoreRanking();
            SaveScore();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.Find("ScoreText") != null)
            scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
        if (GameObject.Find("HiScoreText") != null)
            hiScoreText = GameObject.Find("HiScoreText").GetComponent<Text>();
    }

    private void Update()
    {
        DisplayScore();
        DisplayHiScore();
    }

    /// <summary>
    /// スコアの初期化
    /// </summary>
    public void InitScore()
    {
        totalScore = 0;
        hiScore = ranking.bestScores[0];
        ranking.rank = -1;
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

        UpdateHiScore();
        hiScoreText.text = "HIScore: " + hiScore;
    }

    /// <summary>
    /// 合計スコアの取得
    /// </summary>
    /// <returns></returns>
    public int GetTotalScore()
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
    public void UpdateHiScore()
    {
        if (totalScore <= hiScore) return;

        hiScore = totalScore;
    }

    /// <summary>
    /// スコアランキングの更新
    /// </summary>
    public void UpdateScoreRanking()
    {
        int score = totalScore;

        for (int i = 0; i < ranking.bestScores.Length; i++)
        {
            if (score <= ranking.bestScores[i]) continue;

            if (ranking.rank == -1) ranking.rank = i;
            int s = ranking.bestScores[i];
            ranking.bestScores[i] = score;
            score = s;
        }

        SaveScore();
    }

    private void InitScoreRanking()
    {
        ranking.bestScores = new int[5];

        for (int i = 0; i < ranking.bestScores.Length; i++)
        {
            ranking.bestScores[i] = 1000 - (i * 100);
        }
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

        using (FileStream fileStream = File.Create(path))
        {
            fileStream.Write(data, 0, data.Length);
        }
    }

    /// <summary>
    /// スコアデータの読込
    /// </summary>
    public void LoadScore()
    {
        byte[] data = null;
        using (FileStream fileStream = File.OpenRead(path))
        {
            data = new byte[fileStream.Length];
            fileStream.Read(data, 0, data.Length);
        }

        //読み込むデータを複合化する
        data = Cryptor.Decrypt(data);
        string jsonstr = Encoding.UTF8.GetString(data);
        ranking = JsonUtility.FromJson<ScoreRanking>(jsonstr);
    }

    /// <summary>
    /// ランキングデータの取得
    /// </summary>
    /// <returns></returns>
    public int[] GetScoreRanking()
    {
        return ranking.bestScores;
    }

    /// <summary>
    /// ランクの取得
    /// </summary>
    /// <returns></returns>
    public int GetRank()
    {
        return ranking.rank;
    }
}
