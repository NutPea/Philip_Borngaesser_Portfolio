using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThanksForPlayingMenuController : MonoBehaviour
{

    public Button playAgainButton;

    public Button goToMenuButton;

    public Button quitButton;


    private void Start()
    {
        playAgainButton.onClick.AddListener(PlayAgain);
        goToMenuButton.onClick.AddListener(GoToMenu);
        quitButton.onClick.AddListener(QuitGame);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    public void PlayAgain()
    {
        if(SaveStateManager.instance != null)
        {
            SaveStateManager.instance.DeleteGameStat();

        }
    }

    public void GoToMenu()
    {
        CustomSceneLoader.instance.LoadScene(SceneNames.GetSceneName(SceneNames.Name.MAINMENU));
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
