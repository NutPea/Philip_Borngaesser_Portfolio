using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDialogueTrigger : MonoBehaviour
{
    public string toShowDialogue;
    public bool canNotShowDialogue;

    public void HasShownDialogue()
    {
        canNotShowDialogue = false;
    }

    private void OnTriggerEnter(Collider other)
    {

        if (canNotShowDialogue) return;
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerDialogueManager playerDialogueManager = other.GetComponent<PlayerDialogueManager>();
            playerDialogueManager.toPrintString = toShowDialogue;
            playerDialogueManager.OpenDialogue();
        }
    }
}
