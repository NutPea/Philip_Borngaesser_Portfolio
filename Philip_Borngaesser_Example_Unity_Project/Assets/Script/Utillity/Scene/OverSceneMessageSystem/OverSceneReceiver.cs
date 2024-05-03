using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OverSceneReceiver : MonoBehaviour
{
    public static OverSceneReceiver instance;
    public GameObject player;
    public TransitionHandler transitionHandler;
    public EquipmentHolder equipmentHolder;

    [HideInInspector] public UnityEvent<Equipment.Item> overSceneEquipmentHasEquiped = new UnityEvent<Equipment.Item>();

    private void Awake()
    {
        instance = this;
    }

    IEnumerator Start()
    {
        OverSceneMessanger.instance.UseMessagesAwake(this);
        yield return new WaitForEndOfFrame();
        OverSceneMessanger.instance.UseMessagesStart(this);
        yield return new WaitForEndOfFrame();
        OverSceneMessanger.instance.UseMessagesLateStart(this);
    }



}
