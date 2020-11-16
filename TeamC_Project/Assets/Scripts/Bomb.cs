﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    private GameObject player;
    private ParticleManager particleManager;
    private NormalBoss boss;
    private SetUpScrew setUpScrew;

    private Vector3 direction;
    [SerializeField]
    private float moveSpeed = 0.5f;

    [SerializeField]
    private Vector3 destroyZone = new Vector3(16, -11, 0);

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        particleManager = GetComponent<ParticleManager>();
        boss = GameObject.Find("BombBoss").GetComponent<NormalBoss>();
        setUpScrew = GetComponent<SetUpScrew>();

        direction = player.transform.position - transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (setUpScrew.IsStan) return;

        Move();

        if (GetIsDestroy())
            DisConnect();
    }

    private void Move()
    {
        Vector3 position = transform.position;
        position += direction.normalized * Time.deltaTime * moveSpeed;
        transform.position = position;
    }
    private bool GetIsDestroy()
    {
        bool isDestroy = false;

        if (transform.position.x <= -destroyZone.x || transform.position.x >= destroyZone.x
            || transform.position.y <= destroyZone.y)
            isDestroy = true;

        return isDestroy;
    }

    private void DisConnect()
    {
        if (transform.childCount == 0) return;

        GameObject particle = null;
        foreach (Transform child in transform)
        {
            if (child.name.Contains("Trail"))
            {
                particle = child.gameObject;
                break;
            }
        }
        if (particle == null) return;

        particleManager.StopParticle(particle);
        particle.transform.parent = null;
        boss.RemoveBomb(gameObject);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "PlayerBullet")
        {
            other.gameObject.GetComponent<BulletCollision>().Disconnect();
            GameObject particle = particleManager.GenerateParticle(2);
            particle.transform.position = transform.position;
            particle.transform.rotation = Quaternion.Euler(-90, 0, 0);
            particleManager.OncePlayParticle(particle);
            DisConnect();
        }

        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Blast")
        {
            GameObject particle = particleManager.GenerateParticle(2);
            particle.transform.position = transform.position;
            particle.transform.rotation = Quaternion.Euler(-90, 0, 0);
            particleManager.OncePlayParticle(particle);
            DisConnect();
        }
    }
}
