using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetUpScrew : MonoBehaviour
{
    private float hitTimer;

    [SerializeField]
    private float recoveryTime = 6.0f;
    private float stanElapsedTime;

    public bool IsStan
    {
        get;
        private set;
    } = false;

    public bool IsHitScrew
    {
        get;
        private set;
    } = false;

    // Start is called before the first frame update
    void Start()
    {
        hitTimer = 0.0f;
        stanElapsedTime = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        StanMove();
        Recovery();
    }

    /// <summary>
    /// スタン回復処理
    /// </summary>
    private void Recovery()
    {
        if (!IsStan) return;
        if (IsHitScrew) return;

        stanElapsedTime += Time.deltaTime;
        Debug.Log("スタン中");
        if (stanElapsedTime >= recoveryTime)
        {
            Debug.Log("スタン回復");
            IsStan = false;
            stanElapsedTime = 0.0f;
        }
    }

    /// <summary>
    /// スタン中の敵の並べ替え
    /// </summary>
    private void StanMove()
    {

    }

    /// <summary>
    /// スクリューとの衝突時の処理
    /// </summary>
    /// <param name="distance"></param>
    public void HitScrew(float distance)
    {
        Debug.Log(distance.ToString("f2"));
        IsStan = true;
        IsHitScrew = true;
    }

    /// <summary>
    /// スクリューとの衝突終了時の処理
    /// </summary>
    public void LeaveScrew()
    {
        IsHitScrew = false;
    }
}
