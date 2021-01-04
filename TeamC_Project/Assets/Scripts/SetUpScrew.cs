using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetUpScrew : MonoBehaviour
{
    private float recoveryTime;
    private float stanElapsedTime;

    protected float distanceY;
    protected float adjustPositionX;

    [SerializeField]
    protected float speed = 10.0f;

    [SerializeField]
    protected float maxDistance = 12.0f;

    protected Vector3 destination;
    protected float distance;
    [SerializeField]
    private float destroyZoneY = 22.0f;
    [SerializeField]
    private Vector3 clampPosition;

    private MissedEnemy missedEnemy;

    public Vector3 ClapmPosition
    {
        get { return clampPosition; }
    }

    private float alignmentPositionX;
    private float alignmentPositionY;


    protected enum MoveDirection
    {
        NONE,
        RIGHT,
        LEFT,
        UP,
    }
    protected MoveDirection currentDirection;

    public bool IsStan
    {
        get;
        protected set;
    } = false;

    public bool NotRecovery
    {
        get;
        set;
    } = false;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        stanElapsedTime = 0.0f;
        distanceY = 0.0f;
        adjustPositionX = 0.0f;
        currentDirection = MoveDirection.NONE;
        missedEnemy = GameObject.Find("MissedEnemy").GetComponent<MissedEnemy>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Time.timeScale == 0) return;

        if (currentDirection == MoveDirection.UP)
            MoveUp();
        else 
            Recovery();
    }

    private void MoveUp()
    {
        Vector3 position = transform.position;
        position += Vector3.up * (speed / 2.0f) * Time.deltaTime;
        transform.position = position;

        if (transform.position.y >= destroyZoneY)
        {
            if (transform.tag == "Bomb")
                GameObject.Find("BombBoss(Clone)").GetComponent<NormalBoss>().RemoveBomb(gameObject);
            else if (transform.tag == "Enemy")
                missedEnemy.MissCountUp();
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// スタン回復処理
    /// </summary>
    private void Recovery()
    {
        if (!IsStan) return;
        if (NotRecovery) return;

        stanElapsedTime += Time.deltaTime;
        if (stanElapsedTime >= recoveryTime)
        {
            currentDirection = MoveDirection.UP;
        }
    }

    /// <summary>
    /// スタン中の敵の並べ替え(移動)
    /// </summary>
    /// <param name="basePosition"></param>
    public virtual void StanMove(Vector3 basePosition)
    {
        if (Time.timeScale == 0) return;

        Vector3 position = transform.position;
        SetDestination(basePosition, position, ref distance, ref destination);
        CheckDistance(basePosition, position, ref distance, ref destination);

        //目的地に向かう
        //float currentLocation = (Time.deltaTime * speed) / distance;
        Vector3 dir = (destination - position).normalized;
        transform.position += dir * Time.deltaTime * speed;
    }

    /// <summary>
    /// 目的地の設定
    /// </summary>
    /// <param name="basePosition"></param>
    /// <param name="destination"></param>
    protected void SetDestination(Vector3 basePosition, Vector3 position, ref float distance, ref Vector3 destination)
    {
        destination = basePosition + new Vector3(adjustPositionX, distanceY, 0.0f);
        destination.x = Mathf.Clamp(destination.x, -clampPosition.x, clampPosition.x);
        if (destination.y >= clampPosition.y) destination.y = clampPosition.y;
        distance = Vector3.Distance(position, destination);
    }

    /// <summary>
    /// 目的地までの距離を測る
    /// </summary>
    /// <param name="basePosition"></param>
    /// <param name="position"></param>
    /// <param name="distance"></param>
    /// <param name="destination"></param>
    protected void CheckDistance(Vector3 basePosition, Vector3 position, ref float distance, ref Vector3 destination)
    {
        if (distance <= 0.1f)
        {
            if (currentDirection == MoveDirection.RIGHT)
                currentDirection = MoveDirection.LEFT;
            else if (currentDirection == MoveDirection.LEFT)
                currentDirection = MoveDirection.RIGHT;

            SetDestination(basePosition, position, ref distance, ref destination);
            SetAdjustPositionX();
        }
    }

    /// <summary>
    /// スクリューとの衝突時の処理
    /// </summary>
    /// <param name="distance"></param>
    /// <param name="basePositionX"></param>
    public virtual void HitScrew(float distance, float basePositionX)
    {
        IsStan = true;
        NotRecovery = true;
        distanceY = distance;
        if (transform.position.x <= basePositionX)
            currentDirection = MoveDirection.RIGHT;
        else
            currentDirection = MoveDirection.LEFT;
        SetAdjustPositionX();
    }

    /// <summary>
    /// 横移動の範囲の設定
    /// </summary>
    protected void SetAdjustPositionX()
    {
        if (distanceY <= maxDistance * (1.0f / 3.0f))
        {
            if (currentDirection == MoveDirection.LEFT)
                adjustPositionX = 1.0f;
            else if (currentDirection == MoveDirection.RIGHT)
                adjustPositionX = -1.0f;
        }
        else if (distanceY <= maxDistance * (2.0f / 3.0f))
        {
            if (currentDirection == MoveDirection.LEFT)
                adjustPositionX = 2.0f;
            else if (currentDirection == MoveDirection.RIGHT)
                adjustPositionX = -2.0f;
        }
        else
        {
            if (currentDirection == MoveDirection.LEFT)
                adjustPositionX = 3.0f;
            else if (currentDirection == MoveDirection.RIGHT)
                adjustPositionX = -3.0f;
        }
    }

    /// <summary>
    /// スクリューから外れたときに呼ぶ処理
    /// </summary>
    public void ReleaseScrew(float positionX, float positionY, float stanTime)
    {
        NotRecovery = false;
        recoveryTime = stanTime;
        alignmentPositionX = positionX;
        alignmentPositionY = positionY;

        StartCoroutine(Alignment());
    }

    private IEnumerator Alignment()
    {
        Vector3 destination = new Vector3(alignmentPositionX, alignmentPositionY, transform.position.z);
        Vector3 position = transform.position;
        while (Vector3.Distance(position, destination) > 0.1f)
        {
            Vector3 direction = destination - position;
            position += direction.normalized * speed * Time.deltaTime;
            transform.position = position;
            yield return null;
        }
    }
}
