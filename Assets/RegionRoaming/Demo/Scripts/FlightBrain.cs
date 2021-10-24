using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RegionRoaming;

public class FlightBrain : MonoBehaviour
{
    [SerializeField] Region region;
    [SerializeField] float speed;
    float stoppingDistance = 0.5f;
    GameObject targetCube;
    Vector3 destination;

    private void Awake()
    {
        targetCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        targetCube.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        CalculateNewPath();
    }

    private void Update()
    {
        float distanceRemaining = Vector3.Distance(destination, transform.position);
        if(distanceRemaining <= stoppingDistance)
        {
            CalculateNewPath();
        }

        transform.LookAt(destination);
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    void CalculateNewPath()
    {
        destination = region.PickRandomFlightLocation(15f, 2f, 10f);
        targetCube.transform.position = destination;
    }
}
