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
    private ParticleManager particleManager;

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
        particleManager = GetComponent<ParticleManager>();
    }

    private void FixedUpdate()
    {
        if (GetIsDestroy())
        {
            Disconnect();
        }
    }

    public void Disconnect()
    {
        GameObject particle = transform.GetChild(0).gameObject;
        particleManager.StopParticle(particle);
        particle.transform.parent = null;
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
        if(transform.tag=="PlayerBullet" && other.transform.tag == "Screw")
        {
            IsPenetrate = true;
            GetComponent<Renderer>().material.color = Color.red;
        }

        if ((other.transform.tag == "Player" && transform.tag == "EnemyBullet")
            || (other.transform.tag == "Enemy" && transform.tag == "PlayerBullet"))
        {
            Health health = other.transform.GetComponent<Health>();
            health.Damage(Attack);

            if (other.transform.tag == "Enemy")
            {
                GameObject[] screws = GameObject.FindGameObjectsWithTag("Screw");
                for (int i = screws.Length - 1; i >= 0; i--)
                {
                    int length = screws[i].GetComponent<ScrewCollision>().GetEnemies().Count;
                    for (int j = length - 1; j >= 0; j--)
                    {
                        if (health.IsDead)
                            screws[i].GetComponent<ScrewCollision>().RemoveEnemy(j);
                    }
                }
            }

            if (health.IsDead && other.transform.tag == "Enemy")
            {
                Score score = other.transform.GetComponent<Score>();
                scoreManager.AddScore(score.GetScore());
            }
            if (!IsPenetrate) Disconnect();
        }

        if (other.transform.tag.Contains("Bullet") && !IsPenetrate)
        {
            Disconnect();
        }
    }
}
