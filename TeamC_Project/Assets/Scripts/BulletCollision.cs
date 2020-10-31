using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCollision : MonoBehaviour
{
    [SerializeField]
    private Vector3 destroyZone;
    [SerializeField]
    private float destroyZoneMinY = -11.0f;

    private ScoreManager scoreManager;

    public int Attack
    {
        get;
        set;
    } = 1;

    public bool IsPenetrate
    {
        get;
        set;
    } = false;

    private void Start()
    {
        scoreManager = ScoreManager.Instance;
    }

    private void FixedUpdate()
    {
        if (GetIsDestroy())
            Destroy(gameObject);
    }

    private bool GetIsDestroy()
    {
        bool isDestroy = false;

        if (transform.position.x <= -destroyZone.x || transform.position.x >= destroyZone.x
            || transform.position.y >= destroyZone.y || transform.position.y <= destroyZoneMinY)
            isDestroy = true;

        return isDestroy;
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((other.transform.tag == "Player" && transform.tag == "EnemyBullet")
            || (other.transform.tag == "Enemy" && transform.tag == "PlayerBullet"))
        {
            Health health = other.transform.GetComponent<Health>();
            Score score = other.transform.GetComponent<Score>();

            health.Damage(Attack);
            if (health.IsDead) scoreManager.AddScore(score.GetScore());
            if (!IsPenetrate) Destroy(gameObject);
        }

        if (other.transform.tag == "EnemyBullet" && transform.tag == "PlayerBullet")
        {
            if (!IsPenetrate) Destroy(gameObject);
            Destroy(other.gameObject);
        }
    }
}
