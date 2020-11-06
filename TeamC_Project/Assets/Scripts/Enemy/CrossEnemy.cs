using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossEnemy : Enemy
{
    private Vector3 destination;
    private float distance;

    [SerializeField]
    private Vector3 minLeftDestinationRange;
    [SerializeField]
    private Vector3 maxLeftDestinationRange;
    [SerializeField]
    private Vector3 minRightDestinationRange;
    [SerializeField]
    private Vector3 maxRightDestinationRange;

    private Vector3 cameraPosition;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        cameraPosition = Camera.main.transform.position;
        SetDestination();
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (setupScrew.IsStan) return;

        Move();
        Death();
    }

    protected override void Move()
    {
        Vector3 position = transform.position;
        distance = Vector3.Distance(position, destination);
        MoveEnd();

        //目的地に向かう
        float currentLocation = (Time.deltaTime * moveSpeed) / distance;
        transform.position = Vector3.Lerp(position, destination, currentLocation);
    }

    private void SetDestination()
    {
        float x, y, z;
        if (transform.position.x <= cameraPosition.x)
        {
            x = Random.Range(minRightDestinationRange.x, maxRightDestinationRange.x);
            y = Random.Range(minRightDestinationRange.y, maxRightDestinationRange.y);
            z = Random.Range(minRightDestinationRange.z, maxRightDestinationRange.z);
        }
        else
        {
            x = Random.Range(minLeftDestinationRange.x, maxLeftDestinationRange.x);
            y = Random.Range(minLeftDestinationRange.y, maxLeftDestinationRange.y);
            z = Random.Range(minLeftDestinationRange.z, maxLeftDestinationRange.z);
        }
        destination = new Vector3(x, y, z);
    }

    private void MoveEnd()
    {
        if (distance <= 0.1f)
        {
            destination = new Vector3(transform.position.x, destroyZone.y, transform.position.z) + Vector3.down;
            distance = Vector3.Distance(transform.position, destination);
        }
    }
}
