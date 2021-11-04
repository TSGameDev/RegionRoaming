using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RegionRoaming;

public class RaycastBrain : MonoBehaviour
{
    NavMeshAgent agent;
    [SerializeField] Region region;
    GameObject targetCube;

    private void Awake()
    {
        targetCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        targetCube.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        agent = GetComponent<NavMeshAgent>();
        CalculateNewPath();
    }

    private void Update()
    {
        if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
        {
             CalculateNewPath();
        }
    }

    void CalculateNewPath()
    {
        Vector3 destination = region.PickRandomRaycastLocation();
        agent.SetDestination(destination);
        targetCube.transform.position = destination;
    }
}
