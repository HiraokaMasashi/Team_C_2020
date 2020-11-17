﻿using System.Collections;
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

    [SerializeField]
    private float hitInterval = 1.0f;
    private float hitElpsedTime;

    private ScoreManager scoreManager;

    // Start is called before the first frame update
    void Start()
    {
        isShot = false;
        player = GameObject.FindGameObjectWithTag("Player");
        health = GetComponent<Health>();
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
            if (!isShot)
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
        }
    }
}
