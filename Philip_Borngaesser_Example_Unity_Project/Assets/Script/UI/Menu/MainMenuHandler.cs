using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuHandler : MonoBehaviour
{
    public Image transitionPanel;
    public float transitionTime;
    public LeanTweenType transitionInType = LeanTweenType.easeInOutSine;
    public LeanTweenType transitionOutType = LeanTweenType.easeInOutSine;

    public OptionsUIHandler optionsUIHandler;

    void Start()
    {
        TransitionPanelIn();
        if(SaveStateManager.instance != null)
        {
            SaveStateManager.instance.DeleteGameStat();
        }
        InputManager.instance.inputActions.Keyboard.PauseMenu.performed += ctx => CloseOptions();
    }

    public void TransitionPanelIn()
    {
        transitionPanel.gameObject.SetActive(true);
        LeanTween.value(gameObject, 1, 0, transitionTime).setOnUpdate((float val) =>
        {
            Color c = transitionPanel.color;
            c.a = val;
            transitionPanel.color = c;
        }).setEase(transitionInType).setOnComplete(FinishTransitionPanelIn);
    }

    public void FinishTransitionPanelIn()
    {
        transitionPanel.gameObject.SetActive(false);
    }

    public void TransitionPanelOut()
    {
        transitionPanel.gameObject.SetActive(true);
        LeanTween.value(gameObject, 0, 1, 1).setOnUpdate((float val) =>
        {
            Color c = transitionPanel.color;
            c.a = val;
            transitionPanel.color = c;
        }).setEase(transitionInType).setOnComplete(FinishTransitionPanelOut);
    }

    public void FinishTransitionPanelOut()
    {
        Cursor.visible = false;
        SceneManager.LoadScene(SceneNames.GetSceneName(SceneNames.Name.INTRO));
    }

    public void _StartGame()
    {
        TransitionPanelOut();
    }

    public void _ToggleOptions()
    {
        optionsUIHandler.ToggleOptions();
    }

    public void CloseOptions()
    {
        if (optionsUIHandler.optionsAreOpen)
        {
            optionsUIHandler.ToggleOptions();
        }
    }

    public void _QuitGame()
    {
        Application.Quit();
    }
}
