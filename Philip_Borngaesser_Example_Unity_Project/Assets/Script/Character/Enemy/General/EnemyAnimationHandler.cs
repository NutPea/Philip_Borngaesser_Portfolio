using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HealthManager))]
public class EnemyAnimationHandler : MonoBehaviour
{
    public Animator animator;
    public string animationDeathTrigger;
    
    private HealthManager _healthManager;
    void Start()
    {
        _healthManager = GetComponent<HealthManager>();
        _healthManager.OnDeath.AddListener(OnTriggerDeathAnimation);
    }

    private void OnTriggerDeathAnimation()
    {
        animator.SetTrigger(animationDeathTrigger);
    }


}
