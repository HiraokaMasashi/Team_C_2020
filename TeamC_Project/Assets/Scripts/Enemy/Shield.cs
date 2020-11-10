using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [SerializeField]
    private List<ShieldScrew> shieldScrews;

    // Update is called once per frame
    void Update()
    {
        ComeOff();
    }

    private void ComeOff()
    {
        if (shieldScrews.Count == 0) Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "PlyaerBullet")
        {
            Destroy(other.gameObject);
        }
    }
}
