using PixelCrushers.DialogueSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class QuestItemMenuHandler : MonoBehaviour
{

    public GameObject questItemPrefab;
    public GameObject questMenuContentHolder;
    public Transform questItemContent;
    [SerializeField] private List<MenuQuestItem> questItems = new List<MenuQuestItem>();
    public List<MenuQuestItemHandler> menuQuestItemHandlers; 

    [Header("Discription")]
    public GameObject discriptionContainer;
    public TextMeshProUGUI discriptionText;

    public UnityEvent onOpenItemMenu;
    public UnityEvent onCloseItemMenu;

    bool init;
    public bool isOpen;
    public bool isInPause;

    public PauseUiHandler pauseUiHandler;


    private void Awake()
    {
        questMenuContentHolder.SetActive(false); 
        HideDiscription();
    }

    void Start()
    {
        PlayerInput input = InputManager.instance.inputActions;
        input.Keyboard.Inventory.performed += ctx => ToggleMenu();

        pauseUiHandler.onPause.AddListener(OnOpenPauseMenu);
        pauseUiHandler.onUnpause.AddListener(OnClosePauseMenu);
    }

    private void ToggleMenu()
    {
        if(isInPause)
        {
            return;
        }

        if (!isOpen)
        {
            if (!init)
            {
                SpawnQuestItemUI();
                init = true;
            }
            else
            {
                RedrawQuestItemUI();
            }
            isOpen = true;
            OpenQuestItenMenu();
        }
        else
        {
            isOpen = false;
            CloseQuestItemMenu();
        }
    }


    private void OnOpenPauseMenu()
    {
        isInPause = true;
        if (!isOpen) return;
        questMenuContentHolder.SetActive(false);
       
    }

    private void OnClosePauseMenu()
    {
        isInPause = false;
        if (!isOpen) return;
        OpenQuestItenMenu();
    }
    private void CloseQuestItemMenu()
    {
        questMenuContentHolder.SetActive(false);
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible= false;
    }

    private void OpenQuestItenMenu()
    {
        questMenuContentHolder.SetActive(true);
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void SpawnQuestItemUI()
    {
        for(int i = 0; i< questItems.Count; i++)
        {
            GameObject questItem = Instantiate(questItemPrefab, transform.position, Quaternion.identity);
            questItem.transform.parent = questItemContent.transform;
            MenuQuestItemHandler menuQuestItemHandler = questItem.GetComponent<MenuQuestItemHandler>();
            menuQuestItemHandlers.Add(menuQuestItemHandler);
            menuQuestItemHandler.index = i;

            menuQuestItemHandler.onShowDiscription.AddListener(ShowDiscription);
            menuQuestItemHandler.onHideDiscription.AddListener(HideDiscription);
        }

        RedrawQuestItemUI();
    }

    private void RedrawQuestItemUI()
    {
        foreach(MenuQuestItemHandler handler in menuQuestItemHandlers)
        {

            bool variableValue = DialogueLua.GetVariable(questItems[handler.index].variableName).asBool;
            handler.itemSpriteImage.sprite = questItems[handler.index].itemSprite;
            if (variableValue){
                handler.itemSpriteImage.color= Color.white;
                handler.itemDiscription.text = questItems[handler.index].itemName;
            }
            else{
                handler.itemSpriteImage.color = Color.black;
                handler.itemDiscription.text = "???";
            }
            
        }
    }

    public void ShowDiscription(int index)
    {

        discriptionContainer.SetActive(true);
        bool variableValue = DialogueLua.GetVariable(questItems[index].variableName).asBool;
        if (variableValue)
        {
            discriptionText.text= questItems[index].itemDiscription;

        }
        else
        {
            discriptionText.text = "???";
        }


    }

    public void HideDiscription()
    {
        discriptionContainer.SetActive(false);
    }
}
