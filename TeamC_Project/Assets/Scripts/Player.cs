using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	[SerializeField]
	private GameObject bulletPrefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		if (Input.GetKey(KeyCode.UpArrow))
		{
			transform.Translate( 0, 0.01f, 0);
		}
		if (Input.GetKey(KeyCode.DownArrow))
		{
			transform.Translate( 0, -0.01f, 0);
		}
		if (Input.GetKey (KeyCode.LeftArrow))
		{
			transform.Translate (-0.01f, 0, 0);
		}
		if (Input.GetKey (KeyCode.RightArrow))
		{
			transform.Translate ( 0.01f, 0, 0);
		}
		if (Input.GetKeyDown (KeyCode.A)) {
			Instantiate (bulletPrefab, transform.position, Quaternion.identity);
		}
		Move();
    }
	//移動の処理
	void Move()
	{
		//横軸の値を返す
		float x = Input.GetAxisRaw("Horizontal");

		//縦軸の値を返す
		float y = Input.GetAxisRaw("Vertical");

		//移動制御＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊今回追加
		Vector3 nextPosition = transform.position + new Vector3(x, y, 0) * Time.deltaTime * 4f;
		//移動できる範囲をMathf.Clampで範囲指定して制御
		nextPosition = new Vector3(
			Mathf.Clamp(nextPosition.x, -2.7f, 2.7f),
			Mathf.Clamp(nextPosition.y, -4f, 6f),
			nextPosition.z
			);
		//現在位置にnextPositionを＋
		transform.position = nextPosition;
	}
}
