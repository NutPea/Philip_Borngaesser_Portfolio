using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderEnemySoundHandler : MonoBehaviour
{
    public float toPlayerDistance = 2f;
    GridMovementController gridMovement;
    HealthManager healthManager;
    Transform player;

    void Start()
    {
        gridMovement = GetComponent<GridMovementController>();
        gridMovement.OnGridInteractStart.AddListener(PlayAttackSound);
        gridMovement.OnGridMovementStart.AddListener(PlayMoveSound);
        healthManager = GetComponent<HealthManager>();
        healthManager.OnCalculateDamage.AddListener(OnHit);

        player = OverSceneReceiver.instance.player.transform;
    }

    private void OnHit(bool dead, int arg1, Transform arg2)
    {
        if (dead)
        {
            SoundManager.instance.PlayLibarySound(SoundLibary.SFX.OnSpiderDead);
        }
        else
        {
            SoundManager.instance.PlayLibarySound(SoundLibary.SFX.OnSpiderDamage);
        }
    }


    private void PlayMoveSound()
    {
        float distanceToPlayer = Vector2.Distance(player.transform.position, transform.position);
        if(distanceToPlayer < toPlayerDistance)
        {
            SoundManager.instance.PlayLibarySound(SoundLibary.SFX.OnSpiderMove);
        }
    }

    private void PlayAttackSound(MovementNode arg0)
    {
        SoundManager.instance.PlayLibarySound(SoundLibary.SFX.OnSpiderInteract);
    }


}
