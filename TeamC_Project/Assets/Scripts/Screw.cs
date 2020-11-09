using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screw : MonoBehaviour
{
    private BoxCollider boxCollider;

    private float maxCenterY = 6.0f;//コライダーのY軸の最大センター
    private float maxSizeY = 12.0f;//コライダーのY軸の最大サイズ

    public enum ScrewType
    {
        SHOT,
        INHALE,
        NONE,
    }
    private ScrewType screwType;

    [SerializeField]
    private float moveSpeed = 1.0f;

    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        Move();
        ChangeBoxSize();
        EnemyStanMove();
    }

    private void Move()
    {
        if (screwType != ScrewType.SHOT) return;

        Vector3 position = transform.position;
        position += Vector3.up * (moveSpeed / 2.0f) * Time.deltaTime;
        transform.position = position;
    }


    /// <summary>
    /// スクリュー使用時にあたり判定のサイズを調整する
    /// </summary>
    private void ChangeBoxSize()
    {
        if (screwType != ScrewType.INHALE) return;
        if (boxCollider == null) return;

        //あたり判定の調整サイズ
        float addSize = 0.0f;
        addSize += Time.deltaTime;

        float speed = 2.0f;
        Vector3 center = boxCollider.center;
        center.y += addSize * speed;
        //最大値を超えていたら範囲内に収める
        if (center.y >= maxCenterY) center.y = maxCenterY;
        boxCollider.center = center;

        Vector3 size = boxCollider.size;
        size.y += addSize * 2.0f * speed;
        //最大値を超えていたら範囲内に収める
        if (size.y >= maxSizeY) size.y = maxSizeY;
        boxCollider.size = size;
    }

    private void EnemyStanMove()
    {
        if (player == null) return;

        Vector3 basePosition;
        if (screwType == ScrewType.INHALE)
            basePosition = player.transform.position;
        else
            basePosition = transform.position;

        List<GameObject> enemies = GetComponent<ScrewCollision>().GetEnemies();
        if (enemies == null) return;
        for (int i = enemies.Count - 1; i >= 0; i--)
        {
            enemies[i].GetComponent<SetUpScrew>().StanMove(basePosition);
        }
    }

    public void SetScrewType(ScrewType type)
    {
        screwType = type;
    }

    public ScrewType GetScrewType()
    {
        return screwType;
    }
}
