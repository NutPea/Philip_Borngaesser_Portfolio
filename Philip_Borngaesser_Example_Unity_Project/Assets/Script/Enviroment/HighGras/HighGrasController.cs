using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(GridMovementController))]
public class HighGrasController : MonoBehaviour
{
    private GridMovementController _gridMovementController;
    public Equipment.Item neededEquipment = Equipment.Item.Sword;

    public ParticleSystem cutGrasParticleSystem;
    public GameObject uncutGras;
    public GameObject cutGras;

    public float animTime = 0.2f;
    public float rotateAmount = 5f;
    public LeanTweenType animTyp = LeanTweenType.punch;
    private bool _needToInitCutGrasListener;

    [HideInInspector] public UnityEvent onGrasGotCut = new UnityEvent();
    public UnityEvent onPlayerCutGras = new UnityEvent();

    void Awake()
    {
        _gridMovementController = GetComponent<GridMovementController>();
        _gridMovementController.OnOccupiedGridMovementRequest.AddListener(OnCutGras);
    }

    private void OnCutGras(Transform user)
    {
        PlayerEquipmentController playerEquipmentController = user.GetComponent<PlayerEquipmentController>();
        if (playerEquipmentController == null) return;
        if (playerEquipmentController.currentItemEnum != neededEquipment) return;

        onPlayerCutGras.Invoke();
        cutGrasParticleSystem.Play();
        ForceCutGrass();

    }

    public void ForceCutGrass()
    {
        SetJumpOnCutGrasListener();
        CutGras();
    }

    private void CutGras()
    {
        uncutGras.SetActive(false);
        cutGras.SetActive(true);
        _gridMovementController.GiveCurrentNodeFree();
        onGrasGotCut.Invoke();
    }

    private void SetJumpOnCutGrasListener()
    {
        if(_gridMovementController.currentMovementNode == null)
        {
            return;
        }
        _gridMovementController.currentMovementNode.onArrivedAtNode.AddListener(SetJumpOnCutGras);
    }

    private void SetJumpOnCutGras(bool isPlayer)
    {
        float randomeAmoun = UnityEngine.Random.Range(0.0f, 1.0f);
        if(randomeAmoun < 0.5f)
        {
            LeanTween.rotate(cutGras, new Vector3(0, 0, rotateAmount), animTime).setOnComplete(ResetAnim).setEase(animTyp);
        }
        else
        {
            LeanTween.rotate(cutGras, new Vector3(0, 0, -rotateAmount), animTime).setOnComplete(ResetAnim).setEase(animTyp);
        }

    }

    private void ResetAnim()
    {
        LeanTween.rotate(cutGras, new Vector3(0, 0,0), animTime).setEase(animTyp);
    }

}
