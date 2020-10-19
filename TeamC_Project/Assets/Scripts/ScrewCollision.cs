using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrewCollision : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        //敵等の状態変化処理
        //ヒットしたオブジェクトとの距離を計算
        float distance = Mathf.Abs(Vector3.Distance(transform.position, other.transform.position));
    }

    private void OnTriggerExit(Collider other)
    {
        //敵等の状態変化処理
    }
}
