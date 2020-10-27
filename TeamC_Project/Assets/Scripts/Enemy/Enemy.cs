using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;

    [SerializeField]
    private Vector3 destroyZone;

    enum MoveMode
    {
        NORMAL,
        INTERSEPTION,
    }
    private MoveMode currentMode;

    // Start is called before the first frame update
    void Start()
    {
        currentMode = MoveMode.NORMAL;
    }

    // Update is called once per frame
    void Update()
    {
        //// 自分自身のtransformを取得
        //Transform myTransform = this.transform;

        ////Translate関数を使用
        ////Translate(float x,float y,float z,Space relativeTo = Space.Self)
        //myTransform.Translate(0, moveSpeed, 0, Space.World);

        switch (currentMode)
        {
            case MoveMode.NORMAL:
                NormalMove();
                break;

            case MoveMode.INTERSEPTION:
                break;

            default:
                break;
        }

        if (transform.position.y <= destroyZone.y)
        {
            Destroy(gameObject);
        }
    }

    private void NormalMove()
    {
        Vector3 position = transform.position;
        position += Vector3.down * moveSpeed * Time.deltaTime;
        transform.position = position;
    }

    private void InterseptionMove()
    {

    }
}
