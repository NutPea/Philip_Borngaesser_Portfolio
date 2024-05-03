using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NonPlayerDialogueManager))]
public class DialogueRotateTowardsPlayer : MonoBehaviour
{
    NonPlayerDialogueManager _nonPlayerDialogueManager;
    public GameObject spriteRoot;
    void Start()
    {
        _nonPlayerDialogueManager = GetComponent<NonPlayerDialogueManager>();
        _nonPlayerDialogueManager.onShowDialogue.AddListener(RotateTowardsPlayer);
    }

    private void RotateTowardsPlayer(Transform player)
    {
        Vector3 dir = player.transform.position - transform.position;
        if(dir.x < 0)
        {
            Vector3 spriteScale = spriteRoot.transform.localScale;
            spriteScale.x *= -1;
            spriteRoot.transform.localScale = spriteScale;
        }
        else
        {
            Vector3 spriteScale = spriteRoot.transform.localScale;
            spriteScale.x = Math.Abs(spriteRoot.transform.localScale.x);
            spriteRoot.transform.localScale = spriteScale;
        }
    }

}
