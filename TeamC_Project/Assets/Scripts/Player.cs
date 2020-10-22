﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	[SerializeField]
	private GameObject bulletPrefab;

	private InputManager inputManager;

	[SerializeField]
	private Vector3 minPosition;
	[SerializeField]
	private Vector3 maxPosition;

	private Screw screw;
	private ChargeBullet chargeBullet;

	[SerializeField]
	private float moveSpeed = 4.0f;

	// Start is called before the first frame update
	void Start()
	{
		inputManager = GameObject.Find("InputManager").GetComponent<InputManager>();
		screw = GetComponent<Screw>();
		chargeBullet = GetComponent<ChargeBullet>();
	}

	// Update is called once per frame
	void Update()
	{
		Move();
		ShotBullet();
	}

	private void ShotBullet()
	{
		if (screw.GetMode() != Screw.Mode.NORMAL) return;

		if (inputManager.GetA_ButtonDown())
		{
			Instantiate(bulletPrefab, transform.position, Quaternion.identity);
			chargeBullet.ResetCharge();
		}
	}

	//移動の処理
	private void Move()
	{
		//横軸の値を返す
		float x = inputManager.GetL_Stick_Horizontal();

		//縦軸の値を返す
		float y = inputManager.GetL_Stick_Vertical();
		if (screw.GetMode() == Screw.Mode.SCREW) y = 0;

		//移動制御＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊今回追加
		Vector3 nextPosition = transform.position + new Vector3(x, y, 0) * Time.deltaTime * moveSpeed;
		//移動できる範囲をMathf.Clampで範囲指定して制御
		nextPosition = new Vector3(
			Mathf.Clamp(nextPosition.x, minPosition.x, maxPosition.x),
			Mathf.Clamp(nextPosition.y, minPosition.y, maxPosition.y),
			nextPosition.z);
		//現在位置にnextPositionを＋
		transform.position = nextPosition;
	}
}