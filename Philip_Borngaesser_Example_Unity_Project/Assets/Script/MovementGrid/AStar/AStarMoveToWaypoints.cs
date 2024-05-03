using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AStarMoveToMovementNode))]
public class AStarMoveToWaypoints : MonoBehaviour
{
    public List<Transform> wayPoints;
    List<MovementNode> _wayPointNode = new List<MovementNode>();
    int currentIndex;
    public float waitTimeBetweenWaypoints = 0.0f;
    AStarMoveToMovementNode _aStarMoveToMovementNode;
    public bool movesBetweensPointsOnStart = false;

    public UnityEvent onArrivedAtWaypoint = new UnityEvent();
    bool waitsAtWaitingPoint;

    IEnumerator Start()
    {
        _aStarMoveToMovementNode = GetComponent<AStarMoveToMovementNode>();
        _aStarMoveToMovementNode.OnReachWaypoint.AddListener(MoveToNextWaypoint);

        for(int index = 0; index < wayPoints.Count; index++)
        {
            _wayPointNode.Add(MovementGrid.instance.WorldToMovementNode(wayPoints[index].position));
        }

        yield return new WaitForEndOfFrame();
        if (movesBetweensPointsOnStart) MoveToNextWaypointImmidiate();
    }

   public void MoveToNextWaypoint()
   {
        StartCoroutine(MoveToNextWaypointRoutine());
   }


    IEnumerator MoveToNextWaypointRoutine()
    {
        onArrivedAtWaypoint.Invoke();
        waitsAtWaitingPoint = true;
        yield return new WaitForSeconds(waitTimeBetweenWaypoints);
        MoveToNextWaypointImmidiate();
        waitsAtWaitingPoint = false;

    }

    
    public void StopToWaitAtWaypoint()
    {
        if (!waitsAtWaitingPoint) return;
        StopCoroutine(MoveToNextWaypointRoutine());
        MoveToNextWaypointImmidiate();
    }

    private void MoveToNextWaypointImmidiate()
    {
        _aStarMoveToMovementNode.MoveToNode(_wayPointNode[currentIndex]);
        currentIndex++;
        if (currentIndex >= _wayPointNode.Count)
        {
            currentIndex = 0;
        }
    }

    public void StopMoveingToWaypoints()
    {
        _aStarMoveToMovementNode.OnReachWaypoint.RemoveListener(MoveToNextWaypoint);
    }
}
