using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    //ボスに必要なパターンの列挙
    protected enum BehaviourPattern
    {
        SHOT,
        SUMMON,
        DRILL_ATTACK,
        SHOT_BOMB,
    }
    //現在のパターン
    protected BehaviourPattern pattern;

    protected BulletController bulletController;
    private GameManager gameManager;

    //フレームイン時の目的地
    [SerializeField]
    private Vector3 destination = new Vector3(0, 20, 0);
    //フレームインのスピード
    [SerializeField]
    protected float moveSpeed = 2.0f;
    //フレームインしたか
    protected bool isFrameIn = false;

    //弾の速度
    [SerializeField]
    protected float bulletSpeed = 500.0f;
    //弾の発射間隔
    [SerializeField]
    protected float shotInterval = 5.0f;
    //弾を撃つまでの経過時間
    protected float shotElapsedTime;
    //弾の破棄する時間
    [SerializeField]
    protected float destroyTime = 5.0f;
    //弾を撃ったか
    protected bool isShot;

    //雑魚敵生成オブジェクト
    [SerializeField]
    protected GameObject summonObject;
    //生成したか
    protected bool isSummon;
    //生成までの間隔
    [SerializeField]
    protected float summonInterval = 5.0f;
    //生成までの経過時間
    protected float summonElapsedTime;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        bulletController = GetComponent<BulletController>();
        shotElapsedTime = 0.0f;
        summonElapsedTime = 0.0f;
        isShot = false;
        isSummon = false;

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        gameManager.SetBossEnemy(gameObject);

        pattern = BehaviourPattern.SHOT;

        StartCoroutine(FrameIn());
    }

    private IEnumerator FrameIn()
    {
        while (Vector3.Distance(transform.position, destination) > 0.1f)
        {
            Vector3 position = Vector3.Lerp(transform.position, destination, Time.deltaTime * moveSpeed);
            transform.position = position;
            yield return null;
        }

        isFrameIn = true;
        yield break;
    }
}
