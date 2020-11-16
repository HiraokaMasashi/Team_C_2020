using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrillEnemy : Enemy
{
    //[SerializeField]
    //GameObject drill;

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

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.transform.tag == "PlayerBullet")
    //    {
    //        Instantiate(drill, transform.position, Quaternion.identity);
    //    }
    //}
}
