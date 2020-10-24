using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestObjMove : MonoBehaviour
{
    float xv;
    // Start is called before the first frame update
    void Start()
    {
        xv = 1;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Rotate(new Vector3(0, 1, 0));
    }
}
