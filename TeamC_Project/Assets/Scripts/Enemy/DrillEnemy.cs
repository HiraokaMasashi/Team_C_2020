using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrillEnemy : Enemy
{
    private GameObject drill;
    [SerializeField]
    private float rotateSpeed=1.0f;

    private ParticleManager particleManager;
    private GameObject particle;

    private SetUpScrew setUpScrew;

    protected override void Start()
    {
        base.Start();
        drill = transform.GetChild(0).gameObject;
        particleManager = GetComponent<ParticleManager>();
        particle = null;
        setUpScrew = GetComponent<SetUpScrew>();
    }

    protected override void Update()
    {
        if (setupScrew.IsStan)
        {
            particleManager.StopParticle(particle);
            return;
        }

        Move();
        Death();
        RotateDrill();
    }

    private void RotateDrill()
    {
        drill.transform.rotation *= Quaternion.Euler(0, 360 * Time.deltaTime * rotateSpeed, 0);

        if (particle != null || setupScrew.IsStan) return;
        particle = particleManager.GenerateParticleInChildren(1);
        particle.transform.position = drill.transform.position;
        particle.transform.rotation = drill.transform.rotation;
        particle.transform.localScale /= 2.0f;
        particleManager.StartParticle(particle);
    }
}
