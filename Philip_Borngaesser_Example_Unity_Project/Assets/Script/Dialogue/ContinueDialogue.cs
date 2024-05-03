using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PixelCrushers.DialogueSystem;


public class ContinueDialogue : MonoBehaviour
{

    private void Start()
    {
        InputManager.instance.inputActions.Keyboard.Continue.performed += ctx => OnContinueDialogue();
    }

    public void OnContinueDialogue()
    {
        if (DialogueManager.instance.currentUsedContinueButton != null)
        {
            DialogueManager.instance.currentUsedContinueButton.OnFastForward();
        }
    }
}

