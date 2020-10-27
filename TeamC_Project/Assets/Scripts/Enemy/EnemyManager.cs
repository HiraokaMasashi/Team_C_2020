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
    //現在Waveの敵
    private GameObject[] waveEnemeis;
    //Wave数
    [SerializeField]
    private int maxWave;
    //現在のWave
    private int wave;
    //Waveエンドフラグ
    public bool isEnd;

    // Start is called before the first frame update
    void Start()
    {
        //時間間隔
        interval = 5f;
        wave = 0;
        StartCoroutine(Instance(wave));
    }

    // Update is called once per frame
    void Update()
    {
        /*
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
        */


        if (waveEnemeis != null)
        {
            int count = 0;
            //現在残っている敵の数をカウント
            foreach (var e in waveEnemeis)
            {
                if (e == null)
                    count++;
            }
            //敵が全滅したらWaveを進める
            if (count == waveEnemeis.Length)
            {
                waveEnemeis = null;
                wave++;
                //新しい敵を生成
                if (wave < maxWave)
                    StartCoroutine(Instance(wave));
                //Waveの最後ならエンドフラグをtrueに
                if (wave == maxWave)
                    isEnd = true;
            }
        }
    }

    //敵をパターンごとに生成する
    public void InstancePatternEnemy(int number)
    {
        int length = instancePattern[number].transform.childCount;
        GameObject[] enemies = new GameObject[length];
        for (int i = 0; i < length; i++)
        {
            Debug.Log(length);
            //enemyをインスタンス化する(生成する)
            GameObject enemy = Instantiate(enemyPrefab);
            enemies[i] = enemy;
            //生成した敵の座標を決定する
            enemies[i].transform.position = instancePattern[number].GetChild(i).position;
            //enemy.transform.position = transform.position;
        }
        waveEnemeis = enemies;
        StartCoroutine(FrameIn(enemies));
    }

    //生成した敵を上からフレームインさせる
    private IEnumerator FrameIn(GameObject[] enemies)
    {
        int time = 20;
        while (time > 0)
        {
            yield return null;
            foreach (var e in enemies)
            {
                if (e != null)
                    e.transform.position -= Vector3.up * 0.5f;
            }
            time--;
        }
    }

    //生成に少し間を置く
    private IEnumerator Instance(int wave)
    {
        yield return new WaitForSeconds(3);
        InstancePatternEnemy(wave);
    }
}
