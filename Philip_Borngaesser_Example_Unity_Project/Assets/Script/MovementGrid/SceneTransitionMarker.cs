using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionMarker : MonoBehaviour ,INodeExtension
{

    public SceneNames.Name scene;
    public Vector2 gridPosition;
    public bool isNoPlayScene;
    public MovementNode node;

    public void Start()
    {
        node = MovementGrid.instance.WorldToMovementNode(transform.position);
        AddExtension(node);
        transform.position = node.transform.position;
    }


    public void AddExtension(MovementNode targetNode)
    {
        targetNode.onArrivedAtNode.AddListener(ChangeScene);
    }

    public void ChangeScene(bool isPlayer)
    {
        if (!isPlayer) return;
        if (!CheckIfPlayerCanMove())
        {
            GridMovementController playerGridMovementController = OverSceneReceiver.instance.player.GetComponent<GridMovementController>();
            playerGridMovementController.OnGridMovementEnd.AddListener(MoveBack);

            return;
        }


        if (!isNoPlayScene)
        {
            PositionChangeMessanger positionChangeMessanger = OverSceneMessanger.instance.gameObject.AddComponent<PositionChangeMessanger>();
            positionChangeMessanger.gridPos = gridPosition;
            OverSceneMessanger.instance.AddMessage(positionChangeMessanger);

            TransitionMassanger transitionMassanger = OverSceneMessanger.instance.gameObject.AddComponent<TransitionMassanger>();
            OverSceneMessanger.instance.AddMessage(transitionMassanger);

            GameObject player = OverSceneReceiver.instance.player;
            //Equipment
            PlayerEquipmentController playerEquipmentController = player.GetComponent<PlayerEquipmentController>();
            if (playerEquipmentController.currentItemEnum != Equipment.Item.None)
            {
                OverSceneEquipmentMessanger overSceneEquipmentMessanger = OverSceneMessanger.instance.gameObject.AddComponent<OverSceneEquipmentMessanger>();
                overSceneEquipmentMessanger.overSceneEquipment = playerEquipmentController.currentItemEnum;
                OverSceneMessanger.instance.AddMessage(overSceneEquipmentMessanger);
            }

            //Steps
            PlayerStepHandler playerStepHandler = player.GetComponent<PlayerStepHandler>();
            StepCountMessanger stepCountMessanger = OverSceneMessanger.instance.gameObject.AddComponent<StepCountMessanger>();
            stepCountMessanger.lastStepAmount = playerStepHandler.currentStepAmount;
            OverSceneMessanger.instance.AddMessage(stepCountMessanger);

            //Overscene
            PlayerPickUpHandler playerPickUpHandler = player.GetComponent<PlayerPickUpHandler>();
            if (playerPickUpHandler.hasPickedUpSomething)
            {
                if(playerPickUpHandler.pickUpHandler.dontSavePickUp)
                {
                    PickUpMessanger pickUpMessanger = OverSceneMessanger.instance.gameObject.AddComponent<PickUpMessanger>();
                    pickUpMessanger.PickUpPreparation(playerPickUpHandler.pickUpHandler);
                    OverSceneMessanger.instance.AddMessage(pickUpMessanger);
                }
            }

            SceneNames.currentSceneName = scene;

        }

        CustomSceneLoader.instance.LoadScene(SceneNames.GetSceneName(scene));
    }

    private void MoveBack()
    {
        GridMovementController playerGridMovementController = OverSceneReceiver.instance.player.GetComponent<GridMovementController>();
        playerGridMovementController.MoveToTargetNode(playerGridMovementController.beforeCurrentMovementNode);
        playerGridMovementController.OnGridMovementEnd.RemoveListener(MoveBack);
    }

    private bool CheckIfPlayerCanMove()
    {
        Transform playerTransform = OverSceneReceiver.instance.player.transform;
        PlayerPickUpHandler playerPickUpHandler = playerTransform.GetComponent<PlayerPickUpHandler>();
        if(playerPickUpHandler.hasPickedUpSomething){
            if (playerPickUpHandler.pickUpHandler.dontSavePickUp){
                return false;
            }
        }
        return true;

    }
}
