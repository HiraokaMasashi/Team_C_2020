using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blast : MonoBehaviour
{
    [SerializeField]
    private int playerDamage = 10;
    [SerializeField]
    private int enemyDamage = 10;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
            other.gameObject.GetComponent<Health>().Damage(playerDamage);

        if (other.gameObject.tag == "Enemy")
            other.gameObject.GetComponent<Health>().Damage(enemyDamage);
    }

}
