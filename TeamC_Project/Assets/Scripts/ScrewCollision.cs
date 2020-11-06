using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrewCollision : MonoBehaviour
{
    private List<GameObject> enemies;//スクリューにヒットしている敵
    private List<GameObject> debris;
    private GameObject player;//プレイヤー

    [SerializeField]
    private int maxWait = 4;//一列で並べられる上限

    private void Start()
    {
        enemies = new List<GameObject>();
        debris = new List<GameObject>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter(Collider other)
    {
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
            enemy.GetComponent<SetUpScrew>().HitScrew(distance, player.transform.position.x);
            //スクリューにヒットしている敵をリストに格納
            enemies.Add(enemy);
        }

        if(other.gameObject.tag == "Debri")
        {
            foreach(var d in debris)
            {
                if (other.gameObject == d) return;
            }

            float distance = Mathf.Abs(Vector3.Distance(player.transform.position, other.transform.position));
            GameObject debri = other.gameObject;
            debri.GetComponent<SetUpScrew>().HitScrew(distance, player.transform.position.x);
            debris.Add(debri);
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
    /// リストからの削除(index指定)
    /// </summary>
    /// <param name="index"></param>
    public void RemoveDebri(int index)
    {
        debris.RemoveAt(index);
    }

    /// <summary>
    /// リストからの削除(object指定)
    /// </summary>
    /// <param name="debri"></param>
    public void RemoveDebri(GameObject debri)
    {
        debris.Remove(debri);
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
    /// スクリューにヒットしている敵を返す
    /// </summary>
    /// <returns></returns>
    public List<GameObject> GetDebris()
    {
        return debris;
    }
}
