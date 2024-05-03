using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class DemoButton : MonoBehaviour , IPointerDownHandler, IPointerUpHandler
{
    [HideInInspector] public UnityEvent onButtonDown = new UnityEvent();
    [HideInInspector] public UnityEvent onButtonUp = new UnityEvent();

    public void OnPointerDown(PointerEventData eventData)
    {
        onButtonDown.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        onButtonUp.Invoke();
    }

    
}
