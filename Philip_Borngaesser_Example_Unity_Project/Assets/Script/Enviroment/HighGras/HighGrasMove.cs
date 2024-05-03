using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HighGrasController))]
public class HighGrasMove : MonoBehaviour
{
    public float timeBetweenMoves = 1f;
    private float currentTimeBetweenMoves = 0f;
    private bool isNotMoving;

    public LeanTweenType angleMovementTween;
    bool isRotating;
    public float minMovementAngle = 1f;
    public float maxMovementAngle = 5f;
    private float currentMovementAngle = 0.0f;
    public float movementAngleTime = 0.5f;



    void Start()
    {
        currentTimeBetweenMoves = timeBetweenMoves;
        GetComponent<HighGrasController>().onGrasGotCut.AddListener(OnGrasIsCut);
    }

    void OnGrasIsCut()
    {
        isNotMoving = true;
        LeanTween.cancel(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (isNotMoving) return;
        if (isRotating) return;
        if(currentTimeBetweenMoves < 0)
        {
            isRotating = true;
            currentTimeBetweenMoves = timeBetweenMoves;
            currentMovementAngle = Random.Range(minMovementAngle, maxMovementAngle);
            StartMovementAngle();

        }
        else
        {
            currentTimeBetweenMoves -= Time.deltaTime;
        }

    }

    void StartMovementAngle()
    {
        LeanTween.rotateZ(gameObject, currentMovementAngle, movementAngleTime).setEase(angleMovementTween).setOnComplete(RotateToOpositAngle);
    }

    void RotateToOpositAngle()
    {
        LeanTween.rotateZ(gameObject, -currentMovementAngle, movementAngleTime).setEase(angleMovementTween).setOnComplete(RotateBack);
    }

    void RotateBack()
    {
        LeanTween.rotateZ(gameObject,0, movementAngleTime).setEase(angleMovementTween).setOnComplete(RotateBack);
        isRotating = false;
    }

}
