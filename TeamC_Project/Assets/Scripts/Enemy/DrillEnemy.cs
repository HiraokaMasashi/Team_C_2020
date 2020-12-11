using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrillEnemy : Enemy
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        if (setupScrew.IsStan) return;

        Move();
        Death();
    }
}
