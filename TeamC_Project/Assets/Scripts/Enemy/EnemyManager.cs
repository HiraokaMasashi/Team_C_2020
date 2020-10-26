using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    //敵のPrefab
    public GameObject enemyPrefab;
    //敵生成時間間隔
    private float interval;
    //経過時間
    private float time = 0f;
    //敵の生成パターン
    public Transform[] instancePattern;

    // Start is called before the first frame update
    void Start()
    {
        //時間間隔
        interval = 5f;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        //経過時間が生成時間になったとき(生成時間より大きくなったとき)
        if (time > interval)
        {
            //enemyをインスタンス化する(生成する)
            GameObject enemy = Instantiate(enemyPrefab);
            //生成した敵の座標を決定する(現状　このオブジェクトの座標に生成)
            enemy.transform.position = transform.position;
            //経過時間を初期化して再度時間計測を始める
            time = 0f;
        }
    }

    //敵をパターンごとに生成する
    public void Instance(int number)
    {
        int length = instancePattern[number].transform.childCount;
        for(int i = 0; i < length; i++)
        {
            //enemyをインスタンス化する(生成する)
            GameObject enemy = Instantiate(enemyPrefab);
            //生成した敵の座標を決定する
            enemy.transform.position = instancePattern[number].GetChild(i).position;
        }
    }
}
