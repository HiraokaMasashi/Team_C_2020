using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screw : MonoBehaviour
{
    /// <summary>
    /// スクリューのタイプ
    /// </summary>
    public enum ScrewType
    {
        SHOT,
        INHALE,
        NONE,
    }
    private ScrewType screwType;

    [SerializeField]
    private float moveSpeed = 1.0f;//移動速度
    private GameObject player;//プレイヤー

    [SerializeField]
    private float maxPositionY;//最大位置

    [SerializeField]
    private float stanTime = 2.0f;
    public float StanTime
    {
        get { return stanTime; }
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (Time.timeScale == 0) return;

        Move();
        EnemyStanMove();
    }

    /// <summary>
    /// 移動処理
    /// </summary>
    private void Move()
    {
        //スクリューのタイプが射出型でなければreturn
        if (screwType != ScrewType.SHOT) return;

        Vector3 position = transform.position;
        position += Vector3.up * moveSpeed * Time.deltaTime;
        transform.position = position;
    }

    /// <summary>
    /// スクリューに巻き込んだエネミーの移動処理
    /// </summary>
    private void EnemyStanMove()
    {
        if (player == null) return;

        //ヒットしているスクリューのタイプに応じて、基準位置を変える
        Vector3 basePosition;
        if (screwType == ScrewType.INHALE)
            basePosition = player.transform.position;
        else
            basePosition = transform.position;

        List<GameObject> enemies = GetComponent<ScrewCollision>().GetObjects();
        if (enemies == null) return;
        for (int i = enemies.Count - 1; i >= 0; i--)
        {
            enemies[i].GetComponent<SetUpScrew>().StanMove(basePosition);
        }
    }

    /// <summary>
    /// スクリューのタイプを設定する
    /// </summary>
    /// <param name="type"></param>
    public void SetScrewType(ScrewType type)
    {
        screwType = type;
    }

    /// <summary>
    /// スクリューのタイプを返す
    /// </summary>
    /// <returns></returns>
    public ScrewType GetScrewType()
    {
        return screwType;
    }
}
