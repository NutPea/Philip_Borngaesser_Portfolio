using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSoundeffectOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    public SoundLibary.SFX buttonHoverSoundeffect;
    float startScaleAmount;
    private void Start()
    {
        startScaleAmount = transform.localScale.x;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SoundManager.instance.PlayLibarySound(buttonHoverSoundeffect);
    }

    public void OnPointerExit(PointerEventData eventData)
    {

    }

    public void OnSelect(BaseEventData eventData)
    {
        SoundManager.instance.PlayLibarySound(buttonHoverSoundeffect);
    }

    public void OnDeselect(BaseEventData eventData)
    {

    }
}
