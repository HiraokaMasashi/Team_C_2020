using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    protected enum BehaviourPattern
    {
        SHOT,
        SUMMON,
        SHOT_DRILL,
    }
    protected BehaviourPattern pattern;

    protected BulletController bulletController;
    private GameManager gameManager;

    [SerializeField]
    private Vector3 destination = new Vector3(0, 20, 0);
    [SerializeField]
    protected float moveSpeed = 2.0f;
    protected bool isFrameIn = false;

    [SerializeField]
    protected float bulletSpeed = 500.0f;
    [SerializeField]
    protected float shotInterval = 5.0f;
    protected float shotElapsedTime;
    [SerializeField]
    protected float destryoTime = 5.0f;
    protected bool isShot;

    [SerializeField]
    protected GameObject summonObject;
    protected bool isSummon;
    [SerializeField]
    protected float summonInterval = 5.0f;
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
