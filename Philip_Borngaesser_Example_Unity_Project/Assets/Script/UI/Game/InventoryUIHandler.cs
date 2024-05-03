using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUIHandler : MonoBehaviour
{
    public GameObject QuestItemUIPrefab;
    public QuestItemSpriteData questItemSpriteData;
    private PlayerInventoryHandler inventoryHandler;
    private List<QuestItemUI> currentlyUsedQuestItemUI = new List<QuestItemUI>();

    void Start()
    {
        inventoryHandler = OverSceneReceiver.instance.player.GetComponent<PlayerInventoryHandler>();
        inventoryHandler.onQuestItemGotPickedUp.AddListener(UpdateQuestItemUI);
        inventoryHandler.onQuestItemWasRemoved.AddListener(RemoveQuestItemUI);
    }

    private void UpdateQuestItemUI(QuestItem.Item item)
    {
        GameObject spawnedQuestItemUIPrefab = Instantiate(QuestItemUIPrefab, transform.position, Quaternion.identity);
        spawnedQuestItemUIPrefab.transform.parent = this.transform;

        QuestItemUI questItemUI = new QuestItemUI();
        questItemUI.questItemUIGameobject = spawnedQuestItemUIPrefab;
        questItemUI.Item = item;
        currentlyUsedQuestItemUI.Add(questItemUI);

        Image quesstItemUI = spawnedQuestItemUIPrefab.GetComponentInChildren<Image>();
        quesstItemUI.sprite = questItemSpriteData.GetSpriteForQuestItemUI(item);

    }

    private QuestItemUI GetQuestItemUIGameobject(QuestItem.Item item)
    {
        if (currentlyUsedQuestItemUI.Count <= 0) return null;
        QuestItemUI questItemUI = null;
        foreach(QuestItemUI ui in currentlyUsedQuestItemUI)
        {
            if(ui.Item == item)
            {
                questItemUI = ui;
            }
        }

        return questItemUI;
    }

    private void RemoveQuestItemUI(QuestItem.Item item)
    {
        QuestItemUI removableQuestItemUI = GetQuestItemUIGameobject(item);
        if (removableQuestItemUI == null) return;
        Destroy(removableQuestItemUI.questItemUIGameobject);
        currentlyUsedQuestItemUI.Remove(removableQuestItemUI);
    }


}

public class QuestItemUI
{
    public GameObject questItemUIGameobject;
    public QuestItem.Item Item;
}
