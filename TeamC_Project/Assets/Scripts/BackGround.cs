using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer back1, back2;
    [SerializeField]
    private float speed1, speed2;
    [SerializeField]
    private float length1, length2;
    private float height1, height2;
    private float foward1, foward2;

    private SpriteRenderer clone1, clone2;
    private float initialPos1, initialPos2;

    void Start()
    {
        foward1 = 10;
        foward2 = 100;
        height1 = back1.bounds.size.y / 2;
        height2 = back2.bounds.size.y / 2;

        clone1 = Instantiate(back1, back1.transform.position + Vector3.up * height1 * 2,Quaternion.identity,transform);
        clone2 = Instantiate(back2, back2.transform.position + Vector3.up * height2 * 2, Quaternion.identity,transform);
        initialPos1 = back1.transform.position.y;
        initialPos2 = back2.transform.position.y;
    }

    void Update()
    {
        back1.transform.position -= new Vector3(0, Time.deltaTime * speed1);
        clone1.transform.position -= new Vector3(0, Time.deltaTime * speed1);
        back2.transform.position -= new Vector3(0, Time.deltaTime * speed2);
        clone2.transform.position -= new Vector3(0, Time.deltaTime * speed2);

        if (back1.transform.position.y <= length1)
            back1.transform.position = clone1.transform.position + Vector3.up * height1 * 2;
        if (clone1.transform.position.y <= length1)
            clone1.transform.position = back1.transform.position + Vector3.up * height1 * 2;
        if (back2.transform.position.y <= length2)
            back2.transform.position = clone2.transform.position + Vector3.up * height2 * 2;
        if (clone2.transform.position.y <= length2)
            clone2.transform.position = back2.transform.position + Vector3.up * height2 * 2;
    }
}
