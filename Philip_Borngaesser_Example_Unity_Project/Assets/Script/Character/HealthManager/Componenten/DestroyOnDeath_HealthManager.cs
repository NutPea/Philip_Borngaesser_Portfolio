using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(HealthManager))]
public class DestroyOnDeath_HealthManager : MonoBehaviour
{
    HealthManager _healthManager;
    public float timer = 2f;

    AStarMoveToMovementNode AStarMoveToMovementNode;
    AStarChaseController aStarChaseController;
    public UnityEvent onDeathStart = new UnityEvent();
    public UnityEvent onDeathRemove = new UnityEvent();
    void Start()
    {
        _healthManager = GetComponent<HealthManager>();
        _healthManager.OnDeath.AddListener(Destroing);
        AStarMoveToMovementNode = GetComponent<AStarMoveToMovementNode>();
        aStarChaseController = GetComponent<AStarChaseController>();
    }

    private void Destroing()
    {
        StartCoroutine(DestroyingCouroutine());
    }

    IEnumerator DestroyingCouroutine()
    {
        onDeathStart.Invoke();
        if (AStarMoveToMovementNode != null) AStarMoveToMovementNode.StopMovement();
        yield return new WaitForSeconds(timer);
        onDeathRemove.Invoke();
        gameObject.SetActive(false);
    }
}
