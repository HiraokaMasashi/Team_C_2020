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

    private bool isRotation;

    private ParticleManager particleManager;
    private GameObject rotateEffect;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        health = GetComponent<Health>();
        hitInterval = equipmentHitInterval;
        hitElpsedTime = 0.0f;
        isRotation = false;
        particleManager = GetComponent<ParticleManager>();
        rotateEffect = null;
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
            {
                transform.position = player.transform.position - player.transform.up * 2f;
                if (!isRotation)
                {
                    transform.rotation = Quaternion.Euler(0, 0, 180);
                    isRotation = true;
                }
                transform.rotation *= Quaternion.Euler(0, 360 * Time.deltaTime, 0);
                if (rotateEffect == null)
                {
                    rotateEffect = particleManager.GenerateParticleInChildren(1);
                    rotateEffect.transform.position = transform.position;
                    rotateEffect.transform.rotation = transform.rotation;
                    particleManager.StartParticle(rotateEffect);
                }
            }
        }
        else
        {
            Vector3 position = transform.position;
            position += Vector3.up * moveSpeed * Time.deltaTime;
            transform.position = position;
            transform.rotation *= Quaternion.Euler(0, 360 * Time.deltaTime, 0);
            if (rotateEffect == null)
            {
                rotateEffect = particleManager.GenerateParticleInChildren(1);
                rotateEffect.transform.position = transform.position;
                rotateEffect.transform.rotation = transform.rotation;
                particleManager.StartParticle(rotateEffect);
            }
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

    public void DisConnect()
    {
        if (rotateEffect.transform.parent == null) return;

        rotateEffect.transform.parent = null;
        particleManager.StopParticle(rotateEffect);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Enemy")
        {
            Health otherHealth = other.GetComponent<Health>();

            if (!other.gameObject.name.Contains("Boss"))
                otherHealth.HitDeath();
            else
            {
                //シールドを持っていたらヒット判定を行わない
                if (other.transform.childCount >= 1)
                {
                    if (other.transform.GetChild(0).gameObject.name.Contains("Shield"))
                        return;
                }

                otherHealth.Damage(10);
            }

            if (!IsShot)
                health.Damage(1);

            if (health.IsDead)
                DisConnect();

            GameObject particle = particleManager.GenerateParticle();
            particle.transform.position = other.ClosestPointOnBounds(transform.position + Vector3.left * 1.2f);
            particle.transform.rotation = Quaternion.Euler(-90, 0, 0);
            particleManager.OncePlayParticle(particle);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name.Contains("Boss"))
        {
            //シールドを持っていたらヒット判定を行わない
            if (other.transform.childCount >= 1)
            {
                if (other.transform.GetChild(0).gameObject.name.Contains("Shield"))
                    return;
            }

            hitElpsedTime += Time.deltaTime;
            if (hitElpsedTime < hitInterval) return;

            other.GetComponent<Health>().Damage(10);
            hitElpsedTime = 0.0f;
            health.Damage(1);

            GameObject particle = particleManager.GenerateParticle();
            particle.transform.position = other.ClosestPointOnBounds(transform.position + Vector3.left * 1.2f);
            particle.transform.rotation = Quaternion.Euler(-90, 0, 0);
            particleManager.OncePlayParticle(particle);
        }
    }
}
