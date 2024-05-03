using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GridMovementController))]
public class PositionSaver : MonoBehaviour
{
    public string GUID;
    public bool GUIDGotSet;

    private GridMovementController _gridMovementController;
    public Vector2 gridPosition;
    public SceneNames.Name savedSceneName = SceneNames.Name.MAINMENU;
    bool hasSavedPosition;

    private void Awake()
    {
        _gridMovementController = GetComponent<GridMovementController>();
        _gridMovementController.OnGridMovementEnd.AddListener(OnPositionHasChanged);
    }
   
    private void OnPositionHasChanged()
    {
        gridPosition.x = _gridMovementController.currentMovementNode.gridX;
        gridPosition.y = _gridMovementController.currentMovementNode.gridY;

        SaveStateManager.instance.SaveGridPosition(this);
    }

    public void SetSavedState(PositionSaveState positionSaveState)
    {
        this.gridPosition = positionSaveState.gridPosition;
        this.savedSceneName = positionSaveState.scene;

        hasSavedPosition = true;
    }

    void Start()
    {
        hasSavedPosition = SaveStateManager.instance.GetSavedGridPosition(this);
        Invoke(nameof(SetPositionWhenSaved), 0.1f);
    }

    private void SetPositionWhenSaved()
    {
        if (!hasSavedPosition) return;
        if (savedSceneName != SceneNames.currentSceneName) return;

        MovementNode movementNode = MovementGrid.instance.GetMovementNode(gridPosition);
        _gridMovementController.MoveToTargetNodeImmediately(movementNode);
    }

    public void SetGUID()
    {
        if (Application.isPlaying) return;
        GUID = System.Guid.NewGuid().ToString();
    }


}
