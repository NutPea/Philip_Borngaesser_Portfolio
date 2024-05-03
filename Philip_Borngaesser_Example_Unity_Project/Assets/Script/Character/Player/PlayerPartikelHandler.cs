using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GridMovementController))]
public class PlayerPartikelHandler : MonoBehaviour
{
    public ParticleSystem rightJumpParticle;
    public ParticleSystem leftJumpParticle;
    public ParticleSystem landParticle;
    GridMovementController gridMovement;
    private PlayerBootsHandler playerBootsHandler;
    [Header("JumpCharging")]
    private ParticleSystem sideChargePartikle;
    public ParticleSystem rightChargeJumpParticle;
    public ParticleSystem leftChargeJumpParticle;
    public float resetPartikleTimer = 0.1f;

    private void Awake()
    {
        gridMovement = GetComponent<GridMovementController>();
        gridMovement.OnGridMovementStart.AddListener(OnJump);
        gridMovement.OnGridMovementEnd.AddListener(OnLand);

        playerBootsHandler = GetComponent<PlayerBootsHandler>();
        playerBootsHandler.onJumpFullCharge.AddListener(OnStartCharging);
        playerBootsHandler.onJumpChargeEnd.AddListener(OnStopCharging);

        rightChargeJumpParticle.Stop();
        leftChargeJumpParticle.Stop();
    }
    
    void OnJump()
    {
        ParticleSystem sidePartikle = leftJumpParticle;
        if (gridMovement.isRight) sidePartikle = rightJumpParticle;

        sidePartikle.transform.parent = null;
        sidePartikle.Play();
        StartCoroutine(restartParticleSystem(sidePartikle));
        
    }

    void OnStartCharging()
    {
        sideChargePartikle = leftChargeJumpParticle;
        if (gridMovement.isRight) sideChargePartikle = rightChargeJumpParticle;

        sideChargePartikle.Play();
    }

    void OnStopCharging()
    {
        sideChargePartikle.Stop();
    }

    void OnLand()
    {
        landParticle.transform.parent = null;
        landParticle.Play();
        StartCoroutine(restartParticleSystem(landParticle));
    }

    IEnumerator restartParticleSystem(ParticleSystem particleSystem)
    {
        yield return new WaitForSeconds(resetPartikleTimer);
        particleSystem.transform.parent = transform;
        particleSystem.transform.localPosition = Vector3.zero;
    }

}
