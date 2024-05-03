using PixelCrushers.DialogueSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GateController : MonoBehaviour
{
    [VariablePopup]
    [SerializeField]
    private string variableKeyName = string.Empty;

    public List<GridMovementController> gateMovementController;
    public List<SpriteRenderer> fadeoutSprites;
    SaveTriggerEvent _saveTriggerEvent;
    public float fadeOutRemoveTime = 1f;
    public LeanTweenType fadeOutRemoveTweenTyp = LeanTweenType.punch;
    public GameObject keyLog;
    public SpriteRenderer keyLogSprite;
    public float keyUpDistance = 1f;
    public float keyUpTime = 1f;
    public LeanTweenType keyUpTweenTyp = LeanTweenType.punch;
    public float keySpriteRemoveTime = 1f;
    public LeanTweenType keyRemoveTweenTyp = LeanTweenType.punch;

    bool isRemoving;
    public UnityEvent onPlayerHasNoKey = new UnityEvent();
    public UnityEvent onPlayerUnlockLock = new UnityEvent();

    void Start()
    {
        _saveTriggerEvent = GetComponent<SaveTriggerEvent>();
        GetComponentsInChildren<GridMovementController>(gateMovementController);
        StartCoroutine(LateStart());
    }



    IEnumerator LateStart()
    {
        yield return new WaitForSeconds(0.1f);
        foreach(GridMovementController gridMovement in gateMovementController)
        {
            gridMovement.OnOccupiedGridMovementRequest.AddListener(TryToOpen);
            Vector3 pos = gridMovement.transform.position;
            pos.z = 0;
            gridMovement.transform.position = pos;
        }
        
    }

    private void TryToOpen(Transform player)
    {
        bool hasKey = DialogueLua.GetVariable(variableKeyName).asBool;

        if(!hasKey){
            Debug.Log("Miep");
            onPlayerHasNoKey.Invoke();
            return;
        }

        if (isRemoving) return;
        isRemoving = true;
        onPlayerUnlockLock.Invoke();

        _saveTriggerEvent.SaveEvent();
        OnMoveUpLock();
    }


    private void OnMoveUpLock()
    {
        LeanTween.moveLocalY(keyLog, keyUpDistance, keyUpTime).setOnComplete(OnRemoveLock).setEase(keyUpTweenTyp);
    }

    private void OnRemoveLock()
    {
        LeanTween.value(gameObject, 1, 0, keySpriteRemoveTime).setOnUpdate((float val) =>
        {
            Color c = keyLogSprite.color;
            c.a = val;
            keyLogSprite.color = c;
        }).setOnComplete(RemoveGate).setEase(keyRemoveTweenTyp);
    }

    private void RemoveGate()
    {

        if(fadeoutSprites.Count == 0)
        {
            RemoveGridController();
            return;
        }
        foreach(SpriteRenderer renderer in fadeoutSprites)
        {
            LeanTween.value(gameObject, 1, 0, keySpriteRemoveTime).setOnUpdate((float val) =>
            {
                Color c = renderer.color;
                c.a = val;
                renderer.color = c;
            }).setOnComplete(RemoveGridController).setEase(fadeOutRemoveTweenTyp);
        }

    }

    private void RemoveGridController()
    {
        foreach(GridMovementController gridController in gateMovementController)
        {
            gridController.GiveCurrentNodeFree();
        }
    }

    public void _UnlockGateInstantly()
    {
        foreach (GridMovementController gridController in gateMovementController)
        {
            gridController.GiveCurrentNodeFree();
        }
        gameObject.SetActive(false);

    }

}
