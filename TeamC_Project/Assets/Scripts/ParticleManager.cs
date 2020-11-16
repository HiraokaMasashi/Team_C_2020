using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem[] particles;//パーティクルの配列

    /// <summary>
    /// 1度だけパーティクルを再生
    /// </summary>
    /// <param name="particle"></param>
    public void OncePlayParticle(GameObject particle)
    {
        if (particle.GetComponent<ParticleSystem>() == null) return;

        ParticleSystem p = particle.GetComponent<ParticleSystem>();
        p.Play();
        DestroyParticle(p.gameObject);
    }

    /// <summary>
    /// パーティクルの再生実行
    /// </summary>
    /// <param name="particle"></param>
    public void StartParticle(GameObject particle)
    {
        if (particle.GetComponent<ParticleSystem>() == null) return;

        ParticleSystem p = particle.GetComponent<ParticleSystem>();
        p.Play();
    }

    /// <summary>
    /// パーティクルの停止実行
    /// </summary>
    /// <param name="particle"></param>
    public void StopParticle(GameObject particle)
    {
        if (particle.GetComponent<ParticleSystem>() == null) return;

        ParticleSystem p = particle.GetComponent<ParticleSystem>();
        p.Stop();
        Destroy(particle, p.main.duration);
    }

    public void DestroyParticle(GameObject particle, float destroyTime = 0.0f)
    {
        if (particle.GetComponent<ParticleSystem>() == null) return;

        ParticleSystem p = particle.GetComponent<ParticleSystem>();
        Destroy(particle, p.main.duration + destroyTime);
    }

    /// <summary>
    /// パーティクルを生成
    /// </summary>
    /// <param name="num"></param>
    /// <returns></returns>
    public GameObject GenerateParticle(int num = 0)
    {
        if (particles.Length == 0) return null;

        GameObject particle = particles[num].gameObject;
        GameObject obj = Instantiate(particle, Vector3.zero, Quaternion.identity);

        return obj;
    }

    /// <summary>
    /// パーティクルを子オブジェクトで生成
    /// </summary>
    /// <param name="num"></param>
    /// <returns></returns>
    public GameObject GenerateParticleInChildren(int num = 0)
    {
        GameObject particle = particles[num].gameObject;
        GameObject obj = Instantiate(particle, Vector3.zero, Quaternion.identity);
        obj.transform.parent = transform;

        return obj;
    }
}
