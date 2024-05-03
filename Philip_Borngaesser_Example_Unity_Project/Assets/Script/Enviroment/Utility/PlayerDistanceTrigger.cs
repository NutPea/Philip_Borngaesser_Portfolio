using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerDistanceTrigger : MonoBehaviour
{
    public Transform player;
    public bool getPlayerFromOverSceneReceiver = true;
    public float distance = 2.5f;
    private bool distanceTrigger;
    public UnityEvent onInDistance = new UnityEvent();
    public UnityEvent onOutDistance = new UnityEvent();
    void Start()
    {
        if (getPlayerFromOverSceneReceiver)
        {
            player = OverSceneReceiver.instance.player.transform;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(transform.position,player.transform.position) < distance)
        {
            if (!distanceTrigger)
            {
                onInDistance.Invoke();
                distanceTrigger = true;
            }
        }
        else
        {
            if (distanceTrigger)
            {
                onOutDistance.Invoke();
                distanceTrigger = false;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, distance);
    }
}
