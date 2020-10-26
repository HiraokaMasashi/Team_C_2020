using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //HPスクリプトから参照するため
    public int hp;
    //
    public float moveSpeed;
    
    private Rigidbody2D rb = null;

    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        // 自分自身のtransformを取得
        Transform myTransform = this.transform;

        //Translate関数を使用
        //Translate(float x,float y,float z,Space relativeTo = Space.Self)
        myTransform.Translate(0, moveSpeed, 0, Space.World);

        if (!GetComponent<Renderer>().isVisible)
        {
            Destroy(this.gameObject);
        }
    }

    public void Move()
    {

    }
}
