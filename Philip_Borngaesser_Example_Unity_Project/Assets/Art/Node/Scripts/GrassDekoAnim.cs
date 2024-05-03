using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassDekoAnim : DekoAnim
{

    public float minAnimAngle = 20;
    public float extraAnimAngle = 20;
    float currAnimAngle;
    public float minAnimTime = 0.35f;
    public float shorterAnimTime = 0.25f;
    float currentAnimTime;
    Vector3 startRot;

    public LeanTweenType animTyp = LeanTweenType.punch;
    public override void Start()
    {
        base.Start();
        startRot = transform.eulerAngles;
       
    }

    public override void PlayAnim(bool isPlayer)
    {
        base.PlayAnim(isPlayer);
        if (isRight)
        {
            currAnimAngle = minAnimAngle + (extraAnimAngle * percentage);
        }
        else
        {
            currAnimAngle = - (minAnimAngle + (extraAnimAngle * percentage));
        }
        currentAnimTime = minAnimTime - (shorterAnimTime * percentage);
        Vector3 toRotAngle = startRot + new Vector3(0, 0, currAnimAngle);
        LeanTween.rotate(gameObject, toRotAngle, currentAnimTime).setOnComplete(RotBackMid).setEase(animTyp);
    }

    void RotBackMid()
    {
        LeanTween.rotate(gameObject, startRot, currentAnimTime).setEase(animTyp);
    }



}
