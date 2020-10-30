using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
    [SerializeField]
    private int score = 100;//取得スコア

    /// <summary>
    /// スコアの取得
    /// </summary>
    /// <returns></returns>
    public int GetScore()
    {
        return score;
    }
}
