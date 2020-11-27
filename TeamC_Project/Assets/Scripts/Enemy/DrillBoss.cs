using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrillBoss : Boss
{
    [SerializeField]
    private Transform[] instanceTransforms;

    private GameObject player;

    //[SerializeField]
    //private float shotDrillPower = 300.0f;
    //[SerializeField]
    //private float normalRespawnTime = 2.0f;
    //ドリルのリスポーン時間
    [SerializeField]
    private float destroyRespawnTime = 5.0f;
    private float respawnTime;
    //リスポーンの経過時間
    private float respawnElapsedTime;
    //リスポーン中か
    private bool nowRespawn;
    //ドリルのプレハブ
    [SerializeField]
    private GameObject drillPrefab;
    //ドリル
    private GameObject drill;

    //連射数
    [SerializeField]
    private int rapidShot = 3;
    //発射数
    [SerializeField]
    private int shotCount = 3;
    //プレイヤー方向に撃つか
    private bool isForPlayer;
    private int endShotCount;
    private int endBehaviourPattern;

    private bool endAttack;
    private bool endMove;
    [SerializeField]
    private float attackSpeed = 1.0f;
    private Vector3 startPosition;
    [SerializeField]
    private float attackInterval = 3.0f;
    private float attackElapsedTime;
    private Vector3 attackPosition;

    [SerializeField]
    private int chnageBehaviourCount = 2;
    [SerializeField]
    private Transform instanceTransform;

    // Start is called before the first frame update
    protected override void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        respawnElapsedTime = 0.0f;
        nowRespawn = false;
        isForPlayer = false;
        endShotCount = 0;
        endBehaviourPattern = 0;
        endAttack = false;
        endMove = false;
        attackElapsedTime = 0.0f;
        attackPosition = Vector3.zero;
        drill = transform.GetChild(2).gameObject;

        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isFrameIn || health.IsDead) return;

        ChangePattern();

        Shot();
        SummonEnemy();
        RespawnDrill();
        DrillAttackStart();
        DrillAttackEnd();
    }

    /// <summary>
    /// 行動パターン切り替え処理
    /// </summary>
    private void ChangePattern()
    {
        switch (pattern)
        {
            case BehaviourPattern.SHOT:
                if (endShotCount >= 2)
                {
                    pattern = BehaviourPattern.SUMMON;
                    isShot = false;
                    isForPlayer = false;
                    endShotCount = 0;
                }
                break;

            case BehaviourPattern.SUMMON:
                if (isSummon)
                {
                    SummonNextPattern();
                }
                break;

            case BehaviourPattern.DRILL_ATTACK:
                if (endMove)
                {
                    pattern = BehaviourPattern.SHOT;
                    endAttack = false;
                    endMove = false;
                    attackElapsedTime = 0.0f;
                    attackPosition = Vector3.zero;
                    isPlayAlert = false;
                }
                break;

            default:
                break;
        }
    }

    /// <summary>
    /// ドリル攻撃までの行動選択処理
    /// </summary>
    private void SummonNextPattern()
    {
        //ドリル攻撃をするまでに一連の行動を行った回数が2回未満なら
        if (endBehaviourPattern < chnageBehaviourCount)
        {
            //ランダムで次の行動を選ぶ
            int random = Random.Range(0, 3);
            if (random != 0)
            {
                pattern = BehaviourPattern.SHOT;
                endBehaviourPattern++;
            }
            else
            {
                pattern = BehaviourPattern.DRILL_ATTACK;
                endBehaviourPattern = 0;
                startPosition = transform.position;
            }
        }
        else
        {
            pattern = BehaviourPattern.DRILL_ATTACK;
            endBehaviourPattern = 0;
            startPosition = transform.position;
        }
        isSummon = false;
    }

    /// <summary>
    /// 弾発射開始処理
    /// </summary>
    private void Shot()
    {
        if (pattern != BehaviourPattern.SHOT) return;
        if (isShot) return;

        shotElapsedTime += Time.deltaTime;

        if (shotElapsedTime < shotInterval) return;

        isShot = true;
        if (endShotCount >= 1) isForPlayer = true;
        StartCoroutine(RapidShot());
    }

    /// <summary>
    /// 雑魚敵生成処理
    /// </summary>
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

    /// <summary>
    /// ドリル攻撃開始処理
    /// </summary>
    private void DrillAttackStart()
    {
        if (pattern != BehaviourPattern.DRILL_ATTACK) return;
        if (endAttack) return;
        if (player == null) return;

        //ドリルがなかった場合
        if (drill == null)
        {
            endAttack = true;
            return;
        }

        attackElapsedTime += Time.deltaTime;
        if (attackElapsedTime >= attackInterval - 1.0f && !isPlayAlert)
        {
            isPlayAlert = true;
            SoundManager.Instance.PlaySeByName(alertSe);
        }
        if (attackElapsedTime < attackInterval) return;

        if (attackPosition == Vector3.zero)
            attackPosition = player.transform.position + Vector3.up * 3;

        Vector3 position = (attackPosition - transform.position).normalized;
        transform.position += position * Time.deltaTime * attackSpeed;
        if (Vector3.Distance(transform.position, attackPosition) <= 0.1f)
        {
            transform.position = attackPosition;
            endAttack = true;
        }
    }

    private void DrillAttackEnd()
    {
        if (pattern != BehaviourPattern.DRILL_ATTACK) return;

        if (!endAttack) return;

        Vector3 position = (startPosition - transform.position).normalized;
        transform.position += position * Time.deltaTime * attackSpeed;
        if (Vector3.Distance(transform.position, startPosition) <= 0.1f)
        {
            transform.position = startPosition;
            endMove = true;
        }
    }

    /// <summary>
    /// リスポーン設定
    /// </summary>
    public void SetRespawn()
    {
        respawnTime = destroyRespawnTime;
        //else respawnTime = normalRespawnTime;

        nowRespawn = true;
    }

    /// <summary>
    /// ドリルリスポーン処理
    /// </summary>
    private void RespawnDrill()
    {
        if (drillPrefab == null) return;
        if (!nowRespawn) return;
        if (pattern == BehaviourPattern.DRILL_ATTACK) return;

        respawnElapsedTime += Time.deltaTime;
        if (respawnElapsedTime < respawnTime) return;

        respawnElapsedTime = 0.0f;
        nowRespawn = false;
        Vector3 position = instanceTransform.position - new Vector3(1.25f, 0.25f, 0);
        drill = Instantiate(drillPrefab, position, Quaternion.Euler(-180, -180, 0), transform);
        StartCoroutine(InstanceDrill());
    }

    /// <summary>
    /// ドリル生成処理
    /// </summary>
    /// <returns></returns>
    private IEnumerator InstanceDrill()
    {
        Vector3 destination = instanceTransform.position - new Vector3(1.25f, 2.0f, 0);
        while (Vector3.Distance(drill.transform.position, destination) > 0.1f)
        {
            Vector3 position = Vector3.Lerp(drill.transform.position, destination, Time.deltaTime * moveSpeed);
            drill.transform.position = position;
            yield return null;
        }

        yield break;
    }

    /// <summary>
    /// 連射処理
    /// </summary>
    /// <returns></returns>
    private IEnumerator RapidShot()
    {
        while (isShot)
        {
            for (int i = 0; i < shotCount; i++)
            {
                Vector3 dir;
                int rad = 90;
                for (int j = 0; j < rapidShot; j++)
                {
                    for (int k = 0; k < instanceTransforms.Length; k++)
                    {
                        if (player == null)
                        {
                            isShot = false;
                            yield break;
                        }

                        Vector3 position = instanceTransforms[k].position;
                        if (isForPlayer) dir = player.transform.position - position;
                        else
                        {
                            rad += 180 / shotCount;
                            dir = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0);
                            rad += 15;
                        }

                        bulletController.GenerateBullet(position, dir, destroyTime, "Enemy");
                    }
                }
                yield return new WaitForSeconds(0.2f);
            }

            isShot = false;
            endShotCount++;
            shotElapsedTime = 0.0f;
            yield break;
        }
    }

    public override void DestroyOtherObject()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        List<GameObject> e = new List<GameObject>();
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i] == gameObject) continue;

            e.Add(enemies[i]);
        }

        for (int i = e.Count - 1; i >= 0; i--)
        {
            Destroy(e[i]);
        }
    }
}
