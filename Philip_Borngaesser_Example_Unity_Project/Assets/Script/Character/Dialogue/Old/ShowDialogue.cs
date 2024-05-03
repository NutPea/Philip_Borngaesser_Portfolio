using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(GridMovementController))]
public class ShowDialogue : MonoBehaviour
{
    public GameObject dialogueBackgroundRoot;
    public TextMeshProUGUI dialogueText;
    private GridMovementController _gridMovementController;
    public bool dialogueIsOpen;
    private bool dialogueOpenCloseTrigger;

    public float scaleTime = 1;
    public LeanTweenType beginnScaleType;
    public LeanTweenType endScaleType;

    [HideInInspector] public Transform _player;

    private bool isPrintingText;
    private string printableString;

    private int printableStringLength;
    private int currentPrintableStringIndex;

    private const float SHOW_NEXT_LETTER_TIME = 0.1f;
    private float currentShowNextLetterTime = 0.0f;

    private const float CHANGE_DIALOGUE_TIME = 1f;
    private float currentChangeDialogueTime = 0.0f;
    private bool _changesDialogue;

    int jumpDialogueIndex = 0;
    bool jumpsToDialogue;

    [HideInInspector] public UnityEvent<int> onDialogueHasChanged = new UnityEvent<int>();
    public UnityEvent onNewLetterHasPrinted = new UnityEvent();
    [HideInInspector] public UnityEvent<int> onDialogueHasPrinted = new UnityEvent<int>();
    public UnityEvent onShowDialogue = new UnityEvent();
    [HideInInspector] public UnityEvent onCloseDialogue = new UnityEvent();

    [Header("Dialogue")]
    public NPCNames.Name name;
    public int currentDialogueIndex;
    public List<DialogueText> dialogues;


    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;

        ChangeDialogue(SaveStateManager.instance.GetDialogueIndex(name));
        _gridMovementController = GetComponent<GridMovementController>();
        dialogueText.gameObject.SetActive(false);
        dialogueBackgroundRoot.SetActive(false);
        dialogueBackgroundRoot.transform.localScale = new Vector3(0, 1, 1);
        _gridMovementController.OnOccupiedGridMovementRequest.AddListener(OnOpenDialogue);

    }

    private void OnOpenDialogue(Transform player)
    {
        OpenDialogue();
    }


    private void Update()
    {
      

        if (isPrintingText)
        {
            if(currentShowNextLetterTime < 0)
            {
                currentShowNextLetterTime = SHOW_NEXT_LETTER_TIME;
                currentPrintableStringIndex++;

                if(currentPrintableStringIndex >= printableStringLength)
                {
                    isPrintingText = false;
                    onDialogueHasPrinted.Invoke(currentDialogueIndex);
                    if (dialogues[currentDialogueIndex].autoShowNextDialogue)
                    {
                        _changesDialogue = true;
                        currentChangeDialogueTime = CHANGE_DIALOGUE_TIME;
                    }

                    if (dialogues[currentDialogueIndex].jumpsToTargetDialogueText)
                    {
                        jumpDialogueIndex = dialogues.IndexOf(dialogues[currentDialogueIndex].jumpDialogueText);
                    }
                }

                string printedString = "";
                for(int index = 0; index < currentPrintableStringIndex; index++)
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


        if (_changesDialogue)
        {
            if(currentChangeDialogueTime < 0)
            {
                int nextDialogeIndex = currentDialogueIndex + 1;
                if (jumpsToDialogue)
                {
                    ChangeDialogue(jumpDialogueIndex);
                    jumpsToDialogue = false;
                }
                else
                {
                    ChangeDialogue(nextDialogeIndex);
                }

                if (currentDialogueIndex > dialogues.Count) Debug.Log("Something went wrong");
                ShowCurrentDialogue();
                _changesDialogue = false;
            }
            else
            {
                currentChangeDialogueTime -= Time.deltaTime;
            }
        }

    }
    void OpenDialogue()
    {
        if (dialogueIsOpen || dialogueOpenCloseTrigger) return;
        if (currentDialogueIndex > dialogues.Count) return;
        dialogueText.text = "";
        dialogueBackgroundRoot.SetActive(true);
        LeanTween.scaleX(dialogueBackgroundRoot, 1, scaleTime).setEase(beginnScaleType).setOnComplete(OnOpenDialogueEnd);
        dialogueOpenCloseTrigger = true;
        onShowDialogue.Invoke();
    }

    void OnOpenDialogueEnd()
    {
        dialogueText.gameObject.SetActive(true);
        ShowCurrentDialogue();

        dialogueIsOpen = true;
        dialogueOpenCloseTrigger = false;
    }

    private void ShowCurrentDialogue()
    {
        isPrintingText = true;
        printableString = dialogues[currentDialogueIndex].dialogue;
        currentShowNextLetterTime = SHOW_NEXT_LETTER_TIME;

        printableStringLength = printableString.Length;
        currentPrintableStringIndex = 0;
    }

    void CloseDialoge()
    {
        if (!dialogueIsOpen || dialogueOpenCloseTrigger) return;
        dialogueText.gameObject.SetActive(false);
        LeanTween.scaleX(dialogueBackgroundRoot, 0, scaleTime).setEase(endScaleType).setOnComplete(CloseDialogeEnd);
        dialogueOpenCloseTrigger = true;
        onCloseDialogue.Invoke();
    }

    void CloseDialogeEnd()
    {
        dialogueBackgroundRoot.SetActive(false);

        dialogueIsOpen = false;
        dialogueOpenCloseTrigger = false;
    }

    public void ChangeDialogue(int dialogueIndex)
    {
        currentDialogueIndex = dialogueIndex;
        onDialogueHasChanged.Invoke(dialogueIndex);
        SaveStateManager.instance.SetDialogueIndex(name, dialogueIndex);
    }

    public void PrintNextDialogue()
    {
        _changesDialogue = true;
        currentChangeDialogueTime = CHANGE_DIALOGUE_TIME;
    }

    public void JumpToDialogue(int dialogueIndex)
    {
        jumpDialogueIndex = dialogueIndex;
        jumpsToDialogue = true;
    }

}
