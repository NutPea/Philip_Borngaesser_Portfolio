using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GridMovementController))]
public class ProcedualIdleAnimation : MonoBehaviour
{

    GridMovementController _gridMovementController;
    public Transform sprite;
    Vector3 _startScale;
    public Vector3 upScaleVector = new Vector3(0.8f, 1.1f, 1.0f);
    public Vector3 downScaleVector = new Vector3(1.1f, 0.8f, 1.0f);



    public float upScaleTime = 0.6f;
    float currentUpScaleTime;
    bool _isUpScaling;
    public float downScaleTime = 0.6f;
    float currentDownScaleTime;
    bool _isDownScaling;

    bool movementTrigger;
    void Start()
    {
        _gridMovementController = GetComponent<GridMovementController>();
        currentUpScaleTime = upScaleTime;
        currentDownScaleTime = downScaleTime;
        _startScale = sprite.transform.localScale;

        ResetIdleing();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_gridMovementController.isMoving &&!_gridMovementController.isInteracting)
        {
            movementTrigger = false;
            if (_isUpScaling)
            {
                if (currentUpScaleTime < 0)
                {
                    currentUpScaleTime = upScaleTime;
                    _isDownScaling = true;
                    _isUpScaling = false;
                }
                else
                {
                    currentUpScaleTime -= Time.deltaTime;
                    float percantage = 1-(currentUpScaleTime / upScaleTime);
                    if(_gridMovementController.isRight) sprite.transform.localScale = Vector3.Lerp(downScaleVector, upScaleVector, percantage);
                    else sprite.transform.localScale = Vector3.Lerp(new Vector3(-downScaleVector.x, downScaleVector.y, downScaleVector.z), new Vector3(-upScaleVector.x, upScaleVector.y, upScaleVector.z), percantage);

                }
            }


            if (_isDownScaling)
            {
                if (currentDownScaleTime < 0)
                {
                    currentDownScaleTime = downScaleTime;
                    _isDownScaling = false;
                    _isUpScaling = true;
                }
                else
                {
                    currentDownScaleTime -= Time.deltaTime;
                    float percantage = 1 - (currentDownScaleTime/ downScaleTime);
                    if (_gridMovementController.isRight) sprite.transform.localScale = Vector3.Lerp(upScaleVector,downScaleVector, percantage);
                    else sprite.transform.localScale = Vector3.Lerp(new Vector3(-upScaleVector.x, upScaleVector.y, upScaleVector.z),new Vector3(-downScaleVector.x, downScaleVector.y, downScaleVector.z), percantage);
                }
            }
        }
        else
        {
            if (!movementTrigger)
            {
                ResetIdleing();
                movementTrigger = true;
            }
        }
    }

    void ResetIdleing()
    {
        currentUpScaleTime = upScaleTime;
        currentDownScaleTime = downScaleTime;
        _isDownScaling = false;
        _isUpScaling = true;
    }
}
