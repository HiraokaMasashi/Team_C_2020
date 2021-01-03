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

    private Animator animator;
    [SerializeField]
    private float animatorInterval = 1.0f;
    private float animatorElapsedTime;
    [SerializeField]
    private float enterTime = 2.0f;
    private float enterElapsedTime;

    private GameObject shieldObject;

    // Start is called before the first frame update
    protected override void Start()
    {
        isTurnRight = true;
        shotCount = 0;

        animator = GetComponent<Animator>();
        shieldObject = transform.GetChild(0).gameObject;
        animatorElapsedTime = 0.0f;
        enterElapsedTime = 0.0f;
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isFrameIn) return;

        if (gameManager.IsPerformance)
        {
            if (health.IsDead)
            {
                animator.speed = 0;
                DestroyOtherObject();
                return;
            }

            enterElapsedTime += Time.deltaTime;
            if (enterElapsedTime < enterTime)
                return;

            animator.enabled = false;
            gameManager.IsPerformance = false;
        }

        if (health.IsDead || gameManager.IsEnd)
        {
            StopAllCoroutines();
            return;
        }

        StartAnimation();
        Animation();
        ChangePattern();

        ShotOnScript();
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

    public void Shot()
    {
        //if (pattern != BehaviourPattern.SHOT) return;

        //if (isShot) return;

        //shotElapsedTime += Time.deltaTime;

        //if (shotElapsedTime >= shotInterval - 1.0f && !isPlayAlert)
        //{
        //    isPlayAlert = true;
        //    SoundManager.Instance.PlaySeByName(alertSe);
        //}
        //if (shotElapsedTime < shotInterval) return;

        isShot = true;
        StartCoroutine(ShotBarrage());
        //shotElapsedTime = 0.0f;
    }

    private void ShotOnScript()
    {
        if (shieldObject == null) return;
        if (pattern != BehaviourPattern.SHOT) return;
        if (isShot) return;

        shotElapsedTime += Time.deltaTime;
        if (shotElapsedTime >= shotInterval - 1.0f && !isPlayAlert)
        {
            isPlayAlert = true;
            SoundManager.Instance.PlaySeByName(alertSe);
        }
        if (shotElapsedTime < shotInterval) return;

        Shot();
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
                SoundManager.Instance.PlaySeByName(shotSe);
                yield return new WaitForSeconds(0.1f);
            }

            //isPlayAlert = false;
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

    private void StartAnimation()
    {
        if (shieldObject == null && !animator.enabled)
            animator.enabled = true;
    }

    private void Animation()
    {
        if (shieldObject != null) return;
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("BossMove")) return;

        animatorElapsedTime += Time.deltaTime;
        if (animatorElapsedTime < animatorInterval) return;

        animatorElapsedTime = 0.0f;
        animator.SetTrigger("IsMove");
    }
}
