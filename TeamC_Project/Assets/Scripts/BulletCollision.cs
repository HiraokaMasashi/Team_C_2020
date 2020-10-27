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
            || transform.position.y >= destroyZone.y || transform.position.y <= -5.0f)
            isDestroy = true;

        return isDestroy;
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((other.transform.tag == "Player" && transform.tag == "EnemyBullet")
            || (other.transform.tag == "Enemy" && transform.tag == "PlayerBullet"))
        {
            other.transform.GetComponent<Health>().Damage(Attack);
            if (!IsPenetrate) Destroy(gameObject);
        }

        if (other.transform.tag == "EnemyBullet" && transform.tag == "PlayerBullet")
        {
            if (!IsPenetrate) Destroy(gameObject);
            Destroy(other.gameObject);
        }
    }
}
