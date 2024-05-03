using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonScaleOnHover : MonoBehaviour, IPointerEnterHandler , IPointerExitHandler , ISelectHandler ,IDeselectHandler
{
    public float scaleAmount= 1.1f;
    public float scaleTime = 0.1f;
    float startScaleAmount;
    private void Start()
    {
        startScaleAmount = transform.localScale.x;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        LeanTween.cancel(gameObject);
        LeanTween.scale(gameObject, new Vector3(scaleAmount, scaleAmount, 1), scaleTime).setIgnoreTimeScale(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        LeanTween.cancel(gameObject);
        LeanTween.scale(gameObject, new Vector3(startScaleAmount, startScaleAmount, 1), scaleTime).setIgnoreTimeScale(true);
    }

    public void OnSelect(BaseEventData eventData)
    {
        LeanTween.cancel(gameObject);
        LeanTween.scale(gameObject, new Vector3(scaleAmount, scaleAmount, 1), scaleTime).setIgnoreTimeScale(true);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        LeanTween.cancel(gameObject);
        LeanTween.scale(gameObject, new Vector3(startScaleAmount, startScaleAmount, 1), scaleTime).setIgnoreTimeScale(true);
    }
}
