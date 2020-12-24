using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrillEnemy : Enemy
{
    private GameObject drill;
    [SerializeField]
    private float rotateSpeed=1.0f;

    protected override void Start()
    {
        base.Start();
        drill = transform.GetChild(0).gameObject;
    }

    protected override void Update()
    {
        if (setupScrew.IsStan) return;

        Move();
        Death();
        RotateDrill();
    }

    private void RotateDrill()
    {
        drill.transform.rotation *= Quaternion.Euler(0, 360 * Time.deltaTime * rotateSpeed, 0);
    }
}
