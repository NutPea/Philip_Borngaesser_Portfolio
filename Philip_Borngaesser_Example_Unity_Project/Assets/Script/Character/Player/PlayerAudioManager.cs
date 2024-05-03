using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioManager : MonoBehaviour
{
    GridMovementController _gridMovementController;
    PlayerEquipmentController _playerEquipmentController;
    HealthManager _healthManager;
    PlayerKeyHandler _playerKeyHandler;
    NonPlayerDialogueManager dialogueManager;


    void Start()
    {
        _playerEquipmentController = GetComponent<PlayerEquipmentController>();
        _gridMovementController = GetComponent<GridMovementController>();
        _healthManager = GetComponent<HealthManager>();
        _playerKeyHandler = GetComponent<PlayerKeyHandler>();
        dialogueManager = GetComponent<NonPlayerDialogueManager>();


        _gridMovementController.OnGridInteractEnd.AddListener(PlayOnInteract);
        _gridMovementController.OnGridMovementStart.AddListener(PlayOnJump);
        _gridMovementController.OnGridMovementEnd.AddListener(PlayOnLand);
        _gridMovementController.OnMovementNotAllowed.AddListener(PlayCantMove);

        _playerEquipmentController.onEquipItem.AddListener(PlayOnEquipItem);

        _healthManager.OnCalculateDamage.AddListener(PlayDamage);

        _playerKeyHandler.OnKeyUpdate.AddListener(OnPlayKey);
        dialogueManager.onNewLetterHasPrinted.AddListener(OnLetterHasPrinted);




    }

    private void OnLetterHasPrinted()
    {
        SoundManager.instance.PlayContinuousLibarySound(SoundLibary.SFX.OnUITextPopUpSound);
    }

    private void OnPlayKey(bool pickUp)
    {
        if (pickUp)
        {
            SoundManager.instance.PlayLibarySound(SoundLibary.SFX.OnKeyPickUp);
        }
        else
        {
            //KeyGotUsed Use for Gate (For now)
            SoundManager.instance.PlayLibarySound(SoundLibary.SFX.OnGateOpen);
        }
    }

    private void PlayOnEquipItem(Equipment.Item item)
    {
        switch (item)
        {
            case Equipment.Item.None:
                SoundManager.instance.PlayLibarySound(SoundLibary.SFX.OnDropEquipment);
                break;
            case Equipment.Item.Sword:
                SoundManager.instance.PlayLibarySound(SoundLibary.SFX.OnPickUpSword);
                break;
            case Equipment.Item.StrengthGloves:
                SoundManager.instance.PlayLibarySound(SoundLibary.SFX.OnPickUpGlove);
                break;
        }
    }

    private void PlayDamage(bool dead, int arg1, Transform arg2)
    {
        if (dead)
        {
            SoundManager.instance.PlayLibarySound(SoundLibary.SFX.OnPlayerDead);
        }
        else
        {
            SoundManager.instance.PlayRandomLibarySound(SoundLibary.SFX.OnPlayerDamage);
        }
    }

    private void PlayCantMove()
    {
        SoundManager.instance.PlayContinuousLibarySound(SoundLibary.SFX.OnPlayer_CantMove);
    }

    private void PlayOnJump()
    {
        SoundManager.instance.PlayRandomLibarySound(SoundLibary.SFX.OnPlayer_JumpDefault);
    }

    private void PlayOnLand()
    {
        SoundManager.instance.PlayRandomLibarySound(SoundLibary.SFX.OnPlayer_LandDefault);
    }

    private void PlayOnInteract(MovementNode arg0)
    {
        Equipment.Item item = _playerEquipmentController.currentItemEnum;
        switch (item)
        {
            case Equipment.Item.None:
                SoundManager.instance.PlayLibarySound(SoundLibary.SFX.OnPlayerInteract_Without);
                break;
            case Equipment.Item.Sword:
                SoundManager.instance.PlayLibarySound(SoundLibary.SFX.OnPlayerInteract_WithSword);
                break;
            case Equipment.Item.StrengthGloves:
                SoundManager.instance.PlayLibarySound(SoundLibary.SFX.OnPlayerInteract_WithGloves);
                break;
        }
    }



}
