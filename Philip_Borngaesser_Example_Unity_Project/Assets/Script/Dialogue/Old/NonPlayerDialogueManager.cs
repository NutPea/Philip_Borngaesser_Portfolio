using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(GridMovementController))]
public class NonPlayerDialogueManager : MonoBehaviour
{
    public DialogueCollection usedDialogue;
    private GridMovementController _gridMovementController;
    private CinemachineVirtualCamera mainCineMachineCam;
    private Transform playerTransform;
    private PlayerMovement playerMovement;

    private DialogueConditionHandler _dialogueConditionHandler;
    private bool _hasCondition;

    [HideInInspector] public UnityEvent<int> onDialogueHasChanged = new UnityEvent<int>();
    [HideInInspector] public UnityEvent<DialogueNodeData> onDialogueDataHasChanged = new UnityEvent<DialogueNodeData>();
    public UnityEvent onNewLetterHasPrinted = new UnityEvent();
    [HideInInspector] public UnityEvent<int> onDialogueHasPrinted = new UnityEvent<int>();
    public UnityEvent<Transform> onShowDialogue = new UnityEvent<Transform>();
    public UnityEvent onCloseDialogue = new UnityEvent();

    [Header("Dialogue")]
    public NPCNames.Name name;
    public int currentDialogueIndex = -1;

    private bool isPrintingText;
    private string printableString;

    private int printableStringLength;
    private int currentPrintableStringIndex;

    private const float SHOW_NEXT_LETTER_TIME = 0.075f;
    private float currentShowNextLetterTime = 0.0f;

    private const float CHANGE_DIALOGUE_TIME = 1f;
    private float currentChangeDialogueTime = 0.0f;

    private float startFOV;
    private const float DIALOGUE_FOV = 30f;

    private const float FAST_SHOW_NEXT_LETTER_TIME = 0.01f;
    private bool showDialogueFast = false;

    [Header("DialogueUI")]
    public GameObject dialogueBackgroundRoot;
    public GameObject nextInputObject;
    public TextMeshProUGUI dialogueText;

    public float scaleTime = 0.75f;
    public LeanTweenType beginnScaleType;
    public LeanTweenType endScaleType;

    private DialogueNodeData currendDialogueNode;

    bool isEndDialogue;
    bool waitsForInput;
    bool showNextDialogueTrigger;
    public bool isPlayer;

    void Start()
    {
        if (!isPlayer)
        {
            _gridMovementController = GetComponent<GridMovementController>();
            _gridMovementController.OnOccupiedGridMovementRequest.AddListener(OnStartDialogue);
        }
        _dialogueConditionHandler = GetComponent<DialogueConditionHandler>();
        _hasCondition = _dialogueConditionHandler != null;


        dialogueText.gameObject.SetActive(false);
        dialogueBackgroundRoot.SetActive(false);
        dialogueBackgroundRoot.transform.localScale = new Vector3(0, 1, 1);
        showNextDialogueTrigger = false;
        isEndDialogue = false;

        InputManager.instance.inputActions.Keyboard.Interact.performed += ctx => DialogueInput();
        nextInputObject.SetActive(false);
    }

    private void DialogueInput()
    {
        if (waitsForInput)
        {
            showNextDialogueTrigger = true;
        }
        else
        {
            showDialogueFast = true;
        }
    }

    public void OnStartDialogue(Transform player)
    {
        if (usedDialogue == null) return;
        if (_hasCondition)
        {
            _dialogueConditionHandler.InitConditions(player);
            usedDialogue = _dialogueConditionHandler.GetDialogueCollection(player);
        }

        currentDialogueIndex = -1;
        if (currentDialogueIndex > usedDialogue.dialogueNodeDatas.Count) return;
        playerTransform = player;
        playerMovement = player.GetComponent<PlayerMovement>();
        playerMovement.SetInputIsBlocked(true);

        mainCineMachineCam = MainCameraManager.instance.mainCineMachineCam;
        mainCineMachineCam.Follow = null;
        mainCineMachineCam.transform.position = new Vector3(transform.position.x,transform.position.y,mainCineMachineCam.transform.position.z);
        startFOV = mainCineMachineCam.m_Lens.FieldOfView;
        mainCineMachineCam.m_Lens.FieldOfView = DIALOGUE_FOV;

        dialogueText.text = "";
        dialogueBackgroundRoot.SetActive(true);
        if (currentDialogueIndex == -1) currendDialogueNode = usedDialogue.startNodeData;
        else currendDialogueNode = usedDialogue.dialogueNodeDatas[currentDialogueIndex];
        onDialogueDataHasChanged.Invoke(currendDialogueNode);

        MainCameraManager.instance.SetCinemachineConfiner(false);
        LeanTween.scaleX(dialogueBackgroundRoot, 1, scaleTime).setEase(beginnScaleType).setOnComplete(OnOpenDialogueEnd);
        onShowDialogue.Invoke(player);
    }

    private void OnOpenDialogueEnd()
    {
        dialogueText.gameObject.SetActive(true);
        ShowDialogue();
    }


    private void Update()
    {


        if (isPrintingText)
        {
            if (currentShowNextLetterTime < 0)
            {
                if (!showDialogueFast)
                {
                    currentShowNextLetterTime = SHOW_NEXT_LETTER_TIME;
                }
                else
                {
                    currentShowNextLetterTime = FAST_SHOW_NEXT_LETTER_TIME;
                }

                currentPrintableStringIndex++;

                if (currentPrintableStringIndex >= printableStringLength)
                {
                    isPrintingText = false;
                    waitsForInput = true;
                    if (currendDialogueNode.HasConnectedNodes())
                    {
                        GenerateDecisions();
                    }
                    else if (currendDialogueNode.HasAutoConnection())
                    {
                        //Next AutoNode
                        nextInputObject.SetActive(true);
                        currendDialogueNode = currendDialogueNode.autoConnectedNode;
                        currentDialogueIndex = usedDialogue.dialogueNodeDatas.IndexOf(currendDialogueNode);
                        onDialogueDataHasChanged.Invoke(currendDialogueNode);
                    }
                    else
                    {
                        nextInputObject.SetActive(true);
                        isEndDialogue = true;
                    }
                    onDialogueHasPrinted.Invoke(currentDialogueIndex);
                }

                string printedString = "";
                for (int index = 0; index < currentPrintableStringIndex; index++)
                {
                    printedString += printableString[index];
                }
                dialogueText.text = printedString;
                onNewLetterHasPrinted.Invoke();

            }
            else
            {
                currentShowNextLetterTime -= Time.deltaTime;
            }
        }

        if (showNextDialogueTrigger)
        {
            ShowNextDialogue();
            showDialogueFast = false;
            showNextDialogueTrigger = false;
            waitsForInput = false;
        }
    }

    private void GenerateDecisions()
    {
       
    }

    private void ShowNextDialogue()
    {
        nextInputObject.SetActive(false);
        if (isEndDialogue)
        {
            CloseDialoge();
        }
        else
        {
            ShowDialogue();
        }
    }

    private void ShowDialogue()
    {
        isPrintingText = true;
        printableString = currendDialogueNode.dialogueText;

        currentShowNextLetterTime = SHOW_NEXT_LETTER_TIME;

        printableStringLength = printableString.Length;
        currentPrintableStringIndex = 0;
    }

    void CloseDialoge()
    {
        dialogueText.gameObject.SetActive(false);
        LeanTween.scaleX(dialogueBackgroundRoot, 0, scaleTime).setEase(endScaleType).setOnComplete(CloseDialogeEnd);
        onCloseDialogue.Invoke();
    }

    void CloseDialogeEnd()
    {
        dialogueBackgroundRoot.SetActive(false);
        mainCineMachineCam.Follow = playerTransform;
        mainCineMachineCam.m_Lens.FieldOfView = startFOV;
        MainCameraManager.instance.SetCinemachineConfiner(true);
        playerMovement.SetInputIsBlocked(false);
        isEndDialogue = false;
    }

    public void SetDialogue(DialogueCollection dialogue)
    {
        usedDialogue = dialogue;
    }

}
