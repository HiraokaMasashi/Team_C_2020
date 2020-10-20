using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetUpScrew : MonoBehaviour
{
    private float hitTimer;

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
        hitTimer = 0.0f;
        stanElapsedTime = 0.0f;
        distanceY = 0.0f;
        adjustPositionX = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
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
        if (stanElapsedTime >= recoveryTime)
        {
            Debug.Log("スタン回復");
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
        float currentLocation = (Time.deltaTime * speed) / distance;

        transform.position = Vector3.Lerp(position, destination, currentLocation);
    }

    /// <summary>
    /// スクリューとの衝突時の処理
    /// </summary>
    /// <param name="distance"></param>
    public void HitScrew(float distance)
    {
        IsStan = true;
        IsHitScrew = true;
        distanceY = distance;
        adjustPositionX = Random.Range(-1.0f, 1.0f);
    }

    /// <summary>
    /// スクリューとの衝突終了時の処理
    /// </summary>
    public void LeaveScrew()
    {
        IsHitScrew = false;
    }
}
