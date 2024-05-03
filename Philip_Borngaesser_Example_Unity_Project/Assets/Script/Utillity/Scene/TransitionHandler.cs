using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TransitionHandler : MonoBehaviour
{
    public GameObject transitionPanel;
    public UnityEvent onTransitionHasFinished = new UnityEvent();
    public LeanTweenType outTransitionTweenTyp = LeanTweenType.easeInSine;
    public LeanTweenType inTransitionTweenTyp = LeanTweenType.easeInSine;
    public float transitionTime = 1.5f;

    private void Awake()
    {
        transitionPanel.gameObject.SetActive(true);
        InTransition();
        
    }

    public void OutTransition()
    {
        Time.timeScale = 0;
        transitionPanel.gameObject.SetActive(true);

        Image imageColor = transitionPanel.GetComponent<Image>();
        Color color = imageColor.color;
        color.a = 0;
        imageColor.color = color;

        LeanTween.value(gameObject, 0, 1, transitionTime).setOnUpdate((float val) =>
        {
            Color c = imageColor.color;
            c.a = val;
            imageColor.color = c;
        }).setIgnoreTimeScale(true).setEase(outTransitionTweenTyp).setOnComplete(OnOutTransitionEnd);
    }

    void OnOutTransitionEnd()
    {
        onTransitionHasFinished.Invoke();
        Time.timeScale = 1;
    }

    public void InTransition()
    {
        transitionPanel.gameObject.SetActive(true);

        Image imageColor = transitionPanel.GetComponent<Image>();
        Color color = imageColor.color;
        color.a = 1;
        imageColor.color = color;

        LeanTween.value(gameObject, 1, 0, transitionTime).setOnUpdate((float val) =>
        {
            Color c = imageColor.color;
            c.a = val;
            imageColor.color = c;
        }).setEase(inTransitionTweenTyp).setOnComplete(OnInTransitionEnd);
    }

    void OnInTransitionEnd()
    {
        transitionPanel.gameObject.SetActive(false);
    }
    


}
