using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SaveStateManager : MonoBehaviour
{
    public List<EquipmentSaveState> equipmentSaveStates = new List<EquipmentSaveState>();
    public List<NPCSaveState> npcSaveStates = new List<NPCSaveState>();
    public List<QuestItemSaveState> questItemSaveStates = new List<QuestItemSaveState>();
    public List<SaveTriggerEventSaveState> saveTriggerEventSaveStates = new List<SaveTriggerEventSaveState>();
    public List<DialogueConditionSaveState> dialogueConditionSaveStates = new List<DialogueConditionSaveState>();
    public List<PositionSaveState> positionSaveStates = new List<PositionSaveState>();
    public List<PickUpSaveState> pickUpSaveStates = new List<PickUpSaveState>();
    public MoneySaveState moneySaveState = new MoneySaveState();
    public List<KeyPickUpSaveState> keyPickUpSaveStates = new List<KeyPickUpSaveState>();
    public KeySaveState keySaveState = new KeySaveState();
    public List<SpawnableSaveState> spawnableSaveStates = new List<SpawnableSaveState>();
    public ItemsUnlockedSaveState itemsUnlockedSaveState = new ItemsUnlockedSaveState();
    
    public static SaveStateManager instance;

    [HideInInspector] public UnityEvent<Equipment.Item, int, MovementNode> onPlayerDropEquipment = new UnityEvent<Equipment.Item, int, MovementNode>();


    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            transform.parent = null;
            DontDestroyOnLoad(gameObject);
            LoadGameState();
            onPlayerDropEquipment.AddListener(OnSaveDropedEquipment);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void LoadGameState()
    {
        #region Equipment
        EquipmentSaveState swordSaveState = new EquipmentSaveState();
        swordSaveState.name = "Sword Savestate";
        swordSaveState.item = Equipment.Item.Sword;
        equipmentSaveStates.Add(swordSaveState);

        EquipmentSaveState glovesSaveState = new EquipmentSaveState();
        glovesSaveState.name = "Glove Savestate";
        glovesSaveState.item = Equipment.Item.StrengthGloves;
        equipmentSaveStates.Add(glovesSaveState);

        EquipmentSaveState wateringCanSaveState = new EquipmentSaveState();
        wateringCanSaveState.name = "WateringCan Savestate";
        wateringCanSaveState.item = Equipment.Item.WateringCan;
        equipmentSaveStates.Add(wateringCanSaveState);

        EquipmentSaveState tourchCanSaveState = new EquipmentSaveState();
        tourchCanSaveState.name = "Tourch Savestate";
        tourchCanSaveState.item = Equipment.Item.Torch;
        equipmentSaveStates.Add(tourchCanSaveState);
        #endregion

        #region NPC Dialogue
        int amountOfNPCs = Enum.GetValues(typeof(NPCNames.Name)).Length;
        for(int index = 1;index < amountOfNPCs; index++)
        {
            NPCSaveState nPCSaveState = new NPCSaveState();
            nPCSaveState.currentDialogeIndex = 0;
            nPCSaveState.name = (NPCNames.Name)index;
            npcSaveStates.Add(nPCSaveState);
        }

        #endregion


    }

    public void DeleteGameStat()
    {
        equipmentSaveStates = new List<EquipmentSaveState>();
        npcSaveStates = new List<NPCSaveState>();
        questItemSaveStates = new List<QuestItemSaveState>();
        saveTriggerEventSaveStates = new List<SaveTriggerEventSaveState>();
        positionSaveStates = new List<PositionSaveState>();
        moneySaveState = new MoneySaveState();
        keyPickUpSaveStates = new List<KeyPickUpSaveState>();
        keySaveState = new KeySaveState();
}

    private EquipmentSaveState GetEquipmentSaveState(Equipment.Item equipment)
    {
        EquipmentSaveState equipmentSaveState = new EquipmentSaveState();
        equipmentSaveState.name = "EquipmentNotFound";
        foreach(EquipmentSaveState saveState in equipmentSaveStates)
        {
            if(saveState.item == equipment)
            {
                return saveState;
            }
        }
        Debug.Log("Equipment :" + equipment + " Cont not be found!!!!");
        return equipmentSaveState;
    }

    private void OnSaveDropedEquipment(Equipment.Item equipment, int sceneIndex, MovementNode dropedNode)
    {
        GetEquipmentSaveState(equipment).SetEquipmentDropSaveState(sceneIndex, dropedNode);
    }

    public List<EquipmentSaveState> GetDroppedItemsInScene()
    {
        List<EquipmentSaveState> inSceneDroppedItem = new List<EquipmentSaveState>();
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        foreach(EquipmentSaveState saveState in equipmentSaveStates)
        {
            if(saveState.dropedSceneIndex == currentSceneIndex)
            {
                inSceneDroppedItem.Add(saveState);
            }
        }
        return inSceneDroppedItem;
    }


    public bool UnlockEquipment(Equipment.Item equipment)
    {
        bool hasBeenUnlocked = false;
        EquipmentSaveState equipmentSaveState = GetEquipmentSaveState(equipment);
        if (!equipmentSaveState.hasBeenUnlocked)
        {
            equipmentSaveState.hasBeenUnlocked = true;
            hasBeenUnlocked = true;
        }
        return hasBeenUnlocked;
    }

    public bool EquipmentHasBeenUnlocked(Equipment.Item equipment)
    {
        if (equipmentSaveStates.Count == 0) return false;
        return GetEquipmentSaveState(equipment).hasBeenUnlocked;
    }

    public bool AnyEquipmentHasBeenUnlocked()
    {
        bool anyEquipmentHasBeenUnlocked = false;
        foreach(EquipmentSaveState equipmentSaveState in equipmentSaveStates)
        {
            if (equipmentSaveState.hasBeenUnlocked)
            {
                anyEquipmentHasBeenUnlocked = true;
                break;
            }
        }
        return anyEquipmentHasBeenUnlocked;
    }

    public bool EquipmentHasBeenDropped(Equipment.Item equipment)
    {

        return GetEquipmentSaveState(equipment).hasBeenDropped;
    }


    public void GatherDroppedEquipment()
    {
        foreach(EquipmentSaveState saveState in equipmentSaveStates)
        {
            saveState.hasBeenDropped = false;
        }
    }

    public NPCSaveState GetNPCSaveState(NPCNames.Name name)
    {
        if(name == NPCNames.Name.None)
        {
            Debug.Log("Name is None");
            return null;
        }
        NPCSaveState nPCSaveState = null;
        for(int index = 0; index < npcSaveStates.Count; index++)
        {
            if(npcSaveStates[index].name == name)
            {
                nPCSaveState = npcSaveStates[index];
            }
        }
        if(nPCSaveState == null)
        {
            Debug.Log("NPC Name :" + name + " has not been found");
        }

        return nPCSaveState;

    }

    public int GetDialogueIndex(NPCNames.Name name)
    {
        return GetNPCSaveState(name).currentDialogeIndex;
    }

    public void SetDialogueIndex(NPCNames.Name name, int dialogueIndex)
    {
        GetNPCSaveState(name).currentDialogeIndex = dialogueIndex;
    }



    public void SaveSaveTriggerEvent(SaveTriggerEvent saveTriggerEvent)
    {

        SaveTriggerEventSaveState saveTriggerEventSaveState = FindSaveTriggerEvent(saveTriggerEvent);
        if (saveTriggerEventSaveState == null)
        {
            saveTriggerEventSaveState = new SaveTriggerEventSaveState();
            saveTriggerEventSaveState.GUID = saveTriggerEvent.GUID;
            saveTriggerEventSaveStates.Add(saveTriggerEventSaveState);
        } 
    }


    public bool CheckSaveTriggerEventGUID(SaveTriggerEvent saveTriggerEvent)
    {
        bool saveTriggerExists = false;
        SaveTriggerEventSaveState foundState = FindSaveTriggerEvent(saveTriggerEvent);
        if (foundState == null) return saveTriggerExists;

        saveTriggerExists = true;
        saveTriggerEvent.eventWasSaved = true;
        return saveTriggerExists;
    }
    private SaveTriggerEventSaveState FindSaveTriggerEvent(SaveTriggerEvent saveTriggerEvent)
    {
        SaveTriggerEventSaveState foundSaveTriggerSaveState = null;
        foreach (SaveTriggerEventSaveState saveState in saveTriggerEventSaveStates)
        {
            if (saveState.GUID == saveTriggerEvent.GUID)
            {
                foundSaveTriggerSaveState = saveState;
                break;
            }
        }
        return foundSaveTriggerSaveState;
    }


    public bool CheckDialogeConditionSaves(DialogueConditionSaver saver)
    {
        bool saveTriggerExists = false;
        DialogueConditionSaveState foundState = FindDialogueSaveState(saver);
        if (foundState == null) return saveTriggerExists;

        saveTriggerExists = true;
        saver.wasSaved = true;
        return saveTriggerExists;
    }

    private DialogueConditionSaveState FindDialogueSaveState(DialogueConditionSaver saver)
    {
        DialogueConditionSaveState foundSaveTriggerSaveState = null;
        foreach (DialogueConditionSaveState saveState in dialogueConditionSaveStates)
        {
            if (saveState.GUID == saver.GUID)
            {
                foundSaveTriggerSaveState = saveState;
                break;
            }
        }
        return foundSaveTriggerSaveState;
    }

    public void SaveDialogueCondition(DialogueConditionSaver saver)
    {

        DialogueConditionSaveState foundState = FindDialogueSaveState(saver);
        if (foundState == null)
        {
            foundState = new DialogueConditionSaveState();
            foundState.GUID = saver.GUID;
            foreach(Conditions condtion in saver.dialogueConditionHandler.conditions)
            {
                foundState.conditionsHasBeenMet.Add(condtion.conditionHasBeenMet);
            }
            dialogueConditionSaveStates.Add(foundState);
        }
        else
        {
            foundState.conditionsHasBeenMet.Clear();
            foreach (Conditions condtion in saver.dialogueConditionHandler.conditions)
            {
                foundState.conditionsHasBeenMet.Add(condtion.conditionHasBeenMet);
            }
        }
    }


    public List<bool> GetSavedConditions(DialogueConditionSaver saver)
    {
        DialogueConditionSaveState foundState = FindDialogueSaveState(saver);
        if (foundState == null)
        {
            return foundState.conditionsHasBeenMet;
        }
        return null;
    }

    public void SaveGridPosition(PositionSaver positionSaver)
    {
        bool found = true;
        PositionSaveState foundState = FindPositionSaveState(positionSaver);
        if (foundState == null)
        {
            foundState = new PositionSaveState();
            foundState.GUID = positionSaver.GUID;
            found = false;
        }
        foundState.gridPosition.y = positionSaver.gridPosition.x;
        foundState.gridPosition.x = positionSaver.gridPosition.y;
        foundState.scene = SceneNames.currentSceneName;
        if (found == false)
        {
            positionSaveStates.Add(foundState);
        }
    }

    public bool GetSavedGridPosition(PositionSaver positionSaver)
    {
        bool savePositionExists = false;
        PositionSaveState foundState = FindPositionSaveState(positionSaver);
        if (foundState == null) return savePositionExists;

        positionSaver.SetSavedState(foundState);
        savePositionExists = true;

        return savePositionExists;

    }

    private PositionSaveState FindPositionSaveState(PositionSaver positionSaver)
    {
        PositionSaveState foundPositionSaveState = null;
        foreach (PositionSaveState positionSaveState in positionSaveStates)
        {
            if (positionSaveState.GUID == positionSaver.GUID)
            {
                foundPositionSaveState = positionSaveState;
                break;
            }
        }
        return foundPositionSaveState;
    }

    public void SavePickUpPosition(PickUpSaver pickUpSaver)
    {
        bool found = true;
        PickUpSaveState foundPickUpState = FindPickUpSaveState(pickUpSaver);
        if (foundPickUpState == null)
        {
            foundPickUpState = new PickUpSaveState();
            foundPickUpState.GUID = pickUpSaver.GUID;
            found = false;
        }
        foundPickUpState.xPosition = pickUpSaver.transform.position.x;
        foundPickUpState.yPosition = pickUpSaver.transform.position.y;
        foundPickUpState.zPosition = pickUpSaver.transform.position.z;
        foundPickUpState.sceneIndex = SceneManager.GetActiveScene().buildIndex;
        foundPickUpState.pickUpable = pickUpSaver.pickUpAble;
        foundPickUpState.gameObjectName = pickUpSaver.gameObject.name;
        if (found == false)
        {
            pickUpSaveStates.Add(foundPickUpState);
        }
    }

    public bool GetSavedPickUPPosition(PickUpSaver pickUpSaver)
    {
        bool savePositionExists = false;
        PickUpSaveState foundState = FindPickUpSaveState(pickUpSaver);
        if (foundState == null) return savePositionExists;

        pickUpSaver.savedSceneIndex = foundState.sceneIndex;
        savePositionExists = true;

        return savePositionExists;
    }


    private PickUpSaveState FindPickUpSaveState(PickUpSaver pickUpSaver)
    {
        PickUpSaveState foundPickUpSaveState = null;
        foreach (PickUpSaveState pickUpSaveState in pickUpSaveStates)
        {
            if (pickUpSaveState.GUID == pickUpSaver.GUID)
            {
                foundPickUpSaveState = pickUpSaveState;
                break;
            }
        }
        return foundPickUpSaveState;
    }

    public List<PickUpSaveState> FindPickUpSaveStatesDependingOnScene(int sceneIndex)
    {
        List<PickUpSaveState> foundStates = new List<PickUpSaveState>();
        foreach (PickUpSaveState pickUpAble in pickUpSaveStates)
        {
            if (pickUpAble.sceneIndex == sceneIndex)
            {
                foundStates.Add(pickUpAble);
            }
        }

        return foundStates;
    }


    public void SaveQuestItem(QuestItem.Item questItem)
    {
        QuestItemSaveState questItemSaveState = new QuestItemSaveState();
        questItemSaveState.item = questItem;
        questItemSaveStates.Add(questItemSaveState);
    }

    public QuestItemSaveState GetQuestItemSaveState(QuestItem.Item questItem)
    {
        QuestItemSaveState questItemSaveState = null;
        foreach(QuestItemSaveState saveState in questItemSaveStates)
        {
            if(saveState.item == questItem)
            {
                questItemSaveState = saveState;
            }
        }
        return questItemSaveState;
    }

    public void RemoveQuestItem(QuestItem.Item questItem)
    {
        QuestItemSaveState questItemSaveState = GetQuestItemSaveState(questItem);
        if (questItemSaveState == null) return;
        questItemSaveState.hasBeenUsed = true;
    }

    public bool CheckQuestItemInInventory(QuestItem.Item questItem)
    {
        bool hasFound = false;
        foreach(QuestItemSaveState questItemSaveState in questItemSaveStates)
        {
            if(questItemSaveState.item == questItem)
            {
                if (!questItemSaveState.hasBeenUsed)
                {
                    hasFound = true;
                }
            }
        }
        return hasFound;
    }


    public void SaveMoneyAmount(int amount)
    {
        moneySaveState.moneyAmount = amount;
    }

    public int GetSaveMoneyAmount()
    {
        return moneySaveState.moneyAmount;
    }


    public bool CheckKeyPickUpSaveState(int sceneIndex , int keyIndex)
    {
        bool hasFound = false;
        foreach (KeyPickUpSaveState keyPickUpSaveState in keyPickUpSaveStates)
        {
            if(keyPickUpSaveState.keyPickUpScene == sceneIndex)
            {
                if(keyPickUpSaveState.keyPickUpIndex == keyIndex)
                {
                    hasFound = true;
                }
            }
        }

        return hasFound;
    }

    public void SaveKeyPickUpSaveState(int sceneIndex, int keyIndex)
    {
        KeyPickUpSaveState keyPickUpSaveState = new KeyPickUpSaveState();
        keyPickUpSaveState.keyPickUpScene = sceneIndex;
        keyPickUpSaveState.keyPickUpIndex = keyIndex;
        keyPickUpSaveStates.Add(keyPickUpSaveState);
    }

    public void SaveKeyAmount(int amount)
    {
        keySaveState.keyAmount = amount;
    }

    public int GetSaveKeyAmount()
    {
        return keySaveState.keyAmount;
    }


    public SpawnableSaveState HasSpawnableSaveState(SpawnableSaver saver)
    {
        SpawnableSaveState foundState = null;
        foreach(SpawnableSaveState state in spawnableSaveStates)
        {
            if(saver.name == state.GUID)
            {
                return state;
            }

        }
        return foundState;
    }

    public SpawnableSaveState SaveSpawnable(SpawnableSaver saver)
    {
    
        SpawnableSaveState foundState = HasSpawnableSaveState(saver);
        if(foundState == null)
        {
            SpawnableSaveState saveState = new SpawnableSaveState();
            saveState.GUID = saver.name;
            saveState.item = saver.spawnableItem;
            saveState.xPosition = saver.transform.position.x;
            saveState.yPosition = saver.transform.position.y;
            saveState.zPosition = saver.transform.position.z;
            saveState.sceneBuildIndex = SceneManager.GetActiveScene().buildIndex;

            spawnableSaveStates.Add(saveState);
            return saveState;
        }

        return foundState;
    }


    public List<SpawnableSaveState> GetAvaibleSaveStates(int sceneIndex)
    {
        List<SpawnableSaveState> foundStates = new List<SpawnableSaveState>();
        foreach(SpawnableSaveState spawnableSaveState in spawnableSaveStates)
        {
            if(spawnableSaveState.sceneBuildIndex == sceneIndex)
            {
                foundStates.Add(spawnableSaveState);
            }
        }

        return foundStates;
    }


}
