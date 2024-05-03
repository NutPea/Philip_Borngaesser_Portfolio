using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "QuestItemSpriteData", fileName = "QuestItemSpriteData")]
public class QuestItemSpriteData : ScriptableObject
{
    public List<QuestItemSprite> questItemSprites;


    public Sprite GetSpriteForQuestItemUI(QuestItem.Item item)
    {
        Sprite currentSprite = null;
        foreach(QuestItemSprite questItemSprite in questItemSprites)
        {
            if(questItemSprite.item == item)
            {
                currentSprite = questItemSprite.questItemSprite;
            }
        }
        return currentSprite;
    }
    
}
[System.Serializable]
public class QuestItemSprite
{
    public QuestItem.Item item;
    public Sprite questItemSprite;
}
