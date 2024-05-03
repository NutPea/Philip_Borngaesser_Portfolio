using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AStarMoveToWaypoints))]
[RequireComponent(typeof(AStarChaseController))]
public class PatrollingEnemyController : MonoBehaviour
{
    public enum PatrolingEnemyStates { Patrolling , AttackPlayer};
    public PatrolingEnemyStates currentState;

    public GameObject player;
    private AStarMoveToWaypoints moveToWaypoints;
    private AStarChaseController aStarChaseController;
    public float waitingTimeOnWaypoint = 1f;
    public float aggroRange = 1f;
    bool aggroTrigger;

    private void Awake()
    {
        moveToWaypoints = GetComponent<AStarMoveToWaypoints>();
        moveToWaypoints.waitTimeBetweenWaypoints = waitingTimeOnWaypoint;
        aStarChaseController = GetComponent<AStarChaseController>();
        player = GameObject.FindGameObjectWithTag("Player");
        aStarChaseController.targetGridMovementController = player.GetComponent<GridMovementController>();
        
    }

    //Late Start
    IEnumerator Start()
    {
        yield return new WaitForSeconds(0.1f);
        ChangeState(PatrolingEnemyStates.Patrolling);
    }

    void Update()
    {
        HandleDecision();
    }

    private void HandleDecision()
    {
        if(Vector2.Distance(transform.position,player.transform.position) < aggroRange && !aggroTrigger)
        {
            ChangeState(PatrolingEnemyStates.AttackPlayer);
            aggroTrigger = true;
        }
    }

    void ChangeState(PatrolingEnemyStates state)
    {
        currentState = state;
        switch (currentState)
        {
            case PatrolingEnemyStates.Patrolling: HandlePatrolling(); break;
            case PatrolingEnemyStates.AttackPlayer: HandleAttackPlayer(); break;
        }
    }

    private void HandleAttackPlayer()
    {
        moveToWaypoints.StopMoveingToWaypoints();
        currentState = PatrolingEnemyStates.AttackPlayer;
        aStarChaseController.StartChasing();
    }

    private void HandlePatrolling()
    {
        moveToWaypoints.MoveToNextWaypoint();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, aggroRange);
    }
}
