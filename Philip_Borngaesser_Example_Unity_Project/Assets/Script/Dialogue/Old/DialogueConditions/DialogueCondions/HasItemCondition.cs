using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = " HasItemCondition", menuName = "Dialogue/Condition/HasItemCondition", order = 1)]
public class HasItemCondition : DialogueConditions
{
    public QuestItem.Item item;
    private PlayerInventoryHandler inventoryHandler;
    public override void OnStart(DialogueConditionHandler sceneReferenz, Transform player)
    {
        inventoryHandler = player.GetComponent<PlayerInventoryHandler>();
    }
    public override bool OnCondition(DialogueConditionHandler dialogeNPC, Transform player)
    {
        bool hasItem = false;
        if (inventoryHandler.HasItemInInventory(item))
        {
            hasItem = true;
            inventoryHandler.RemoveItem(item);
        }
        return hasItem;
    }

}
