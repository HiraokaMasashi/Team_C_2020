﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrewCollision : MonoBehaviour
{
    private List<GameObject> enemies;//スクリューにヒットしている敵
    private GameObject player;//プレイヤー

    private void Start()
    {
        enemies = new List<GameObject>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter(Collider other)
    {
        //敵等の状態変化処理

        //本来はタグで行う
        if (other.gameObject.name.Contains("Cube"))
        {
            //ヒットした敵との距離を計算
            float distance = Mathf.Abs(Vector3.Distance(player.transform.position, other.transform.position));
            GameObject enemy = other.gameObject;
            enemy.GetComponent<SetUpScrew>().HitScrew(distance);
            //スクリューにヒットしている敵をリストに格納
            enemies.Add(enemy);
        }
    }

    public void RemoveEnemy(int index)
    {
        enemies.RemoveAt(index);
    }

    public void RemoveEnemy(GameObject enemy)
    {
        enemies.Remove(enemy);
    }

    /// <summary>
    /// スクリューにヒットしている敵を返す
    /// </summary>
    /// <returns></returns>
    public List<GameObject> GetEnemies()
    {
        return enemies;
    }
}
