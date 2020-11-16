using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrewCollision : MonoBehaviour
{
    private List<GameObject> objects;//スクリューにヒットしている敵
    private GameObject player;

    private void Start()
    {
        objects = new List<GameObject>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy" || other.gameObject.tag == "Bomb")
        {
            if (other.gameObject.name.Contains("Boss")) return;

            if (other.GetComponent<SetUpScrew>().IsStan) return;

            //既にヒットしていたものなら、return
            foreach (var o in objects)
            {
                if (other.gameObject == o) break;
            }

            //ヒットした敵との距離を計算
            GameObject enemy = other.gameObject;
            float distance;
            if (GetComponent<Screw>().GetScrewType() == Screw.ScrewType.INHALE)
                distance = Mathf.Abs(Vector3.Distance(player.transform.position, enemy.transform.position));
            else
                distance = Mathf.Abs(Vector3.Distance(transform.position, enemy.transform.position));
            enemy.GetComponent<SetUpScrew>().HitScrew(distance, transform.position.x);
            //スクリューにヒットしている敵をリストに格納
            objects.Add(enemy);
        }
    }

    /// <summary>
    /// リストからの削除(index指定)
    /// </summary>
    /// <param name="index"></param>
    public void RemoveObject(int index)
    {
        objects.RemoveAt(index);
    }

    /// <summary>
    /// リストからの削除(object指定)
    /// </summary>
    /// <param name="obj"></param>
    public void RemoveObject(GameObject obj)
    {
        objects.Remove(obj);
    }

    /// <summary>
    /// スクリューにヒットしている敵を返す
    /// </summary>
    /// <returns></returns>
    public List<GameObject> GetObjects()
    {
        return objects;
    }
}
