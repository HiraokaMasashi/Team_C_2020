using UnityEngine;
using System.Collections;

public class BulletController : MonoBehaviour
{
    [SerializeField]
    private GameObject bulletPrefab;

    public void GenerateBullet(Vector3 position, Vector3 direction, float destroyTime, string tagName = "Player")
    {
        GameObject bullet = Instantiate(bulletPrefab, position, Quaternion.identity);
        bullet.tag = tagName + "Bullet";
        bullet.GetComponent<Bullet>().SetDirection(direction);
        bullet.transform.rotation = Quaternion.LookRotation(direction);

        GameObject particle = bullet.GetComponent<ParticleManager>().GenerateParticleInChildren();
        particle.transform.position = bullet.transform.position;
        particle.transform.rotation = Quaternion.LookRotation(direction, Vector3.back);
        particle.transform.localScale = Vector3.one;

        bullet.GetComponent<ParticleManager>().StartParticle(particle);
        bullet.GetComponent<ParticleManager>().DestroyParticle(particle, destroyTime);
        Destroy(bullet, destroyTime);
    }

    public GameObject GenerateBomb(GameObject bombPrefab, Vector3 position, Vector3 direction)
    {
        GameObject bomb = Instantiate(bombPrefab, position, Quaternion.identity);
        bomb.transform.rotation = Quaternion.LookRotation(direction);

        return bomb;
    }
}