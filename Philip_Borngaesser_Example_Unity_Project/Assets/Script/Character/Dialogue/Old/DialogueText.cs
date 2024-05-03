using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DialogueText", fileName = "DialogueText")]
public class DialogueText : ScriptableObject
{
    public string dialogue;
    public bool autoShowNextDialogue = false;
    public bool jumpsToTargetDialogueText;
    public DialogueText jumpDialogueText;
}
