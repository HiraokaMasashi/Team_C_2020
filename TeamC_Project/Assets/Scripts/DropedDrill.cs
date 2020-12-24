using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropedDrill : MonoBehaviour
{
    [SerializeField]
    float moveSpeed = 1.0f;
    [SerializeField]
    protected Vector3 destroyZone = new Vector3(0.0f, -11.0f, 0.0f);

    [SerializeField]
    private float rotateSpeed = 0.5f;

    void Update()
    {
        Move();
        Death();
        Rotate();
    }

    private void Move()
    {
        Vector3 position = transform.position;
        position += Vector3.down * moveSpeed * Time.deltaTime;
        transform.position = position;
    }

    private void Death()
    {
        if (transform.position.y <= destroyZone.y)
        {
            Destroy(gameObject);
        }
    }

    private void Rotate()
    {
        transform.rotation *= Quaternion.Euler(0, 360 * Time.deltaTime * rotateSpeed, 0);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            if (other.transform.GetComponent<Player>().IsEquipmentDrill || 
                other.transform.GetComponent<Player>().GetCurrentMode() != Player.Mode.NORMAL) return;

            other.transform.GetComponent<Player>().EquipmentDrill();
            Destroy(gameObject);
        }
    }
}
