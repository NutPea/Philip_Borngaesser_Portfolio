using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuQuestItemHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    [HideInInspector] public UnityEvent<int> onShowDiscription = new UnityEvent<int>();
    [HideInInspector] public UnityEvent onHideDiscription = new UnityEvent();
    public int index;
    public Image itemSpriteImage;
    public TextMeshProUGUI itemDiscription;

    public void OnDeselect(BaseEventData eventData)
    {
        onHideDiscription.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        onShowDiscription.Invoke(index);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        onHideDiscription.Invoke();
    }

    public void OnSelect(BaseEventData eventData)
    {
        onShowDiscription.Invoke(index);
    }
}
