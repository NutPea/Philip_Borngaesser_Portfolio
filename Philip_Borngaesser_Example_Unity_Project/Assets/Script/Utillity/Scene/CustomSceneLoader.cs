using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomSceneLoader : MonoBehaviour
{
    public static CustomSceneLoader instance;
    public TransitionHandler transitionHandler;
    int sceneIndex;

    private void Awake()
    {
        instance = this;
        transitionHandler.onTransitionHasFinished.AddListener(TransitionHasFinished);
    }

    public void LoadScene(int index)
    {
        transitionHandler.OutTransition();
        sceneIndex = index;
    }

    void TransitionHasFinished()
    {
        SceneManager.LoadScene(sceneIndex);
    }

}
