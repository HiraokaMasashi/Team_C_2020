using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drill : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 1.0f;
    [SerializeField]
    private float destroyZoneY;

    private bool isShoot;

    private ScrewCollision screwCollision;

    // Start is called before the first frame update
    void Start()
    {
        isShoot = false;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        DestroyDrill();
    }

    private void DestroyDrill()
    {
        if (transform.position.y >= destroyZoneY)
            Destroy(gameObject);
    }

    private void Move()
    {
        if (!isShoot) return;

        Vector3 position = transform.position;
        position += Vector3.up * moveSpeed * Time.deltaTime;
        transform.position = position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Screw")
        {
            isShoot = true;
        }

        if(other.transform.tag == "Enemy" && isShoot)
        {
            screwCollision = GameObject.FindGameObjectWithTag("Screw").GetComponent<ScrewCollision>();
            screwCollision.RemoveEnemy(other.gameObject);
            other.GetComponent<Health>().HitDeath();
        }
    }
}
