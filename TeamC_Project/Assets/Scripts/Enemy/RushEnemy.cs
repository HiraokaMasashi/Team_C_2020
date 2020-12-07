using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RushEnemy : Enemy
{
    [SerializeField]
    private float rushSpeed = 2.0f;
    private bool isRush;
    [SerializeField]
    private float waitTime = 1.0f;
    private float waitElapedTime;

    private Vector3 direction;

    [SerializeField]
    private float standbyPositionY = 15.0f;

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

    public void SetWaitTime(int addWaitTime)
    {
        waitTime += addWaitTime;
    }
}
