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

    [SerializeField]
    private float moveSpeed = 1.0f;

    private void Start()
    {
        health = GetComponent<Health>();
        elapsedTime = 0.0f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Screw")
        {
            int damage = 10;
            StartCoroutine(DamageComeOff(damage));
            health.Damage(damage);
        }

        if (other.transform.tag == "Drill")
        {
            int damage = 10;
            StartCoroutine(DamageComeOff(damage));
            health.Damage(damage);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == "Screw")
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime < damageInterval) return;
            elapsedTime = 0.0f;

            int damage = 10;
            StartCoroutine(DamageComeOff(damage));
            health.Damage(damage);
        }
    }

    private IEnumerator DamageComeOff(int damage)
    {
        Vector3 destination = transform.position - new Vector3(0, 0.1f * damage, 0);

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
