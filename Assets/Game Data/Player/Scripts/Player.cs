using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    #region Dependices
    [SerializeField] PlayerConnector playerConnector;

    NavMeshAgent agent;
    Animator animController;

    #endregion

    #region Movement Variables

    Vector3 movePos;
    int terrainLayer = 1 << 6;
    float closeEnough = 0.2f;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        //Dependience collection
        agent = GetComponent<NavMeshAgent>();
        animController = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //If the distance between the movement order and current position is less than 0.2, stop walking and runing animations.
        if (agent.hasPath)
        {
            if (agent.remainingDistance < closeEnough)
            {
                animController.SetBool(playerConnector.animRunHash, false);
                animController.SetBool(playerConnector.animWalkHash, false);
            }
        }
    }

    /// <summary>
    /// Function that controls issuing a movement command to the player.
    /// </summary>
    public void PlayerMove()
    {
        //Get a terrain positon from a click on the screen.
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, terrainLayer))
        {
            //Assigned terrain poition to the movePos variable
            movePos = hit.point;
        }
        //Sets the players next walk/run position via the Navmesh agent component.
        agent.SetDestination(movePos);

        //If the distance between the issues movement order position is greater than the walk threshol, makes the player run to the location.
        if (agent.remainingDistance > playerConnector.walkThreshold)
        {
            agent.speed = playerConnector.runSpeed;
            animController.SetBool(playerConnector.animRunHash, true);
            animController.SetBool(playerConnector.animWalkHash, false);
        }
        //Else the player will walk to the location
        else if(agent.remainingDistance < playerConnector.walkThreshold)
        {
            agent.speed = playerConnector.walkSpeed;
            animController.SetBool(playerConnector.animWalkHash, true);
            animController.SetBool(playerConnector.animRunHash, false);
        }
    }
}
