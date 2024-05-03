using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DialogueConditionSaver))]
public class DialogueConditionHandler : MonoBehaviour
{
    public List<Conditions> conditions;
    public DialogueCollection defaultCollection;
    private DialogueConditionSaver saver;
    private bool hasBeenInit;

    private void Start()
    {
        saver = GetComponent<DialogueConditionSaver>();
    }

    public void InitConditions(Transform player)
    {
        if (hasBeenInit) return;
        foreach (Conditions dialogueConditions in conditions)
        {
            dialogueConditions.InitConditions(this, player);
        }
        hasBeenInit = true;
    }
    public DialogueCollection GetDialogueCollection(Transform player)
    {
        foreach(Conditions condition in conditions)
        {
            if(condition.dontUseConditionAgain && condition.conditionHasBeenMet)
            {
                continue;
            }

            bool conditionHasBeenMet = true;
            foreach(DialogueConditions dialogueConditions in condition.conditions)
            {
                if (condition.isAnd)
                {
                    conditionHasBeenMet = true;
                    if (!dialogueConditions.OnCondition(this, player))
                    {
                        conditionHasBeenMet = false;
                        break;
                    }
                }

                if (condition.isOr)
                {
                    conditionHasBeenMet = false;
                    if (dialogueConditions.OnCondition(this, player))
                    {
                        conditionHasBeenMet = true;
                        break;
                    }
                }
            }

            if (conditionHasBeenMet)
            {
                condition.conditionHasBeenMet = true;
                saver.SaveConditions();
                return condition.dialogueCollection;
            }
        }

        return defaultCollection;
    }


    
}

[System.Serializable]
public class Conditions{
    public bool isAnd = true;
    public bool isOr = false;

    public DialogueCollection dialogueCollection;
    public List<DialogueConditions> conditions;

    public bool dontUseConditionAgain;
    public bool conditionHasBeenMet;



    public void InitConditions(DialogueConditionHandler sceneReferenz, Transform player)
    {
        for(int index = 0; index < conditions.Count; index++)
        {
            DialogueConditions copyCondition = conditions[index].MakeCopy();
            conditions[index] = copyCondition;
        }

        foreach(DialogueConditions condition in conditions)
        {
            condition.OnStart(sceneReferenz, player);
        }
    }
}
