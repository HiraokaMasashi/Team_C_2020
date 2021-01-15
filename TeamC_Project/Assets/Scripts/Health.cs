using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ParticleManager))]
public class Health : MonoBehaviour
{
    [SerializeField]
    private int hp = 1;//現在の体力
    /// <summary>
    /// 最大体力
    /// </summary>
    public int MaxHp
    {
        get;
        private set;
    }

    private ParticleManager particleManager;//パーティクルマネージャー
    private SoundManager soundManager;
    [SerializeField]
    private string deadSe;//死亡SE
    [SerializeField]
    private string damageSe;//ダメージSE

    [SerializeField]
    private GameObject dropPrefab;//死亡時のドロップアイテム
    [SerializeField, Tooltip("生成位置の調整")]
    private Vector3 adjustPosition;

    private bool isDamageEffect;//ダメージ演出中か
    [SerializeField]
    private MeshRenderer[] meshRenderers;//透過させるメッシュ
    [SerializeField]
    private float effectTime = 2.0f;//演出時間
    private float effectElapsedTime;//演出経過時間
    [SerializeField]
    private float effectSpeed = 0.5f;//演出速度

    private GameManager gameManager;//ゲームマネージャー
    [SerializeField]
    private float particleInstanceTime = 0.5f;//パーティクル生成時間

    [SerializeField]
    private Vector3 minScale = Vector3.one;//ボス死亡時の縮小値

    private Score score;
    private ScoreManager scoreManager;

    [Header("ボスの死亡エフェクトの表示位置の設定")]
    [SerializeField, Range(1.0f, 5.0f)]
    private float instanceEffectPositionX = 1.0f;
    [SerializeField, Range(1.0f, 5.0f)]
    private float instanceEffectPositionY = 1.0f;
    [SerializeField]
    private float instanceEffectPositionZ = -1.0f;

    /// <summary>
    /// 現在の体力を返す
    /// </summary>
    public int Hp
    {
        get { return hp; }
    }

    /// <summary>
    /// 死亡判定
    /// </summary>
    public bool IsDead
    {
        //HPが0以下ならtrueを返す
        get { return hp <= 0; }
    }

    private void Start()
    {
        MaxHp = hp;

        particleManager = GetComponent<ParticleManager>();
        soundManager = SoundManager.Instance;
        isDamageEffect = false;
        effectElapsedTime = 0;

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        scoreManager = ScoreManager.Instance;

        if (GetComponent<Score>() != null)
            score = GetComponent<Score>();
    }

    private void Update()
    {
        DamageEffect();
        DeathEffect();
    }

    /// <summary>
    /// 死亡エフェクト
    /// </summary>
    private void DeathEffect()
    {
        if (!IsDead) return;

        //ボス以外
        if (!transform.name.Contains("Boss") && !transform.name.Contains("Screw"))
        {
            GameObject particle = particleManager.GenerateParticle();
            if (particle != null)
            {
                particle.transform.position = transform.position;
                particle.transform.rotation = Quaternion.Euler(-90.0f, 0.0f, 0.0f);
                particleManager.OncePlayParticle(particle);
            }

            if (deadSe != "")
                soundManager.PlaySeByName(deadSe);
            Destroy(gameObject);
        }

        if (transform.name.Contains("Boss") && transform.tag == "Enemy")
        {
            //ボスの死亡演出処理の実行
            if (gameManager.IsPerformance) return;

            gameManager.IsPerformance = true;
            GetComponent<Boss>().DestroyOtherObject();
            ExplosionInstance();
            StartCoroutine(BossDeadEffect());
        }
    }


    /// <summary>
    /// 衝突時に呼ぶ死亡処理
    /// </summary>
    public void HitDeath()
    {
        hp = 0;
        if (score != null)
            scoreManager.AddScore(score.GetScore());

        if (dropPrefab != null)
            Instantiate(dropPrefab, transform.position + adjustPosition, Quaternion.identity);
    }

    /// <summary>
    /// ダメージ処理
    /// </summary>
    /// <param name="damage">ダメージ量</param>
    public void Damage(int damage)
    {
        hp -= damage;
        if (damageSe != "")
            soundManager.PlaySeByName(damageSe);
        if (hp <= 0)
        {
            hp = 0;
            if (score != null && !gameObject.name.Contains("Boss"))
                scoreManager.AddScore(score.GetScore());

            if (dropPrefab != null)
                Instantiate(dropPrefab, transform.position + adjustPosition, Quaternion.identity);
        }
        else
            isDamageEffect = true;
    }

    /// <summary>
    /// ダメージ演出処理
    /// </summary>
    private void DamageEffect()
    {
        if (IsDead) return;

        //ダメージ演出中でない、またはポーズ中はreturn
        if (!isDamageEffect || Time.timeScale == 0) return;

        effectElapsedTime += Time.deltaTime;
        //演出時間を過ぎたら
        if (effectElapsedTime >= effectTime)
        {
            //配列に格納したメッシュを全て映す
            foreach (var mesh in meshRenderers)
            {
                mesh.enabled = true;
            }
            isDamageEffect = false;
            effectElapsedTime = 0.0f;
            return;
        }

        //メッシュの描画を切り替える
        foreach (var mesh in meshRenderers)
        {
            mesh.enabled = !mesh.enabled;
        }
    }

    /// <summary>
    /// 爆発エフェクトの生成処理
    /// </summary>
    /// <param name="adjustPositionX">x方向の調整</param>
    /// <param name="adjustPositionY">y方向の調整</param>
    private void ExplosionInstance(float adjustPositionX = 0.0f, float adjustPositionY = 0.0f)
    {
        GameObject particle = particleManager.GenerateParticle(1);
        particle.transform.position = transform.position + new Vector3(adjustPositionX, adjustPositionY, instanceEffectPositionZ);
        float randomScale = Random.Range(0.5f, 1.0f);
        particle.transform.localScale = new Vector3(randomScale, randomScale, randomScale);
        particle.transform.rotation = Quaternion.Euler(90, 0, 0);
        particleManager.OncePlayParticle(particle);
    }

    /// <summary>
    /// ボスの死亡演出処理
    /// </summary>
    /// <returns></returns>
    private IEnumerator BossDeadEffect()
    {
        //メッシュの描画を切り替える
        foreach (var mesh in meshRenderers)
        {
            mesh.enabled = true;
        }
        Vector3 scale = transform.localScale;
        float elapsedTime = 0;
        while (true)
        {
            //ボスのスケールを縮小する
            scale -= Vector3.one * Time.deltaTime * effectSpeed;
            transform.localScale = scale;
            transform.rotation *= Quaternion.Euler(5 * Time.deltaTime, 0, 10 * Time.deltaTime);

            elapsedTime += Time.deltaTime;
            //一定時間毎に爆発パーティクルを生成
            if (elapsedTime >= particleInstanceTime)
            {
                float x = Random.Range(-instanceEffectPositionX, instanceEffectPositionX);
                float y = Random.Range(-instanceEffectPositionY, instanceEffectPositionY);
                ExplosionInstance(x, y);
                if (damageSe != "")
                    soundManager.PlaySeByName(damageSe);
                elapsedTime = 0;
            }

            //最小縮小値までいったら削除
            if (transform.localScale.x <= minScale.x || transform.localScale.y <= minScale.y || transform.localScale.z <= minScale.z)
            {
                if (score != null)
                    scoreManager.AddScore(score.GetScore());
                gameManager.IsPerformance = false;
                ExplosionInstance();
                if (deadSe != "")
                    soundManager.PlaySeByName(deadSe);
                Destroy(gameObject);
                yield break;
            }

            yield return null;
        }

    }
}
