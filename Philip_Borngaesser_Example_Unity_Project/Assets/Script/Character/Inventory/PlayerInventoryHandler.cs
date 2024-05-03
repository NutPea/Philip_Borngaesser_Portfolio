using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInventoryHandler : MonoBehaviour
{

    public List<QuestItemSaveState> questItemSaveStates = new List<QuestItemSaveState>();
    public UnityEvent<QuestItem.Item> onQuestItemGotPickedUp = new UnityEvent<QuestItem.Item>();
    public UnityEvent<QuestItem.Item> onQuestItemWasRemoved = new UnityEvent<QuestItem.Item>();

    IEnumerator  Start()
    {
        yield return new WaitForEndOfFrame();
        LoadQuestItemInventory();
    }
    private void LoadQuestItemInventory()
    {
        foreach(QuestItemSaveState questItemSaveState in SaveStateManager.instance.questItemSaveStates)
        {
            if (!questItemSaveState.hasBeenUsed)
            {
                questItemSaveStates.Add(questItemSaveState);
                onQuestItemGotPickedUp.Invoke(questItemSaveState.item);
            }
        }
    }

    public void PickUpItem(QuestItem.Item item)
    {
        QuestItemSaveState questItemSaveState = new QuestItemSaveState();
        questItemSaveState.item = item;
        questItemSaveStates.Add(questItemSaveState);
        onQuestItemGotPickedUp.Invoke(item);
        SaveQuestItem(item);
    }

    public bool HasItemInInventory(QuestItem.Item item)
    {
        bool hasFound = false;
        foreach (QuestItemSaveState questItemSaveState in questItemSaveStates)
        {
            if (questItemSaveState.item == item)
            {
                hasFound = true;
                break;
            }
        }
        return hasFound;
    }


    public QuestItemSaveState GetQuestItemSaveState(QuestItem.Item item)
    {
        QuestItemSaveState foundQuestItemSaveState = null;
        foreach (QuestItemSaveState questItemSaveState in questItemSaveStates)
        {
            if(questItemSaveState.item == item)
            {
                questItemSaveStates.Remove(questItemSaveState);
                foundQuestItemSaveState = questItemSaveState;
                break;
            }
        }
        return foundQuestItemSaveState;
    }

    public void RemoveItem(QuestItem.Item item)
    {
        QuestItemSaveState removableQuestItemSaveState = GetQuestItemSaveState(item);
        if (removableQuestItemSaveState == null) return;
        questItemSaveStates.Remove(removableQuestItemSaveState);
        onQuestItemWasRemoved.Invoke(item);
        SaveStateManager.instance.RemoveQuestItem(item);
    }

    public void SaveQuestItem(QuestItem.Item item)
    {
        SaveStateManager.instance.SaveQuestItem(item);
    }


}
