using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiSocreEnemy : Enemy
{
    [SerializeField]
    private float waitTime = 1.0f;//待機時間
    [SerializeField]
    private float waitSpeed = 2.0f;//待機速度
    private float waitElepsedTime;

    [SerializeField]
    private float withdrawalZoneY = 22.0f;//撤退位置

    [SerializeField]
    private float withdrawalSpeed = 20.0f;//撤退時の速度
    private bool isWithdrawal;//撤退中か
    [SerializeField]
    private float standbyPositionY = 15.0f;//準備位置

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        isWithdrawal = false;
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (setupScrew.IsStan) return;

        StandbyWithdrawal();
        Move();
    }

    protected override void Move()
    {
        if (!isWithdrawal) return;

        Vector3 position = transform.position;
        position += Vector3.up * withdrawalSpeed * Time.deltaTime;
        transform.position = position;

        if (transform.position.y >= withdrawalZoneY)
            Destroy(gameObject);
    }

    /// <summary>
    /// 撤退準備
    /// </summary>
    private void StandbyWithdrawal()
    {
        if (isWithdrawal) return;

        if(transform.position.y >= standbyPositionY)
        {
            Vector3 position = transform.position;
            position += Vector3.down * moveSpeed * Time.deltaTime;
            transform.position = position;
            return;
        }

        waitElepsedTime += Time.deltaTime * waitSpeed;
        if (waitElepsedTime < waitTime) return;

        isWithdrawal = true;
    }
}
