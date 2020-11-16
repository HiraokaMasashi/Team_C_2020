using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropedDrill : MonoBehaviour
{
    [SerializeField]
    float moveSpeed = 1.0f;
    [SerializeField]
    protected Vector3 destroyZone = new Vector3(0.0f, -11.0f, 0.0f);

    void Update()
    {
        Move();
        Death();
    }

    void Move()
    {
        Vector3 position = transform.position;
        position += Vector3.down * moveSpeed * Time.deltaTime;
        transform.position = position;
    }

    void Death()
    {
        if (transform.position.y <= destroyZone.y)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            if (other.transform.GetComponent<Player>().IsEquipmentDrill) return;

            other.transform.GetComponent<Player>().EquipmentDrill();
            Destroy(gameObject);
        }
    }
}
