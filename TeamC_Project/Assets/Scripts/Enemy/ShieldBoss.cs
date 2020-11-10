using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldBoss : MonoBehaviour
{
    private BulletController bulletController;

    private GameManager gameManager;

    [SerializeField]
    private float bulletSpeed = 500.0f;
    [SerializeField]
    private int shotBulletCount = 20;
    [SerializeField]
    private float shotInterval = 5.0f;
    private float elapsedTime;
    [SerializeField]
    private int shotbarrageRows = 4;

    private bool isShot;

    // Start is called before the first frame update
    void Start()
    {
        bulletController = GetComponent<BulletController>();
        elapsedTime = 0.0f;
        isShot = false;

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        gameManager.SetBossEnemy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        Shot();
    }

    private void Shot()
    {
        if (isShot) return;

        elapsedTime += Time.deltaTime;

        if (elapsedTime < shotInterval) return;
        {
            StartCoroutine(ShotBarrage());
        }
    }

    private IEnumerator ShotBarrage()
    {
        isShot = true;
        int rad = 0;
        while (isShot)
        {
            for (int j = 0; j < shotBulletCount; j++)
            {
                for (int i = 0; i < shotbarrageRows; i++)
                {
                    Vector3 dir = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0.0f);
                    bulletController.GenerateBullet(transform.position, dir, bulletSpeed, 20.0f, "Enemy");
                }
                rad += 8;

                if (j == shotBulletCount - 1)
                {
                    isShot = false;
                    yield break;
                }
                else
                    yield return new WaitForSeconds(0.05f);
            }
        }
    }
}
