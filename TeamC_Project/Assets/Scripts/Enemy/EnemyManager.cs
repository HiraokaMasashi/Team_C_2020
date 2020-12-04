using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnemyManager : MonoBehaviour
{
    //敵のPrefab
    [SerializeField]
    private GameObject[] enemyPrefabs;
    //敵の生成パターン
    [SerializeField]
    private Transform[] instancePattern;
    //現在Waveの敵
    private GameObject[] waveEnemeis;
    //Wave数
    [SerializeField]
    private int maxWave;
    //現在のWave
    private int wave;

    [Space]
    [SerializeField]
    private GameObject bossPrefab;
    [SerializeField]
    private Transform bossPattern;
    private GameObject boss;

    private SoundManager soundManager;
    [SerializeField]
    private string[] bgms;

    [SerializeField]
    private Text waveText;

    private GameManager gameManager;
    private GameObject player;
    [SerializeField]
    private Vector3 performancePosition;
    [SerializeField]
    private float performanceSpeed = 2.0f;

    //Waveエンドフラグ
    public bool IsEnd
    {
        get;
        private set;
    } = false;

    // Start is called before the first frame update
    void Start()
    {
        wave = 0;
        soundManager = SoundManager.Instance;
        soundManager.PlayBgmByName(bgms[0]);

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        player = GameObject.FindGameObjectWithTag("Player");

        //StartCoroutine(InstanceEnemy(wave));
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.GetResult() != GameManager.ResultMode.NONE) return;

        NextWave();
        DisPlayWave();
        PerformancePlayerPosition();
    }

    private void NextWave()
    {
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
                {
                    StartCoroutine(InstanceEnemy(wave));
                }

                //Waveの最後ならエンドフラグをtrueに
                else if (wave == maxWave)
                {
                    gameManager.IsPerformance = true;
                    soundManager.StopBgm();
                    soundManager.PlayBgmByName(bgms[1]);
                    //StartCoroutine(InstanceBossPerforme());
                }
            }
        }
    }

    private void InstanceBoss()
    {
        if (boss != null) return;
        if (bossPrefab == null) return;

        boss = Instantiate(bossPrefab, bossPattern.GetChild(0).position, bossPrefab.transform.rotation);
    }

    //敵をパターンごとに生成する
    public void InstancePatternEnemy(int number)
    {
        int length = instancePattern[number].transform.childCount;
        GameObject[] enemies = new GameObject[length];
        //Debug.Log(length);
        for (int i = 0; i < length; i++)
        {
            //enemyをインスタンス化する(生成する)
            GameObject enemy = Instantiate(enemyPrefabs[number]);
            enemies[i] = enemy;
            //生成した敵の座標を決定する
            enemies[i].transform.position = instancePattern[number].GetChild(i).position;
        }
        waveEnemeis = enemies;
        StartCoroutine(FrameIn(enemies));
    }

    //生成した敵を上からフレームインさせる
    private IEnumerator FrameIn(GameObject[] enemies)
    {
        float elapsedTime = 0;
        while (elapsedTime > 2.0f)
        {
            foreach (var e in enemies)
            {
                if (e != null)
                    e.transform.position -= Vector3.up * 0.5f * Time.deltaTime;
            }
            elapsedTime += Time.deltaTime;
            yield return null;
            //time--;
        }
    }

    //生成に少し間を置く
    private IEnumerator InstanceEnemy(int wave)
    {
        waveText.enabled = true;
        yield return new WaitForSeconds(3);

        InstancePatternEnemy(wave);
        waveText.enabled = false;
    }

    private void PerformancePlayerPosition()
    {
        if (!gameManager.IsPerformance) return;

        if (Vector3.Distance(player.transform.position, performancePosition) > 0.1)
        {
            Vector3 position = player.transform.position;
            position = (performancePosition - position).normalized;
            player.transform.position += position * Time.deltaTime * performanceSpeed;
        }
        else
            StartCoroutine(InstanceBossPerforme());
    }

    private IEnumerator InstanceBossPerforme()
    {
        waveText.enabled = true;
        yield return new WaitForSeconds(3.0f);

        InstanceBoss();
        gameManager.IsPerformance = false;
        waveText.enabled = false;
    }

    private void DisPlayWave()
    {
        if (waveText == null) return;

        if (wave < maxWave)
            waveText.text = "Wave: " + (wave + 1).ToString("D2") + " / " + maxWave.ToString("D2");
        else
            waveText.text = "BOSS Battle";
    }

    /// <summary>
    /// 外部クラスから初めの生成を行う
    /// </summary>
    public void StartInstanceEnemy()
    {
        StartCoroutine(InstanceEnemy(wave));
    }
}
