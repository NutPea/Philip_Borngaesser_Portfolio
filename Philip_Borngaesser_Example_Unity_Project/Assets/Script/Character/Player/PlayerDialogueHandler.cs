using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDialogueHandler : MonoBehaviour
{
    public DialogueCollection currentDialogue;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenDialoge(DialogueCollection dialogue)
    {
        currentDialogue = dialogue;
    }
}
