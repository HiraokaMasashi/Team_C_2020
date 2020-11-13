using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class BossDrill : MonoBehaviour
{
    private Health health;
    //private Rigidbody rigid;
    //[SerializeField]
    //private float shotPower = 300.0f;

    //[SerializeField]
    //private Vector3 destroyZone = new Vector3(0, -12.0f, 0);

    // Start is called before the first frame update
    void Start()
    {
        health = GetComponent<Health>();
        //rigid = GetComponent<Rigidbody>();
    }

    //private void Update()
    //{
    //    DestroyObject();        
    //}

    //private void DestroyObject()
    //{
    //    if (transform.position.y <= destroyZone.y)
    //    {
    //        transform.parent.GetComponent<DrillBoss>().SetRespawn();
    //        Destroy(gameObject);
    //    }
    //}

    //public void ShotDrill()
    //{
    //    rigid.AddForce(Vector3.down * shotPower);
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "PlayerBullet")
        {
            health.Damage(1);
            if (health.IsDead)
            {
                transform.parent.GetComponent<DrillBoss>().SetRespawn();
                Destroy(gameObject);
            }
            other.GetComponent<BulletCollision>().Disconnect();
        }
    }
}
