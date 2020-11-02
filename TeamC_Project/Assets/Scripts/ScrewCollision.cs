using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrewCollision : MonoBehaviour
{
    private List<GameObject> enemies;//スクリューにヒットしている敵
    private GameObject player;//プレイヤー

    private BoxCollider boxCollider;

    private void Start()
    {
        enemies = new List<GameObject>();
        player = GameObject.FindGameObjectWithTag("Player");

        boxCollider = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //本来はタグで行う
        if (other.gameObject.tag == "Enemy")
        {
            //既にヒットしていたものなら、return
            foreach (var e in enemies)
            {
                if (other.gameObject == e) return;
            }

            //ヒットした敵との距離を計算
            float distance = Mathf.Abs(Vector3.Distance(player.transform.position, other.transform.position));
            GameObject enemy = other.gameObject;
            enemy.GetComponent<SetUpScrew>().HitScrew(distance, boxCollider.size.y);
            //スクリューにヒットしている敵をリストに格納
            enemies.Add(enemy);
        }
    }

    /// <summary>
    /// リストからの削除(index指定)
    /// </summary>
    /// <param name="index"></param>
    public void RemoveEnemy(int index)
    {
        enemies.RemoveAt(index);
    }

    /// <summary>
    /// リストからの削除(object指定)
    /// </summary>
    /// <param name="enemy"></param>
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

    /// <summary>
    /// 敵の座標の設定
    /// </summary>
    public void SetEnemiesPosition()
    {
        foreach(var e in enemies)
        {
            Vector3 position;
            SetUpScrew setUpScrew = e.GetComponent<SetUpScrew>();
            if (setUpScrew.GetStanLevel() == SetUpScrew.StanLevel.LEVEL1)
            {
                position = e.transform.position;
                //e.transform.position = 
            }
            else if (setUpScrew.GetStanLevel() == SetUpScrew.StanLevel.LEVEL2)
            {

            }
            else
            {

            }
        }
    }
}
