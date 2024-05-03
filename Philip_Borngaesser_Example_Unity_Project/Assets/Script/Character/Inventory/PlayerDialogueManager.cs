using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class PlayerDialogueManager : MonoBehaviour
{
    public GameObject dialogueCanvas;
    public TextMeshProUGUI dialogueText;
    public float timeAfterDialogueGotFullShown;

    private const float SHOW_NEXT_LETTER_TIME = 0.1f;
    private float currentShowNextLetterTime = 0.0f;
    private bool isPrintingText;
    private int currentPrintableStringIndex;
    private int printableStringLength;
    private int currentDialogueIndex;

    private string printableString;

    [HideInInspector] public UnityEvent<int> onDialogueHasChanged = new UnityEvent<int>();
    public UnityEvent onNewLetterHasPrinted = new UnityEvent();
    [HideInInspector] public UnityEvent<int> onDialogueHasPrinted = new UnityEvent<int>();
    public UnityEvent onShowDialogue = new UnityEvent();
    [HideInInspector] public UnityEvent onCloseDialogue = new UnityEvent();
    public float scaleTime = 1;

    public LeanTweenType beginnScaleType;
    public LeanTweenType endScaleType;

    public string toPrintString;


    void Update()
    {
        if (isPrintingText)
        {
            if (currentShowNextLetterTime < 0)
            {
                currentShowNextLetterTime = SHOW_NEXT_LETTER_TIME;
                currentPrintableStringIndex++;

                if (currentPrintableStringIndex >= printableStringLength)
                {
                    StartCoroutine(CloseDialogueCouroutine());
                    isPrintingText = false;
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
    }

    public void OpenDialogue()
    {

        dialogueText.text = "";
        dialogueCanvas.SetActive(true);
        LeanTween.scaleX(dialogueCanvas, 1, scaleTime).setEase(beginnScaleType).setOnComplete(OnOpenDialogueEnd);
        onShowDialogue.Invoke();
    }

    void OnOpenDialogueEnd()
    {
        dialogueText.gameObject.SetActive(true);
        isPrintingText = true;
        printableString = toPrintString;
        currentShowNextLetterTime = SHOW_NEXT_LETTER_TIME;
        printableStringLength = printableString.Length;
        currentPrintableStringIndex = 0;

    }


    IEnumerator CloseDialogueCouroutine()
    {
        yield return new WaitForSeconds(timeAfterDialogueGotFullShown);
        CloseDialoge();
    }

    void CloseDialoge()
    {
        dialogueText.gameObject.SetActive(false);
        LeanTween.scaleX(dialogueCanvas, 0, scaleTime).setEase(endScaleType).setOnComplete(CloseDialogeEnd);
        onCloseDialogue.Invoke();
    }

    void CloseDialogeEnd()
    {
        dialogueCanvas.SetActive(false);
    }
}
