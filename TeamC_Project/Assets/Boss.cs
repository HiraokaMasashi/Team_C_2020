﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{

    [SerializeField]
    private float minInterval = 10.0f;
    [SerializeField]
    private float maxInterval = 10.0f;

    private float num=2;

    protected BulletController bulletController;

    [SerializeField]
    protected float shotInterval;
    [SerializeField]
    protected float roopInterval;

    [SerializeField]
    protected float countIntervalTime;
    [SerializeField]
    protected float roopIntervalTime;
    protected GameObject player;
    [SerializeField]
    int countbullet=0;

    // Start is called before the first frame update
    void Start()
    {
        bulletController = GetComponent<BulletController>();
        shotInterval = Random.Range(minInterval, maxInterval);
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        ShotBulletBoss();
    }

    protected virtual void ShotBulletBoss()
    {
        if (player == null) return;
        

        countIntervalTime += Time.deltaTime;
        
        if (countIntervalTime >= shotInterval)
        {
            roopIntervalTime += Time.deltaTime;

            if (roopIntervalTime>=roopInterval)
            {
                Vector3 position = transform.position + Vector3.down;
                Vector3 direction = (player.transform.position - transform.position)/4;
                bulletController.GenerateBullet(ChargeBullet.ChargeMode.STAGE_1, position, direction, 300.0f, 3.0f, "Enemy");

               //Sleep();
                countbullet += 1;
                roopIntervalTime = 0;
            }

            if (countbullet >= 5)
            {
                countIntervalTime = 0;
                roopIntervalTime = 0;
                countbullet = 0;
            }
        }
    }

    IEnumerable Sleep()
    {
        yield return new WaitForSeconds(num); // num秒待機
    }


}
