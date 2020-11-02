using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetUpScrew : MonoBehaviour
{
    public enum StanLevel
    {
        LEVEL1,
        LEVEL2,
        LEVEL3,
    }
    private StanLevel stanLevel;

    private float hitTimer;
    [SerializeField]
    private float[] hitTimerLevel;

    [SerializeField]
    private float recoveryTime = 6.0f;
    private float stanElapsedTime;

    private float distanceY;
    private float adjustPositionX;

    [SerializeField]
    private float speed = 1.0f;

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
        stanLevel = StanLevel.LEVEL1;
        hitTimer = 0.0f;
        stanElapsedTime = 0.0f;
        distanceY = 0.0f;
        adjustPositionX = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 0) return;

        Recovery();
        StanLevelUp();
    }

    /// <summary>
    /// スタン回復処理
    /// </summary>
    private void Recovery()
    {
        if (!IsStan) return;
        if (IsHitScrew) return;

        stanElapsedTime += Time.deltaTime;
        if (stanElapsedTime >= recoveryTime)
        {
            IsStan = false;
            stanElapsedTime = 0.0f;
        }
    }

    /// <summary>
    /// スタン中の敵の並べ替え(移動)
    /// </summary>
    public void StanMove(Vector3 basePosition)
    {
        Vector3 position = new Vector3(transform.position.x, basePosition.y + distanceY, transform.position.z);
        Vector3 destination = new Vector3(basePosition.x + adjustPositionX, basePosition.y + distanceY, transform.position.z);
        float distance = Vector3.Distance(position, destination);
        float currentLocation = (Time.deltaTime * speed * 2.0f) / distance;

        transform.position = Vector3.Lerp(position, destination, currentLocation);
    }

    /// <summary>
    /// スクリューとの衝突時の処理
    /// </summary>
    /// <param name="distance"></param>
    public void HitScrew(float distance, float boxSizeY)
    {
        IsStan = true;
        IsHitScrew = true;
        distanceY = distance;
        AddTimer(boxSizeY);
        adjustPositionX = Random.Range(-1.0f, 1.0f);
    }

    /// <summary>
    /// 距離によるボーナス加算
    /// </summary>
    /// <param name="boxSizeY"></param>
    private void AddTimer(float boxSizeY)
    {
        //最大範囲の1/3の距離なら、2秒加算
        if (distanceY <= boxSizeY * (1.0f/3.0f))
            hitTimer = 2.0f;
        //2/3の距離なら、1秒加算
        else if (distanceY <= boxSizeY * (2.0f / 3.0f))
            hitTimer = 1.0f;
    }

    /// <summary>
    /// スクリューとの衝突終了時の処理
    /// </summary>
    public void LeaveScrew()
    {
        IsHitScrew = false;
        hitTimer = 0.0f;
    }

    /// <summary>
    /// スタンレベルの設定
    /// </summary>
    private void StanLevelUp()
    {
        if (!IsHitScrew) return;
        if (stanLevel == StanLevel.LEVEL3) return;

        hitTimer += Time.deltaTime;

        if (hitTimer >= hitTimerLevel[2])
            stanLevel = StanLevel.LEVEL3;
        else if (hitTimer >= hitTimerLevel[1])
            stanLevel = StanLevel.LEVEL2;
        else
            stanLevel = StanLevel.LEVEL1;

        Debug.Log(transform.name + ":" + stanLevel);
    }

    public StanLevel GetStanLevel()
    {
        return stanLevel;
    }
}
