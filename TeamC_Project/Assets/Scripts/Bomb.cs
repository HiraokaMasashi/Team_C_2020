using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    private GameObject player;
    private ParticleManager particleManager;
    private NormalBoss boss;
    private SetUpScrew setUpScrew;

    private Vector3 direction;
    [SerializeField]
    private float moveSpeed = 0.5f;

    [SerializeField]
    private Vector3 destroyZone = new Vector3(16, -11, 0);

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        particleManager = GetComponent<ParticleManager>();
        boss = GameObject.Find("BombBoss(Clone)").GetComponent<NormalBoss>();
        setUpScrew = GetComponent<SetUpScrew>();

        direction = player.transform.position - transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (setUpScrew.IsStan) return;

        Move();
        if (GetIsDestroy())
            DisConnect();
    }

    private void Move()
    {
        Vector3 position = transform.position;
        position += direction.normalized * Time.deltaTime * moveSpeed;
        transform.position = position;
    }

    private bool GetIsDestroy()
    {
        bool isDestroy = false;

        if (transform.position.x <= -destroyZone.x || transform.position.x >= destroyZone.x
            || transform.position.y <= destroyZone.y)
            isDestroy = true;

        return isDestroy;
    }

    private void DisConnect()
    {
        boss.RemoveBomb(gameObject);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PlayerBullet")
        {
            if (!other.gameObject.GetComponent<Bullet>().IsPenetrate)
                other.gameObject.GetComponent<Bullet>().Disconnect();

            GameObject[] screws = GameObject.FindGameObjectsWithTag("Screw");
            for (int i = screws.Length - 1; i >= 0; i--)
            {
                int length = screws[i].GetComponent<ScrewCollision>().GetObjects().Count;
                for (int j = length - 1; j >= 0; j--)
                {
                    screws[i].GetComponent<ScrewCollision>().RemoveObject(j);
                }
            }

            GameObject particle = particleManager.GenerateParticle();
            particle.transform.position = transform.position;
            particle.transform.rotation = Quaternion.Euler(-90, 0, 0);
            particleManager.OncePlayParticle(particle);
            DisConnect();
        }

        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Blast")
        {
            GameObject[] screws = GameObject.FindGameObjectsWithTag("Screw");
            for (int i = screws.Length - 1; i >= 0; i--)
            {
                int length = screws[i].GetComponent<ScrewCollision>().GetObjects().Count;
                for (int j = length - 1; j >= 0; j--)
                {
                    screws[i].GetComponent<ScrewCollision>().RemoveObject(j);
                }
            }

            GameObject particle = particleManager.GenerateParticle();
            particle.transform.position = transform.position;
            particle.transform.rotation = Quaternion.Euler(-90, 0, 0);
            particleManager.OncePlayParticle(particle);
            DisConnect();
        }
    }
}
