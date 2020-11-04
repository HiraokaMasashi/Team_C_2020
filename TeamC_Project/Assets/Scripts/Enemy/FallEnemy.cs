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
        Death();
    }

    protected override void Move()
    {
        base.Move();
    }
}
