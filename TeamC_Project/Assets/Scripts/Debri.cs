using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debri : SetUpScrew
{
    private ScoreManager scoreManager;

    private ParticlaManager particlaManager;

    // Start is called before the first frame update
    protected override void Start()
    {
        distanceY = 0.0f;
        adjustPositionX = 0.0f;
        currentDirection = MoveDirection.NONE;

        scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
        particlaManager = GetComponent<ParticlaManager>();
    }

    public override void HitScrew(float distance, float basePositionX)
    {
        IsHitScrew = true;
        distanceY = distance;
        if (transform.position.x <= basePositionX)
            currentDirection = MoveDirection.RIGHT;
        else
            currentDirection = MoveDirection.LEFT;
        SetAdjustPositionX();
    }

    private void PlayDestroyParticle()
    {
        GameObject particle = particlaManager.GenerateParticle();
        particlaManager.OncePlayParticle(particle);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Enemy")
        {
            other.GetComponent<Health>().HitDeath();
            scoreManager.AddScore(other.GetComponent<Score>().GetScore());
            PlayDestroyParticle();
        }

        if (other.transform.tag == "Player")
        {
            other.GetComponent<Health>().HitDeath();
            PlayDestroyParticle();
        }
    }
}
