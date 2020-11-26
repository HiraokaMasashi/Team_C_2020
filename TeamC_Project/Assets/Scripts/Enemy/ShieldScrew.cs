using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class ShieldScrew : MonoBehaviour
{
    private Health health;

    private Boss boss;

    [SerializeField]
    private float screwDamageInterval = 1.0f;
    [SerializeField]
    private float drillDamageInterval = 0.1f;
    private float elapsedTime;

    [SerializeField]
    private float moveSpeed = 1.0f;

    private void Start()
    {
        health = GetComponent<Health>();
        elapsedTime = 0.0f;
        boss = transform.root.GetComponent<Boss>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!boss.GetFrameIn()) return;

        if (other.transform.tag == "Screw")
        {
            int damage = 1;
            StartCoroutine(DamageComeOff(damage));
            health.Damage(damage);
        }

        if (other.transform.tag == "Drill")
        {
            if (!other.GetComponent<Drill>().IsShot)
            {
                int damage = 10;
                StartCoroutine(DamageComeOff(damage));
                health.Damage(damage);
            }
            else Destroy(other.gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == "Screw")
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime < screwDamageInterval) return;
            elapsedTime = 0.0f;

            int damage = 1;
            StartCoroutine(DamageComeOff(damage));
            health.Damage(damage);
        }

        if (other.transform.tag == "Drill")
        {
            if (other.GetComponent<Drill>().IsShot) return;

            elapsedTime += Time.deltaTime;
            if (elapsedTime < drillDamageInterval) return;
            elapsedTime = 0.0f;

            int damage = 10;
            StartCoroutine(DamageComeOff(damage));
            health.Damage(damage);
            other.transform.GetComponent<Health>().Damage(1);
        }
    }

    private IEnumerator DamageComeOff(int damage)
    {
        Vector3 destination = transform.position - new Vector3(0, 0.05f * damage, 0);

        while (Vector3.Distance(transform.position, destination) > 0.01f)
        {
            Vector3 position = Vector3.Lerp(transform.position, destination, Time.deltaTime * moveSpeed);
            transform.position = position;
            yield return null;
        }

        if (health.IsDead)
        {
            Destroy(gameObject);
        }
        yield break;
    }
}
