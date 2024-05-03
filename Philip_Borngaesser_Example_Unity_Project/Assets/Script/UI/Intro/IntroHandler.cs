using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IntroHandler : MonoBehaviour
{
    public GameObject textPivot;
    public float upValue;
    public float goingUpTime;
    public Image transitionPanel;

    public float transitionInTime;
    public LeanTweenType transitionInType = LeanTweenType.easeInSine;


    public float transitionOutTime;
    public LeanTweenType transitionOutType = LeanTweenType.easeInSine;
    void Start()
    {
        TransitionIn();
    }

    void TransitionIn()
    {
        transitionPanel.gameObject.SetActive(true);
        LeanTween.value(gameObject, 1, 0, transitionInTime).setOnUpdate((float val) =>
        {
            Color c = transitionPanel.color;
            c.a = val;
            transitionPanel.color = c;
        }).setEase(transitionInType).setOnComplete(MoveTextUp);
    }

    void MoveTextUp()
    {
        transitionPanel.gameObject.SetActive(false);
        LeanTween.moveLocalY(textPivot,upValue,goingUpTime).setOnComplete(TransitionOut);
    }

    void TransitionOut()
    {
        transitionPanel.gameObject.SetActive(true);
        LeanTween.value(gameObject, 0, 1, transitionOutTime).setOnUpdate((float val) =>
        {
            Color c = transitionPanel.color;
            c.a = val;
            transitionPanel.color = c;
        }).setEase(transitionOutType).setOnComplete(LoadFirstLevel);
    }

    void LoadFirstLevel()
    {
        SceneManager.LoadScene(SceneNames.GetSceneName(SceneNames.Name.BANSTONE_INSIDE));
    }
}
