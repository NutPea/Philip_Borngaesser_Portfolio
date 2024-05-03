using Cinemachine;
using PixelCrushers.DialogueSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(DialogueSystemEvents))]
public class SetupDialogueHandler : MonoBehaviour
{
    DialogueSystemEvents dialogueSystemEvents;


    private PlayerMovement playerMovement;

    [Header("DialogueCam")]
    public Transform dialogueCamPosition;
    private CinemachineVirtualCamera mainCineMachineCam;
    private float startFOV;
    private const float DIALOGUE_FOV = 30f;

    public UnityEvent onDialogueStart = new UnityEvent();
    public UnityEvent onDialogueEnd = new UnityEvent();

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        dialogueSystemEvents = GetComponent<DialogueSystemEvents>();
    }

    void Start()
    {
        dialogueSystemEvents.conversationEvents.onConversationStart.AddListener(OnChangeToDialogue);
        dialogueSystemEvents.conversationEvents.onConversationEnd.AddListener(OnChangeBackToGame);
    }

    private void OnChangeBackToGame(Transform actor)
    {
        onDialogueEnd.Invoke();
        //Player
        playerMovement.SetInputIsBlocked(false);
        //Camera
        mainCineMachineCam.Follow = transform;
        mainCineMachineCam.m_Lens.FieldOfView = startFOV;
        MainCameraManager.instance.SetCinemachineConfiner(true);
    }

    private void OnChangeToDialogue(Transform actor)
    {
        Debug.Log("Miep");
        onDialogueStart.Invoke();
        //Player
        playerMovement.SetInputIsBlocked(true);
       //Camera
        mainCineMachineCam = MainCameraManager.instance.mainCineMachineCam;
        mainCineMachineCam.Follow = dialogueCamPosition;
        startFOV = mainCineMachineCam.m_Lens.FieldOfView;
        mainCineMachineCam.m_Lens.FieldOfView = DIALOGUE_FOV;
    }

    private Vector3 GenerateDialoguePosition()
    {
        return dialogueCamPosition.position;
    }
}
