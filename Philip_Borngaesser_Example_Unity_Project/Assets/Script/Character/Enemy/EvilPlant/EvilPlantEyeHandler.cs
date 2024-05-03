using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EvilPlantEyeHandler : MonoBehaviour
{
    private GridMovementController _gridMovementController;
    public Equipment.Item neededEquipment = Equipment.Item.Sword;
    private Animator anim;
    public UnityEvent onEvilPlantEyeWasDestroyed;
    private GameObject player;

    public Transform eyeSpriteTRansform;
    public float lookDistance = 0.1f;
    bool isDead;

    private void Awake()
    {
        _gridMovementController = GetComponent<GridMovementController>();
        _gridMovementController.OnOccupiedGridMovementRequest.AddListener(OnTryAttack);
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        player = OverSceneReceiver.instance.player;
    }

    private void Update()
    {
        if (isDead) return;
        Vector2 dirToPlayer = player.transform.position - transform.position;
        dirToPlayer = dirToPlayer.normalized;
        dirToPlayer = dirToPlayer * lookDistance;
        eyeSpriteTRansform.transform.localPosition = dirToPlayer;
    }

    private void OnTryAttack(Transform player)
    {
        PlayerEquipmentController playerEquipmentController = player.GetComponent<PlayerEquipmentController>();
        if (playerEquipmentController == null) return;
        if (playerEquipmentController.currentItemEnum != neededEquipment) return;

        OnDestoryEvilEyePlant();
    }

    public void OnDestoryEvilEyePlant()
    {
        isDead = true;
        onEvilPlantEyeWasDestroyed.Invoke();
        _gridMovementController.GiveCurrentNodeFree();
        anim.SetTrigger("onDie");
    }

   
}
