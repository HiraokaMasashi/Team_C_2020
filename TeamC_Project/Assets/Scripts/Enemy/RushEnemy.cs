using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RushEnemy : Enemy
{
    [SerializeField]
    private float rushSpeed = 2.0f;//突進速度
    private bool isRush;//突進中か
    [SerializeField]
    private float waitTime = 1.0f;//待機時間
    private float waitElapedTime;

    private Vector3 direction;

    [SerializeField]
    private float standbyPositionY = 15.0f;//準備位置

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        isRush = false;
        waitElapedTime = 0;
        direction = Vector3.zero;
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (setupScrew.IsStan) return;

        RushStandby();
        Move();
        Death();
    }

    /// <summary>
    /// 進行方向設定
    /// </summary>
    private void SetDirection()
    {
        if (direction != Vector3.zero) return;

        direction = (player.transform.position - transform.position).normalized;
    }

    protected override void Move()
    {
        Vector3 position = transform.position;
        position += direction * rushSpeed * Time.deltaTime;
        transform.position = position;
    }

    /// <summary>
    /// 突進準備
    /// </summary>
    private void RushStandby()
    {
        if (isRush) return;

        if(transform.position.y > standbyPositionY)
        {
            Vector3 position = transform.position;
            position += Vector3.down * moveSpeed * Time.deltaTime;
            transform.position = position;
            return;
        }

        waitElapedTime += Time.deltaTime;
        if (waitElapedTime < waitTime) return;

        SetDirection();
        isRush = true;
    }

    /// <summary>
    /// 待機時間の設定
    /// </summary>
    /// <param name="addWaitTime"></param>
    public void SetWaitTime(int addWaitTime)
    {
        waitTime += addWaitTime;
    }
}
