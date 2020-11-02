﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetUpScrew : MonoBehaviour
{
    [SerializeField]
    private float recoveryTime = 6.0f;
    private float stanElapsedTime;

    private float distanceY;
    private float adjustPositionX;

    [SerializeField]
    private float speed = 1.0f;

    [SerializeField]
    private float maxDistance = 12.0f;

    enum MoveDirection
    {
        NONE,
        RIGHT,
        LEFT,
    }
    private MoveDirection currentDirection;

    public bool IsStan
    {
        get;
        private set;
    } = false;

    public bool IsHitScrew
    {
        get;
        private set;
    } = false;

    // Start is called before the first frame update
    void Start()
    {
        stanElapsedTime = 0.0f;
        distanceY = 0.0f;
        adjustPositionX = 0.0f;
        currentDirection = MoveDirection.NONE;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 0) return;

        Recovery();
    }

    /// <summary>
    /// スタン回復処理
    /// </summary>
    private void Recovery()
    {
        if (!IsStan) return;
        if (IsHitScrew) return;

        stanElapsedTime += Time.deltaTime;
        if (stanElapsedTime >= recoveryTime)
        {
            IsStan = false;
            stanElapsedTime = 0.0f;
        }
    }

    /// <summary>
    /// スタン中の敵の並べ替え(移動)
    /// </summary>
    /// <param name="basePosition"></param>
    public void StanMove(Vector3 basePosition)
    {
        Vector3 position = transform.position;
        Vector3 destination = new Vector3();
        SetDestination(basePosition, ref destination);

        float distance = Vector3.Distance(position, destination);
        CheckDistance(basePosition, position, ref distance, ref destination);

        //目的地に向かう
        float currentLocation = (Time.deltaTime * speed) / distance;
        transform.position = Vector3.Lerp(position, destination, currentLocation);
    }

    /// <summary>
    /// 目的地の設定
    /// </summary>
    /// <param name="basePosition"></param>
    /// <param name="destination"></param>
    private void SetDestination(Vector3 basePosition, ref Vector3 destination)
    {
        destination = basePosition + new Vector3(adjustPositionX, distanceY, 0.0f);
    }

    /// <summary>
    /// 目的地までの距離を測る
    /// </summary>
    /// <param name="basePosition"></param>
    /// <param name="position"></param>
    /// <param name="distance"></param>
    /// <param name="destination"></param>
    private void CheckDistance(Vector3 basePosition, Vector3 position, ref float distance, ref Vector3 destination)
    {
        if (distance <= 0.1f)
        {
            if (currentDirection == MoveDirection.RIGHT)
                currentDirection = MoveDirection.LEFT;
            else if (currentDirection == MoveDirection.LEFT)
                currentDirection = MoveDirection.RIGHT;

            SetAdjustPositionX();
            SetDestination(basePosition, ref destination);
        }

        distance = Vector3.Distance(position, destination);
    }

    /// <summary>
    /// スクリューとの衝突時の処理
    /// </summary>
    /// <param name="distance"></param>
    /// <param name="basePositionX"></param>
    public void HitScrew(float distance, float basePositionX)
    {
        IsStan = true;
        IsHitScrew = true;
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
    private void SetAdjustPositionX()
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
    /// スクリューとの衝突終了時の処理
    /// </summary>
    public void LeaveScrew()
    {
        IsHitScrew = false;
    }
}
