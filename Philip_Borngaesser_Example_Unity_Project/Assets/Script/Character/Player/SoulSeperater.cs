using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulSeperater : MonoBehaviour
{
    public GameObject soul;
    public bool unlocked;
    public bool isSeperated;
    GridMovementController playerGridMovementController;
    GridMovementController soulGridMovementController;
    PlayerMovement soulPlayerMovementController;
    PlayerBootsHandler playerBootsHandler;
    PlayerBootsHandler soulPlayerBootsHandler;
    PlayerEquipmentController soulEquipmentController;
    private MovementNode toMoveNode;

    [Header("Camera")]
    private CinemachineVirtualCamera mainCineMachineCam;
    private Transform beforeSoulSeperateLookAt;
    public Transform whileSoulSeperateLookAt;
    public float farWayPercentage = 0.5f;
    public float distanceBeforeStartChangeFOV = 3f;
    public float maxDistanceForMaxFOV = 5f;
    private float startFOV = 0f;
    public float maxFOV = 50f;

    void Start()
    {
        playerGridMovementController = GetComponent<GridMovementController>();
        soulGridMovementController = soul.GetComponent<GridMovementController>();
        soulEquipmentController = soul.GetComponent<PlayerEquipmentController>();
        soulPlayerMovementController = soul.GetComponent<PlayerMovement>();

        playerBootsHandler = GetComponent<PlayerBootsHandler>();
        soulPlayerBootsHandler = soul.GetComponent<PlayerBootsHandler>();
        InputManager.instance.inputActions.Keyboard.Separate.performed += ctx => Seperate();

        mainCineMachineCam = MainCameraManager.instance.mainCineMachineCam;
        beforeSoulSeperateLookAt = mainCineMachineCam.Follow;
        startFOV = mainCineMachineCam.m_Lens.FieldOfView;
        OnDeSeperate();
        }

    private void OnDisable()
    {
        if (isSeperated)
        {
            playerGridMovementController.OnGridMovementStart.RemoveListener(OnCalculateNewCameraPosition);
        }
    }

    public void Seperate()
    {
        if (!unlocked) return;
        if (playerGridMovementController.isMoving) return;
        if (soulGridMovementController.isMoving) return;
        if (!isSeperated) OnSeperate();
        else OnDeSeperate();
    }

    private void OnDeSeperate()
    {
        soulEquipmentController.DropCurrentEquipment();
        isSeperated = false;
        soul.SetActive(false);
        mainCineMachineCam.Follow = beforeSoulSeperateLookAt;
        mainCineMachineCam.LookAt = beforeSoulSeperateLookAt;
        mainCineMachineCam.m_Lens.FieldOfView = startFOV;
        soulPlayerMovementController.stopInput = true;

        playerGridMovementController.OnGridMovementStart.RemoveListener(OnCalculateNewCameraPosition);
    }

    private void OnSeperate()
    {
        toMoveNode = playerGridMovementController.currentMovementNode.FindFreeNodeAround();
        if (toMoveNode == null)
        {
            Debug.Log("PlayAnimation");
            return;
        }
        soul.transform.position = transform.position;
        soul.SetActive(true);
        isSeperated = true;
        soulPlayerMovementController.stopInput = false;

        mainCineMachineCam.Follow = whileSoulSeperateLookAt;
        mainCineMachineCam.LookAt = whileSoulSeperateLookAt;
  
        soulPlayerBootsHandler.unlocked = playerBootsHandler.unlocked;

        OnCalculateNewCameraPosition();
        playerGridMovementController.OnGridMovementStart.AddListener(OnCalculateNewCameraPosition);


        Invoke(nameof(MoveSoul), 0.05f);
    }

    void OnCalculateNewCameraPosition()
    {
        Vector3 directionToSoul = soul.transform.position - transform.position;
        float distanceToSoul = new Vector2(directionToSoul.x,directionToSoul.y).magnitude;
        directionToSoul = directionToSoul.normalized;
        if (distanceToSoul > distanceBeforeStartChangeFOV)
        {
            float percentageBetweenMinAndMax = (distanceToSoul - distanceBeforeStartChangeFOV) / (maxDistanceForMaxFOV -distanceBeforeStartChangeFOV);
            if (percentageBetweenMinAndMax > 1) percentageBetweenMinAndMax = 1;
            mainCineMachineCam.m_Lens.FieldOfView = Mathf.Lerp(startFOV, maxFOV, percentageBetweenMinAndMax);
        }

        whileSoulSeperateLookAt.transform.position = transform.position + directionToSoul * (distanceToSoul*farWayPercentage);

    }

    private void MoveSoul()
    {
        soulGridMovementController.MoveToTargetNode(toMoveNode);
    }
}
