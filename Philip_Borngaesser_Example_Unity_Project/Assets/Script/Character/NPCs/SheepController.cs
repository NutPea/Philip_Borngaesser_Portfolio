using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SheepController : MonoBehaviour
{
    GridMovementController _gridMovementController;
    AStarMoveToMovementNode _aStarMovement;
    AStarMoveToWaypoints _AStarMoveToWaypoints;
    public bool canNotMove;

    public Equipment.Item runAwayEquipment;
    public Equipment.Item killEquipment;

    public float runAwayTime = 2f;
    public float timeBetweenMovesWhileRunAway = 0.5f;
    public UnityEvent onRunAwayStart = new UnityEvent();
    public UnityEvent onRunAwayStop = new UnityEvent();
    public float timeUntilDeath = 1f;
    public UnityEvent onDeathStart = new UnityEvent();
    public UnityEvent onDeathRemove = new UnityEvent();

    void Start()
    {
        _gridMovementController = GetComponent<GridMovementController>();
        _gridMovementController.OnOccupiedGridMovementRequest.AddListener(OnInteractWithSheep);
        _aStarMovement = GetComponent<AStarMoveToMovementNode>();
        _AStarMoveToWaypoints = GetComponent<AStarMoveToWaypoints>();
    }

    private void OnInteractWithSheep(Transform player)
    {
        PlayerEquipmentController playerEquipmentController = player.GetComponent<PlayerEquipmentController>();
        if(playerEquipmentController == null) return;
        if(playerEquipmentController.currentItemEnum == killEquipment)
        {
            KillSheep();
        }
        else if(playerEquipmentController.currentItemEnum == runAwayEquipment)
        {
            OnSheepRunAway();
        }
    }

    private void OnSheepRunAway()
    {
        if (canNotMove) return;
        StartCoroutine(RunAwayCouroutine());
    }

    IEnumerator RunAwayCouroutine()
    {
        float beforeRunAwaySpeed = _aStarMovement.timeBetweenMoves;
        _aStarMovement.timeBetweenMoves = timeBetweenMovesWhileRunAway;
        onRunAwayStart.Invoke();
        _AStarMoveToWaypoints.StopToWaitAtWaypoint();
        yield return new WaitForSeconds(runAwayTime);
        _aStarMovement.timeBetweenMoves = beforeRunAwaySpeed;
        onRunAwayStop.Invoke();
    }

    private void KillSheep()
    {
        StartCoroutine(KillCouroutine());
    }

    IEnumerator KillCouroutine()
    {
        _aStarMovement.StopMovement();
        onDeathStart.Invoke();
        yield return new WaitForSeconds(timeUntilDeath);
        onDeathRemove.Invoke();
        gameObject.SetActive(false);
    }
}
