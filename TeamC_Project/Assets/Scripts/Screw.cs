using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screw : MonoBehaviour
{
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

    [SerializeField]
    private float maxPositionY;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (Time.timeScale == 0) return;

        Move();
        //ChangeBoxSize();
        EnemyStanMove();
    }

    private void Move()
    {
        if (screwType != ScrewType.SHOT) return;

        Vector3 position = transform.position;
        position += Vector3.up * (moveSpeed / 2.0f) * Time.deltaTime;
        transform.position = position;
    }

    private void EnemyStanMove()
    {
        if (player == null) return;

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

    public void SetScrewType(ScrewType type)
    {
        screwType = type;
    }

    public ScrewType GetScrewType()
    {
        return screwType;
    }
}
