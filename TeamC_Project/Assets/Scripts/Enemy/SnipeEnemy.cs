using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnipeEnemy : Enemy
{
    private float stopPositionY;
    [SerializeField]
    private float minStopPositionY = 10.0f;
    [SerializeField]
    private float maxStopPositionY = 12.0f;

    protected override void Start()
    {
        base.Start();
        stopPositionY = Random.Range(minStopPositionY, maxStopPositionY);
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (setupScrew.IsStan) return;

        Move();
        ShotBullet();
    }

    protected override void Move()
    {
        //一定の位置まで進むと止まる
        if (transform.position.y <= stopPositionY) return;

        Vector3 position = transform.position;
        position += Vector3.down * moveSpeed * Time.deltaTime;
        transform.position = position;
    }

    protected override void ShotBullet()
    {
        base.ShotBullet();
    }
}
