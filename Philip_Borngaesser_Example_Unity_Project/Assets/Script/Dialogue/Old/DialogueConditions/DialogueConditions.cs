using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DialogueConditions : ScriptableObject
{
    public abstract void OnStart(DialogueConditionHandler sceneReferenz , Transform player);

    // True so the condition is met.
    public abstract bool OnCondition(DialogueConditionHandler dialogeNPC , Transform player);

    public DialogueConditions MakeCopy()
    {
        return ScriptableObject.Instantiate(this);
    }
}
