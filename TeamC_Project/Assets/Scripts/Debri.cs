using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticlaManager))]
public class Debri : SetUpScrew
{
    private ScoreManager scoreManager;
    private ParticlaManager particlaManager;

    private ScrewCollision screwCollision;

    // Start is called before the first frame update
    protected override void Start()
    {
        distanceY = 0.0f;
        adjustPositionX = 0.0f;
        currentDirection = MoveDirection.NONE;

        scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
        particlaManager = GetComponent<ParticlaManager>();
    }

    public override void StanMove(Vector3 basePosition)
    {
        Vector3 position = transform.position;
        SetDestination(basePosition, position, ref distance, ref destination);
        CheckDistance(basePosition, position, ref distance, ref destination);

        //目的地に向かう
        float currentLocation = (Time.deltaTime * speed) / distance;
        transform.position = Vector3.Lerp(position, destination, currentLocation);
        distanceY -= Time.deltaTime;
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

    private void PlayDestroyParticle(GameObject enemy = null)
    {
        screwCollision = GameObject.FindGameObjectWithTag("Screw").GetComponent<ScrewCollision>();
        screwCollision.RemoveDebri(gameObject);
        if (enemy != null)
            screwCollision.RemoveEnemy(enemy);

        GameObject particle = particlaManager.GenerateParticle();
        if (particle != null)
        {
            particle.transform.position = transform.position;
            particlaManager.OncePlayParticle(particle);
        }
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Enemy")
        {
            if (!IsHitScrew) return;

            other.GetComponent<Health>().HitDeath();
            scoreManager.AddScore(other.GetComponent<Score>().GetScore());
            PlayDestroyParticle(other.gameObject);
        }

        if (other.transform.tag == "Player")
        {
            if (!IsHitScrew) return;

            other.GetComponent<Health>().Damage(1);
            PlayDestroyParticle();
        }
    }
}
