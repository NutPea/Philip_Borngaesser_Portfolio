using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PauseUiHandler : MonoBehaviour
{

    public List<GameObject> GameUI;
    public List<GameObject> PauseUI;
    public bool isPaused;
    public OptionsUIHandler optionsUIHandler;

    [HideInInspector] public UnityEvent onPause;
    [HideInInspector] public UnityEvent onUnpause;

    void Start()
    {
        InputManager.instance.inputActions.Keyboard.PauseMenu.performed += ctx => TogglePause();
        UnPauseGame();
    }



    public void TogglePause()
    {
        if (optionsUIHandler.optionsAreOpen)
        {
            optionsUIHandler.ToggleOptions();
            return;
        }
        if (isPaused)
        {
            PauseGame();
            isPaused = false;
        }
        else
        {
            UnPauseGame();
            isPaused = true;
        }
    }


    public void PauseGame()
    {
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        foreach(GameObject gameObject in GameUI)
        {
            gameObject.SetActive(false);
        }

        foreach (GameObject gameObject in PauseUI)
        {
            gameObject.SetActive(true);
        }
        onPause.Invoke();
    }

    public void UnPauseGame()
    {
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        foreach (GameObject gameObject in GameUI)
        {
            gameObject.SetActive(true);
        }

        foreach (GameObject gameObject in PauseUI)
        {
            gameObject.SetActive(false);
        }
        onUnpause.Invoke();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void BackToMenu()
    {
        CustomSceneLoader.instance.LoadScene(SceneNames.GetSceneName(SceneNames.Name.MAINMENU));
    }

}
