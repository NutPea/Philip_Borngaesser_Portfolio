using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StepsUIHandler : MonoBehaviour
{
    public GameObject player;
    public TextMeshProUGUI stepText;
    public Vector3 steptextScaleAmount = Vector3.one;
    public float scaleTime;
    public LeanTweenType scaleTweenTyp;

    PlayerStepHandler _playerStepHandler;
    void Awake()
    {
        _playerStepHandler = player.GetComponent<PlayerStepHandler>();
        _playerStepHandler.onStepUpdate.AddListener(OnUpdateStepUI);
        if (_playerStepHandler.stepDoesNotCounts) gameObject.SetActive(false);
        else gameObject.SetActive(true);
    }

    private void OnUpdateStepUI()
    {
        stepText.text = _playerStepHandler.currentStepAmount.ToString();
        LeanTween.scale(stepText.gameObject, steptextScaleAmount, scaleTime).setEase(scaleTweenTyp).setOnComplete(OnScaleBack);
    }

    private void OnScaleBack()
    {
        LeanTween.scale(stepText.gameObject, Vector3.one, scaleTime).setEase(scaleTweenTyp);
    }
}
