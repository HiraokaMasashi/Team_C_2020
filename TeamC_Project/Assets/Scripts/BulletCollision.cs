using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCollision : MonoBehaviour
{
    private ScrewCollision screwCollision;

    public int Attack
    {
        get;
        set;
    } = 1;

    public bool IsPenetrate
    {
        get;
        set;
    } = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.name.Contains("Cube"))
        {
            other.transform.GetComponent<Health>().Damage(Attack);
            if (other.transform.GetComponent<Health>().IsDead)
            {
                screwCollision = GameObject.Find("ScrewParticle(Clone)").GetComponent<ScrewCollision>();
                screwCollision.RemoveEnemy(other.gameObject);
            }
            if (!IsPenetrate) Destroy(gameObject);
        }
    }
}
