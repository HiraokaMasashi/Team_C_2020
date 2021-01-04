using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissedEnemy : SingletonMonoBehaviour<MissedEnemy>
{
    private static int missedEnemyCount;
    [SerializeField]
    private string se;

    private GameObject playerObject;

    // Start is called before the first frame update
    void Start()
    {
        missedEnemyCount = 0;
        playerObject = GameObject.FindGameObjectWithTag("Player");
    }

    public static int GetMissedEnemyCount()
    {
        return missedEnemyCount;
    }

    public void MissCountUp()
    {
        if (playerObject == null) return;

        SoundManager.Instance.PlaySeByName(se);
        missedEnemyCount++;
    }
}
