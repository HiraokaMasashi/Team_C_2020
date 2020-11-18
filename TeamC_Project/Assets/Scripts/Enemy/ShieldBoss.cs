using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldBoss : Boss
{
    private bool isTurnRight;

    [SerializeField]
    private int shotBulletCount = 20;
    [SerializeField]
    private int shotBarrageRows = 4;

    private int shotCount;

    [SerializeField]
    private int summonCount = 5;

    // Start is called before the first frame update
    protected override void Start()
    {
        isTurnRight = true;
        shotCount = 0;

        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isFrameIn) return;

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

        if (shotElapsedTime >= shotInterval - 1.0f && !isPlayAlert)
        {
            isPlayAlert = true;
            SoundManager.Instance.PlaySeByName(alertSe);
        }
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
                    bulletController.GenerateBullet(transform.position, dir, destroyTime, "Enemy");
                    if (isTurnRight)
                        rad += 30;
                    else
                        rad -= 30;
                }
                yield return new WaitForSeconds(0.1f);
            }

            isPlayAlert = false;
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
            float x = -10 + (20 * i);
            Vector3 position = new Vector3(x, 22.0f, 0.0f);
            Instantiate(summonObject, position, Quaternion.Euler(new Vector3(40, 180, 0)));
        }
        summonElapsedTime = 0.0f;
        isSummon = true;
    }
}
