using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalBoss : Boss
{
    private GameObject player;

    [SerializeField]
    private Transform[] shotTransforms;

    private List<GameObject> bombs;
    enum ShotDirection
    {
        FORWARD,
        LEFT,
        RIGHT,
        NONE,
    }
    private int previousNum;

    private bool endShot;
    [SerializeField]
    private GameObject bombPrefab;
    [SerializeField]
    private string bombSe;

    // Start is called before the first frame update
    protected override void Start()
    {
        //shotInterval = Random.Range(minInterval, maxInterval);
        player = GameObject.FindGameObjectWithTag("Player");
        previousNum = -1;
        endShot = false;
        bombs = new List<GameObject>();

        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isFrameIn) return;

        if (health.IsDead)
        {
            StopAllCoroutines();
            return;
        }

        ChangePattern();
        ShotBullet();
        ShotBomb();
    }

    private void ChangePattern()
    {
        switch (pattern)
        {
            case BehaviourPattern.SHOT:
                if (endShot)
                {
                    endShot = false;
                    pattern = BehaviourPattern.SHOT_BOMB;
                }
                break;

            case BehaviourPattern.SHOT_BOMB:
                if (bombs.Count == 0 && endShot)
                {
                    endShot = false;
                    isPlayAlert = false;
                    pattern = BehaviourPattern.SHOT;
                }
                break;

            default:
                break;
        }
    }

    private void ShotBullet()
    {
        if (pattern != BehaviourPattern.SHOT) return;
        if (player == null) return;
        if (isShot) return;

        shotElapsedTime += Time.deltaTime;
        if (shotElapsedTime < shotInterval) return;

        isShot = true;
        StartCoroutine(RapidShot());
        //countbullet += 1;
        //roopIntervalTime = 0;
        //if (countbullet >= 5)
        //{
        //    countIntervalTime = 0;
        //    roopIntervalTime = 0;
        //    countbullet = 0;
        //}
    }

    private void ShotBomb()
    {
        if (pattern != BehaviourPattern.SHOT_BOMB) return;
        if (player == null) return;
        if (endShot) return;

        shotElapsedTime += Time.deltaTime;

        if(shotElapsedTime >= shotInterval - 1.0f && !isPlayAlert)
        {
            isPlayAlert = true;
            SoundManager.Instance.PlaySeByName(alertSe);
        }
        if (shotElapsedTime < shotInterval) return;

        for (int i = 0; i < shotTransforms.Length; i++)
        {
            Vector3 position = shotTransforms[i].position;
            Vector3 direction = (player.transform.position - shotTransforms[i].position).normalized;
            bombs.Add(bulletController.GenerateBomb(bombPrefab, position, direction, bombSe));
        }

        shotElapsedTime = 0.0f;
        endShot = true;
    }

    private void SetDirection(ref Vector3 direction)
    {
        int random;
        do
        {
            random = Random.Range(0, (int)ShotDirection.NONE);
        } while (random == previousNum);

        previousNum = random;
        if (random == 1)
            direction += Vector3.left;
        else if (random == 2)
            direction += Vector3.right;
    }

    public void RemoveBomb(GameObject bomb)
    {
        bombs.Remove(bomb);
    }

    private IEnumerator RapidShot()
    {
        while (isShot)
        {
            for (int i = 0; i < 5; i++)
            {
                Vector3 dir = Vector3.down;
                SetDirection(ref dir);
                for (int j = 0; j < shotTransforms.Length; j++)
                {
                    if (player == null)
                    {
                        isShot = false;
                        yield break;
                    }

                    Vector3 position = shotTransforms[j].position;
                    bulletController.GenerateBullet(position, dir, destroyTime, "Enemy");
                }
                yield return new WaitForSeconds(0.5f);
            }

            isShot = false;
            shotElapsedTime = 0.0f;
            endShot = true;
            yield break;
        }
    }

    public override void DestroyOtherObject()
    {
        base.DestroyOtherObject();
        for (int i = bombs.Count - 1; i >= 0; i--)
        {
            Destroy(bombs[i]);
        }
    }
}
