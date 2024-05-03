using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "MenuQuestItem", menuName = "MenuQuestItem")]
public class MenuQuestItem : ScriptableObject
{

    public Sprite itemSprite;
    [VariablePopup]
    public string variableName;
    public string itemName;
    [TextArea]
    public string itemDiscription;
}
