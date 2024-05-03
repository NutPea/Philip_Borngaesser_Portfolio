using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HighGrasController))]
public class HighGrasChangeInteractableOnCut : MonoBehaviour
{
    HighGrasController _highGrasController;
    public GameObject interactableObject;
    private void Awake()
    {
        interactableObject.SetActive(false);
    }

    void Start()
    {
        _highGrasController = GetComponent<HighGrasController>();
        _highGrasController.onGrasGotCut.AddListener(OnActivateInteractable);
    }

    private void OnActivateInteractable()
    {
        interactableObject.SetActive(interactableObject);
    }
}
