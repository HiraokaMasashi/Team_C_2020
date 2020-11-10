using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class ShieldScrew : MonoBehaviour
{
    private Health health;

    [SerializeField]
    private float damageInterval = 1.0f;
    private float elapsedTime;

    private void Start()
    {
        health = GetComponent<Health>();
        elapsedTime = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        ComeOff();   
    }

    private void ComeOff()
    {
        if (health.IsDead)
            Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Screw")
        {
            health.Damage(1);
        }

        if(other.transform.tag == "Drill")
        {
            health.Damage(10);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.transform.tag == "Screw")
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime < damageInterval) return;
            elapsedTime = 0.0f;
            health.Damage(1);
        }
    }
}
