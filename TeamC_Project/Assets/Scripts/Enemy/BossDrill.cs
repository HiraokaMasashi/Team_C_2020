using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class BossDrill : MonoBehaviour
{
    private Health health;
    private ParticleManager particleManager;

    // Start is called before the first frame update
    void Start()
    {
        health = GetComponent<Health>();
        particleManager = GetComponent<ParticleManager>();
    }

    private void Update()
    {
        if (health.IsDead)
        {
            transform.parent.GetComponent<DrillBoss>().RemoveDrill();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Drill")
            other.gameObject.GetComponent<Drill>().DisConnect();

        if (other.transform.tag == "PlayerBullet")
        {
            health.Damage(1);
            GameObject particle = particleManager.GenerateParticle();
            particle.transform.position = other.ClosestPointOnBounds(transform.position);
            particleManager.OncePlayParticle(particle);
            if (health.IsDead)
            {
                transform.parent.GetComponent<DrillBoss>().SetRespawn();
                Destroy(gameObject);
            }
            other.GetComponent<Bullet>().Disconnect();
        }

        if(other.transform.tag == "Player")
        {
            if (transform.parent.GetComponent<Boss>().GetCurrentPattern() != Boss.BehaviourPattern.DRILL_ATTACK)
                return;

            other.GetComponent<Health>().Damage(1);
        }
    }
}
