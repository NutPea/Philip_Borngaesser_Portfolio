using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneDekoAnim : DekoAnim
{
    public float minJumpHeight = 0.25f;
    public float shorterJumpHeight = 0.1f;
    public float minAnimTime = 0.35f;
    public float shorterAnimTime = 0.25f;
    float currentAnimTime;
    float currentJumpHeight;
    float startyPos = 0;


    public LeanTweenType animTyp = LeanTweenType.punch;
    public override void Start()
    {
        base.Start();
        startyPos = transform.localPosition.y;
    }

    public override void PlayAnim(bool isPlayer)
    {
        base.PlayAnim(isPlayer);
        currentAnimTime = minAnimTime - (shorterAnimTime * percentage);
        currentJumpHeight = minJumpHeight - (shorterJumpHeight * percentage);
        LeanTween.moveLocalY(gameObject, startyPos+ currentJumpHeight, currentAnimTime).setOnComplete(MoveBack).setEase(animTyp);
    }

    void MoveBack()
    {
        LeanTween.moveLocalY(gameObject, startyPos, minAnimTime).setEase(animTyp);
    }

}
