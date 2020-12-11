using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallEnemy : Enemy
{
    [SerializeField]
    private float minSpeed = 1.0f;
    [SerializeField]
    private float maxSpeed = 3.0f;

    protected override void Start()
    {
        base.Start();

        moveSpeed = Random.Range(minSpeed, maxSpeed);
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (setupScrew.IsStan) return;

        Move();
        ShotBullet();
        Death();
    }

    protected override void Move()
    {
        base.Move();
    }

    protected override void ShotBullet()
    {
        if (player == null) return;

        elapsedTime += Time.deltaTime;

        if (elapsedTime >= shotInterval)
        {
            Vector3 position = transform.position + Vector3.down;
            Vector3 direction = Vector3.down;
            bulletController.GenerateBullet(position, direction, 3.0f, "Enemy");
            elapsedTime = 0;
        }
    }
}
