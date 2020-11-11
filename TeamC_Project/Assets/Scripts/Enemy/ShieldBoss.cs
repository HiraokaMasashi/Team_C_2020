using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldBoss : MonoBehaviour
{
    enum BehaviourPattern
    {
        SHOT,
        SUMMON,
    }
    private BehaviourPattern pattern;
    private bool isTurnRight;

    private BulletController bulletController;

    private GameManager gameManager;

    [SerializeField]
    private float bulletSpeed = 500.0f;
    [SerializeField]
    private int shotBulletCount = 20;
    [SerializeField]
    private float shotInterval = 5.0f;
    private float shotElapsedTime;
    [SerializeField]
    private int shotBarrageRows = 4;

    [SerializeField]
    private float destryoTime = 5.0f;
    private bool isShot;

    private int shotCount;

    [SerializeField]
    private GameObject summonObject;
    private bool isSummon;
    [SerializeField]
    private int summonCount = 5;
    [SerializeField]
    private float summonInterval = 5.0f;
    private float summonElapsedTime;

    // Start is called before the first frame update
    void Start()
    {
        bulletController = GetComponent<BulletController>();
        shotElapsedTime = 0.0f;
        summonElapsedTime = 0.0f;
        isShot = false;

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        gameManager.SetBossEnemy(gameObject);

        pattern = BehaviourPattern.SHOT;
        isTurnRight = true;
        shotCount = 0;
        isSummon = false;
    }

    // Update is called once per frame
    void Update()
    {
        ChangePattern();

        Shot();
        SummonEnemy();
    }

    private void ChangePattern()
    {
        switch (pattern)
        {
            case BehaviourPattern.SHOT:
                if (shotCount >= 2)
                {
                    pattern = BehaviourPattern.SUMMON;
                    shotCount = 0;
                    isTurnRight = !isTurnRight;
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
        StartCoroutine(ShotBarrage());
        shotElapsedTime = 0.0f;
    }

    private IEnumerator ShotBarrage()
    {
        int rad = 0;
        while (isShot)
        {
            for (int j = 0; j < shotBulletCount; j++)
            {
                for (int i = 0; i < shotBarrageRows; i++)
                {
                    rad += 360 / shotBarrageRows;
                    Vector3 dir = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0.0f);
                    bulletController.GenerateBullet(transform.position, dir, bulletSpeed, destryoTime, "Enemy");
                    if (isTurnRight)
                        rad += 5;
                    else
                        rad -= 5;
                }
                yield return new WaitForSeconds(0.1f);
            }

            isShot = false;
            shotCount++;
            yield break;
        }
    }

    private void SummonEnemy()
    {
        if (pattern != BehaviourPattern.SUMMON) return;

        summonElapsedTime += Time.deltaTime;

        if (summonElapsedTime < summonInterval) return;

        for (int i = 0; i < summonCount; i++)
        {
            float x = -20 + (10 * i);
            Vector3 position = new Vector3(x, 22.0f, 0.0f);

            Instantiate(summonObject, position, Quaternion.identity);
        }
        summonElapsedTime = 0.0f;
        isSummon = true;
    }
}
