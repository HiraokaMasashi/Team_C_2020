using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class ShieldScrew : MonoBehaviour
{
    private Health health;

    private Boss boss;

    [SerializeField]
    private float screwDamageInterval = 1.0f;//スクリューのダメージ間隔
    [SerializeField]
    private float drillDamageInterval = 0.1f;//ドリルのダメージ間隔
    private float elapsedTime;//経過時間

    [SerializeField]
    private float moveSpeed = 1.0f;//移動速度

    private bool isThrowAway;//外れたか
    [SerializeField]
    private float throwAwayPositionY = -10.0f;//外れた場合の目的地

    [SerializeField]
    private Vector3 minScale;//最大縮小サイズ

    private SoundManager soundManager;
    [SerializeField]
    private string se;

    private void Start()
    {
        health = GetComponent<Health>();
        elapsedTime = 0.0f;
        boss = transform.root.GetComponent<Boss>();
        isThrowAway = false;
        soundManager = SoundManager.Instance;
    }

    private void Update()
    {
        if (!isThrowAway) return;

        ThrowAway();
        ThrowDown();
    }

    /// <summary>
    /// 移動
    /// </summary>
    private void ThrowAway()
    {
        Vector3 position = transform.position;
        Vector3 destination = new Vector3(position.x, throwAwayPositionY, position.z);

        Vector3 dir = (destination - position).normalized;
        transform.position += dir * Time.deltaTime * moveSpeed;

        if (Vector3.Distance(transform.position, destination) <= 0.1f)
            Destroy(gameObject);
    }

    /// <summary>
    /// 回転と縮小
    /// </summary>
    private void ThrowDown()
    {
        Quaternion rotation = transform.rotation;
        rotation *= Quaternion.Euler(10 * Time.deltaTime, 0, 180 * Time.deltaTime);
        transform.rotation = rotation;

        Vector3 scale = transform.localScale;
        scale -= Vector3.one * Time.deltaTime * moveSpeed;
        transform.localScale = scale;

        if (transform.localScale.x <= minScale.x || transform.localScale.y <= minScale.y || transform.localScale.z <= minScale.z)
            Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!boss.GetFrameIn()) return;

        if (other.transform.tag == "Screw")
        {
            int damage = 1;
            StartCoroutine(DamageComeOff(damage));
            health.Damage(damage);
        }

        if (other.transform.tag == "Drill")
        {
            if (!other.GetComponent<Drill>().IsShot)
            {
                int damage = 10;
                StartCoroutine(DamageComeOff(damage));
                health.Damage(damage);
                other.gameObject.GetComponent<Health>().Damage(1);
            }
            else Destroy(other.gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (health.IsDead) return;

        if (other.transform.tag == "Screw")
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime < screwDamageInterval) return;
            elapsedTime = 0.0f;

            int damage = 1;
            StartCoroutine(DamageComeOff(damage));
            health.Damage(damage);
        }

        if (other.transform.tag == "Drill")
        {
            if (other.GetComponent<Drill>().IsShot) return;

            elapsedTime += Time.deltaTime;
            if (elapsedTime < drillDamageInterval) return;
            elapsedTime = 0.0f;

            int damage = 10;
            StartCoroutine(DamageComeOff(damage));
            health.Damage(damage);
            other.transform.GetComponent<Health>().Damage(1);
        }
    }

    private IEnumerator DamageComeOff(int damage)
    {
        Vector3 destination = transform.position - new Vector3(0, 0.05f * damage, 0);

        while (Vector3.Distance(transform.position, destination) > 0.01f)
        {
            Vector3 position = Vector3.Lerp(transform.position, destination, Time.deltaTime * moveSpeed);
            transform.position = position;
            yield return null;
        }

        if (health.IsDead)
        {
            isThrowAway = true;
            transform.parent = null;
            if (se != "")
                soundManager.PlaySeByName(se);
        }

        yield break;
    }
}
