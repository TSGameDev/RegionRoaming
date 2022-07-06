using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    NavMeshAgent agent;
    int terrainLayer = 1 << 6;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayerMove()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, Mathf.Infinity, terrainLayer))
        {
            agent.SetDestination(hit.point);
            Debug.Log(hit.point);
        }
    }
}
