using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCollision : MonoBehaviour
{
    private ScrewCollision screwCollision;
    [SerializeField]
    private Vector3 destroyZone;

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

    private void FixedUpdate()
    {
        if (GetIsDestroy())
            Destroy(gameObject);
    }

    private bool GetIsDestroy()
    {
        bool isDestroy = false;

        if (transform.position.x <= -destroyZone.x || transform.position.x >= destroyZone.x
            || transform.position.y >= destroyZone.y)
            isDestroy = true;

        return isDestroy;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Enemy")
        {
            other.transform.GetComponent<Health>().Damage(Attack);
            if (other.transform.GetComponent<Health>().IsDead)
            {
                if (GameObject.Find("ScrewParticle(Clone)") != null)
                {
                    screwCollision = GameObject.Find("ScrewParticle(Clone)").GetComponent<ScrewCollision>();
                    screwCollision.RemoveEnemy(other.gameObject);
                }
            }
            if (!IsPenetrate) Destroy(gameObject);
        }
    }
}
