using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrillBoss : Boss
{
    [SerializeField]
    private Transform[] instanceTransforms;

    private GameObject player;

    [SerializeField]
    private float shotDrillPower = 300.0f;
    [SerializeField]
    private float normalRespawnTime = 2.0f;
    [SerializeField]
    private float destroyRespawnTime = 5.0f;
    private float respawnTime;
    private float respawnElapsedTime;
    private bool nowRespawn;
    [SerializeField]
    private GameObject drillPrefab;
    private GameObject drill;

    // Start is called before the first frame update
    protected override void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        respawnElapsedTime = 0.0f;
        nowRespawn = false;

        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isFrameIn) return;

        ChangePattern();

        Shot();
        SummonEnemy();
        RespawnDrill();
    }

    private void ChangePattern()
    {
        switch (pattern)
        {
            case BehaviourPattern.SHOT:
                if (isShot)
                {
                    pattern = BehaviourPattern.SUMMON;
                    isShot = false;
                }
                break;

            case BehaviourPattern.SUMMON:
                if (isSummon)
                {
                    pattern = BehaviourPattern.SHOT;
                    isSummon = false;
                }
                break;

            default:
                break;
        }
    }

    private void Shot()
    {
        if (pattern != BehaviourPattern.SHOT) return;

        if (isShot) return;

        shotElapsedTime += Time.deltaTime;

        if (shotElapsedTime < shotInterval) return;

        isShot = true;
        for (int i = 0; i < instanceTransforms.Length; i++)
        {
            Vector3 position = instanceTransforms[i].position;
            Vector3 dir = player.transform.position - position;
            bulletController.GenerateBullet(position, dir, bulletSpeed, destryoTime, "Enemy");
        }
        shotElapsedTime = 0.0f;
    }

    private void SummonEnemy()
    {
        if (pattern != BehaviourPattern.SUMMON) return;

        summonElapsedTime += Time.deltaTime;

        if (summonElapsedTime < summonInterval) return;

        for (int i = 0; i < instanceTransforms.Length; i++)
        {
            Vector3 position = instanceTransforms[i].position;
            Instantiate(summonObject, position, Quaternion.identity);
        }
        summonElapsedTime = 0.0f;
        isSummon = true;
    }

    public void SetRespawn(bool isDestroy = false)
    {
        if (isDestroy) respawnTime = destroyRespawnTime;
        else respawnTime = normalRespawnTime;

        nowRespawn = true;
    }

    private void RespawnDrill()
    {
        if (drillPrefab == null) return;
        if (!nowRespawn) return;

        respawnElapsedTime += Time.deltaTime;
        if (respawnElapsedTime < respawnTime) return;

        respawnElapsedTime = 0.0f;
        nowRespawn = false;
        drill = Instantiate(drillPrefab, transform.position, Quaternion.identity, transform);
        StartCoroutine(InstanceDrill());
    }

    private IEnumerator InstanceDrill()
    {
        Vector3 destination = transform.position + new Vector3(0, -1.2f, 0);
        while (Vector3.Distance(drill.transform.position, destination) > 0.1f)
        {
            Vector3 position = Vector3.Lerp(drill.transform.position, destination, Time.deltaTime * moveSpeed);
            drill.transform.position = position;
            yield return null;
        }

        yield break;
    }
}
