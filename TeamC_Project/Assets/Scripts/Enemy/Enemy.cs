using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    protected float moveSpeed = 1.0f;
    [SerializeField]
    protected Vector3 destroyZone = new Vector3(0.0f, -11.0f, 0.0f);

    protected SetUpScrew setupScrew;

    protected BulletController bulletController;
    protected float shotInterval;
    [SerializeField]
    private float minInterval = 1.0f;
    [SerializeField]
    private float maxInterval = 5.0f;
    protected float elapsedTime;
    protected GameObject player;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        setupScrew = GetComponent<SetUpScrew>();

        bulletController = GetComponent<BulletController>();
        shotInterval = Random.Range(minInterval, maxInterval);
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (setupScrew.IsStan) return;

        Move();
        ShotBullet();
        Death();
    }

    protected virtual void Move()
    {
        Vector3 position = transform.position;
        position += Vector3.down * moveSpeed * Time.deltaTime;
        transform.position = position;
    }

    protected virtual void ShotBullet()
    {
        if (player == null) return;

        elapsedTime += Time.deltaTime;

        if (elapsedTime >= shotInterval)
        {
            Vector3 position = transform.position + Vector3.down;
            Vector3 direction = player.transform.position - transform.position;
            bulletController.GenerateBullet(position, direction, 3.0f, "Enemy");
            elapsedTime = 0;
        }
    }

    protected void Death()
    {
        if (transform.position.y <= destroyZone.y)
        {
            Destroy(gameObject);
        }
    }
}
