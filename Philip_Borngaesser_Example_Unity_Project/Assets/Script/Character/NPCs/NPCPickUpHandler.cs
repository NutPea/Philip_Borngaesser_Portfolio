using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NPCPickUpHandler : MonoBehaviour
{
    public Transform pickUpPosition;
    private PickUpSaver pickUpSaver;
    private GridMovementController gridMovementController;
    [HideInInspector] public bool dontSavePickUp;

    public UnityEvent onPickUp = new UnityEvent();
    public UnityEvent onDrop = new UnityEvent();

    private void Awake()
    {
        pickUpSaver= GetComponent<PickUpSaver>();
        if(pickUpSaver == null ) dontSavePickUp = true;
        gridMovementController = GetComponent<GridMovementController>();
    }

    void Start()
    {
        if (pickUpPosition == null) pickUpPosition = transform;
    }

    public void OnPickUp()
    {
        MonoBehaviour[] toDisableComponents = GetComponents<MonoBehaviour>();
        foreach (MonoBehaviour component in toDisableComponents)
        {
            if (component.GetType() == typeof(NPCPickUpHandler)) continue;
            if (component.GetType() == typeof(Transform)) continue;
            if (component.GetType() == typeof(GridMovementController)) continue;
            component.enabled = false;
        }
        onPickUp.Invoke();
    }

    public void OnDrop()
    {
        MonoBehaviour[] toDisableComponents = GetComponents<MonoBehaviour>();
        foreach (MonoBehaviour component in toDisableComponents)
        {
            if (component.GetType() == typeof(NPCPickUpHandler)) continue;
            if (component.GetType() == typeof(Transform)) continue;
            if (component.GetType() == typeof(GridMovementController)) continue;
            component.enabled = true;
        }
        if(!dontSavePickUp)
        {
            if (gridMovementController == null) pickUpSaver.SavePickUpPosition();
            else gridMovementController.OnGridMovementEnd.AddListener(AfterMovementSave);
        }
        onDrop.Invoke();    

    }

    public void AfterMovementSave()
    {
        pickUpSaver.SavePickUpPosition();
        gridMovementController.OnGridMovementStart.RemoveListener(AfterMovementSave);
    }
}
