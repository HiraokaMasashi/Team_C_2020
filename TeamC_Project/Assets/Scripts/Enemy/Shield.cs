using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [SerializeField]
    private Vector3 destination = new Vector3(0, -4, 0);
    [SerializeField]
    private float moveSpeed = 1.0f;

    private bool isDown = false;

    // Update is called once per frame
    void Update()
    {
        ComeOff();
        MoveDown();
    }

    private void ComeOff()
    {
        if (transform.childCount <= 2)
        {
            if (isDown) return;

            isDown = true;
            transform.parent = null;
            GetComponent<BoxCollider>().enabled = false;
        }
    }

    private void MoveDown()
    {
        if (!isDown) return;

        Vector3 dir = (destination - transform.position).normalized;
        transform.position += dir * Time.deltaTime * moveSpeed;

        Quaternion rotation = transform.rotation;
        rotation *= Quaternion.Euler(-20 * Time.deltaTime, 0, 0);
        transform.rotation = rotation;

        Vector3 scale = transform.localScale;
        if (!(scale.x <= 1 || scale.y <= 1 || scale.z <= 1))
        {
            scale -= Vector3.one * Time.deltaTime;
            transform.localScale = scale;
        }
        //else
        //    Destroy(gameObject);

        if (Vector3.Distance(transform.position, destination) <= 0.1f)
            Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "PlayerBullet")
        {
            other.gameObject.GetComponent<Bullet>().Disconnect();
        }

        if (other.transform.tag == "Drill")
        {
            if (other.GetComponent<Drill>().IsShot) Destroy(other.gameObject);
        }
    }
}
