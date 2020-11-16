using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class Drill : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 1.0f;
    [SerializeField]
    private float destroyZoneY;

    private bool isShot;
    private GameObject player;
    private Health health;

    // Start is called before the first frame update
    void Start()
    {
        isShot = false;
        player = GameObject.FindGameObjectWithTag("Player");
        health = GetComponent<Health>();
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
        if (!isShot)
        {
            if (transform.parent == null)
                transform.position = player.transform.position + Vector3.up;
            return;
        }

        Vector3 position = transform.position;
        position += Vector3.up * moveSpeed * Time.deltaTime;
        transform.position = position;
    }

    public void Shot()
    {
        isShot = true;
        GetComponent<BoxCollider>().enabled = true;
        transform.rotation = Quaternion.Euler(Vector3.zero);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Enemy")
        {
            if (!other.transform.name.Contains("Boss"))
                other.GetComponent<Health>().HitDeath();

            if (!isShot)
                health.Damage(1);
        }
    }
}
