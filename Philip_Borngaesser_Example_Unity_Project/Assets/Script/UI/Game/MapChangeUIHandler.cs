using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapChangeUIHandler : MonoBehaviour
{

    public TextMeshProUGUI mapNameText;
    public List<Image> backgroundImages;
    public List<float> backgroundImagesAplhaValue;

    public float fadeInTime = 1f;
    public float showTime = 1f;
    public float fadeOutTime = 1f;

    public LeanTweenType fadeInType = LeanTweenType.easeInSine;
    public LeanTweenType fadeOutType = LeanTweenType.easeInSine;

    void Start()
    {
        foreach(Image image in backgroundImages)
        {
            backgroundImagesAplhaValue.Add(image.color.a);
            Color imageColor = image.color;
            imageColor.a = 0;
            image.color = imageColor;
            image.gameObject.SetActive(false);
        }
        Color textColor = mapNameText.color;
        textColor.a = 0;
        mapNameText.color = textColor;
        mapNameText.gameObject.SetActive(false);

        ShowMapUI();
    }


    private void ShowMapUI()
    {

        foreach (Image image in backgroundImages)
        {
            image.gameObject.SetActive(true);
        }
        mapNameText.gameObject.SetActive(true);
        mapNameText.text = SceneNames.GetSceneNameDescriptionBySceneName(SceneManager.GetActiveScene().name);

        LeanTween.value(gameObject, 0, 1, fadeInTime).setOnUpdate((float val) =>
        {
            for(int index = 0; index < backgroundImages.Count; index++)
            {
                Color imageColor = backgroundImages[index].color;
                imageColor.a = backgroundImagesAplhaValue[index] * val;
                backgroundImages[index].color = imageColor;
            }
            Color textColor = mapNameText.color;
            textColor.a = val;
            mapNameText.color = textColor;
        }).setEase(fadeInType).setOnComplete(StartsWaitCouroutine);
    }

    public void StartsWaitCouroutine()
    {
        StartCoroutine(WaitCouroutine());

    }

    IEnumerator WaitCouroutine()
    {
        yield return new WaitForSeconds(showTime);
        LeanTween.value(gameObject, 1, 0, fadeOutTime).setOnUpdate((float val) =>
        {
            for (int index = 0; index < backgroundImages.Count; index++)
            {
                Color imageColor = backgroundImages[index].color;
                imageColor.a = backgroundImagesAplhaValue[index] * val;
                backgroundImages[index].color = imageColor;
            }
            Color textColor = mapNameText.color;
            textColor.a = val;
            mapNameText.color = textColor;
        }).setEase(fadeOutType).setOnComplete(RemoveUI);
    }

    public void RemoveUI()
    {
        foreach (Image image in backgroundImages)
        {
            image.gameObject.SetActive(false);
        }
        mapNameText.gameObject.SetActive(false);
    }
}
