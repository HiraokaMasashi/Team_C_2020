using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround : MonoBehaviour
{
    [SerializeField]
    float speed = 1;

    [SerializeField]
    float startScreenY = 40;
    [SerializeField]
    float endScreenY=-18f;

    void Update()
    {
        transform.position -= new Vector3(0, Time.deltaTime * speed);
        if (transform.position.y <= endScreenY)
        {
            transform.position = new Vector3(0, startScreenY, 10);
        }
    }
}
