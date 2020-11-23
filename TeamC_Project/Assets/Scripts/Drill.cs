using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class Drill : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 1.0f;
    [SerializeField]
    private float maxDestroyZoneY;
    [SerializeField]
    private float minDestroyZoneY;

    public bool IsShot
    {
        get;
        private set;
    } = false;

    public bool IsThrowAway
    {
        get;
        set;
    } = false;

    private GameObject player;
    private Health health;

    [SerializeField]
    private float equipmentHitInterval = 0.3f;
    [SerializeField]
    private float shotHitInterval = 0.1f;
    private float hitInterval;
    private float hitElpsedTime;

    private ScoreManager scoreManager;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        health = GetComponent<Health>();
        hitInterval = equipmentHitInterval;
        hitElpsedTime = 0.0f;

        scoreManager = ScoreManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        DestroyDrill();
    }

    private void DestroyDrill()
    {
        if (transform.position.y >= maxDestroyZoneY || transform.position.y <= minDestroyZoneY)
            Destroy(gameObject);
    }

    private void Move()
    {
        if (IsThrowAway)
        {
            Vector3 position = transform.position;
            position += Vector3.down * moveSpeed * Time.deltaTime;
            transform.position = position;
            return;
        }

        if (!IsShot)
        {
            if (transform.parent == null && player != null)
                transform.position = player.transform.position - player.transform.up * 1.5f;
        }
        else
        {
            Vector3 position = transform.position;
            position += Vector3.up * moveSpeed * Time.deltaTime;
            transform.position = position;
        }
    }

    public void Shot()
    {
        IsShot = true;
        transform.position = player.transform.position + Vector3.up * 1.5f;
        GetComponent<BoxCollider>().enabled = true;
        transform.rotation = Quaternion.Euler(new Vector3(180, 0, 0));
        hitInterval = shotHitInterval;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Enemy")
        {
            Health otherHealth = other.GetComponent<Health>();

            if (!other.gameObject.name.Contains("Boss"))
            {
                otherHealth.HitDeath();
                Score score = other.transform.GetComponent<Score>();
                scoreManager.AddScore(score.GetScore());
            }
            else
            {
                otherHealth.Damage(10);
                if (otherHealth.IsDead)
                {
                    Score score = other.transform.GetComponent<Score>();
                    scoreManager.AddScore(score.GetScore());
                }
            }

            if (!IsShot)
                health.Damage(1);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name.Contains("Boss"))
        {
            hitElpsedTime += Time.deltaTime;
            if (hitElpsedTime < hitInterval) return;

            other.GetComponent<Health>().Damage(10);
            hitElpsedTime = 0.0f;
            health.Damage(1);
        }
    }
}
