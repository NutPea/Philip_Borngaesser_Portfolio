using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class DialogueConditionSaver : MonoBehaviour
{
    [HideInInspector] public bool wasSaved = false;
    public string GUID;
    public bool GUIDGotSet;
    [HideInInspector] public bool firstTimeInSceneTrigger;

    public DialogueConditionHandler dialogueConditionHandler;

    
    
    IEnumerator Start()
    {
        dialogueConditionHandler = GetComponent<DialogueConditionHandler>();
        if (string.IsNullOrEmpty(GUID))
        {
            string positionString = "||| " + transform.position.x.ToString("F4") + "---" + transform.position.y.ToString("F4") + "---" + transform.position.z.ToString("F4") + " - " + SceneManager.GetActiveScene().buildIndex;
            GUID = gameObject.name + positionString;
        }
        yield return new WaitForEndOfFrame();
        if (SaveStateManager.instance.CheckDialogeConditionSaves(this))
        {

            LoadCondition();
        }
    }

    public void SaveConditions()
    {
        SaveStateManager.instance.SaveDialogueCondition(this);
    }

    private void LoadCondition()
    {
        List<bool> loadedConditions = SaveStateManager.instance.GetSavedConditions(this);
        if(loadedConditions != null)
        {
            for(int index = 0; index < dialogueConditionHandler.conditions.Count; index++)
            {
                dialogueConditionHandler.conditions[index].conditionHasBeenMet = loadedConditions[index];
            }
        }
    }

    public void SetGUIDEditor()
    {
        if (Application.isPlaying) return;
        GUID = System.Guid.NewGuid().ToString();
    }

}
