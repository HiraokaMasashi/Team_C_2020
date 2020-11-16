using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [SerializeField]
    private Vector3 destination = new Vector3(0, -4, 0);
    [SerializeField]
    private float moveSpeed = 1.0f;

    private bool isDown = false;

    // Update is called once per frame
    void Update()
    {
        ComeOff();
    }

    private void ComeOff()
    {
        if (transform.childCount == 0)
        {
            if (isDown) return;

            isDown = true;
            transform.parent = null;
            StartCoroutine(MoveDown());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "PlayerBullet")
        {
            other.gameObject.GetComponent<BulletCollision>().Disconnect();
        }
    }

    private IEnumerator MoveDown()
    {
        while (Vector3.Distance(transform.position, destination) > 0.01f)
        {
            Vector3 position = Vector3.Lerp(transform.position, destination, Time.deltaTime * moveSpeed);
            transform.position = position;
            transform.Rotate(new Vector3(-360, 0, 0) * Time.deltaTime);
            yield return null;
        }

        Destroy(gameObject);
        yield break;
    }
}
