using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EvilPlantHandler : MonoBehaviour
{

    private GridMovementController _gridMovementController;
    private Animator anim;
    public Equipment.Item neededEquipment = Equipment.Item.Sword;
    public EvilPlantEyeHandler eyePlantEye;

    [Header("Attacking")]
    public Transform spawnPosition;
    public GameObject evilPlantProjectile;

    public float timeBetweenShots;
    private float currentTimeBetweenShots;

    public float distanceBeforeAttack;
    public Transform player;
    public UnityEvent onDie = new UnityEvent();

    bool isDead;

    void Start()
    {
        if (player == null) player = OverSceneReceiver.instance.player.transform;

        currentTimeBetweenShots = 0;
        anim = GetComponent<Animator>();
    }

    private void Awake()
    {
        _gridMovementController = GetComponent<GridMovementController>();
        _gridMovementController.OnOccupiedGridMovementRequest.AddListener(OnTryAttack);

        eyePlantEye.onEvilPlantEyeWasDestroyed.AddListener(OnDestoryEvilPlant);
    }

    private void Update()
    {
        if (isDead) return;
        float distanceToPlayer = Vector2.Distance(player.transform.position, transform.position);
        if (distanceToPlayer > distanceBeforeAttack) return;

        if(currentTimeBetweenShots < 0)
        {
            currentTimeBetweenShots = timeBetweenShots;
            anim.SetTrigger("onAttack");
        }
        else
        {
            currentTimeBetweenShots -= Time.deltaTime;
        }
    }

   //AnimationEvent
    private void Shot()
    {
        GameObject plantProjectile = Instantiate(evilPlantProjectile, spawnPosition.position, Quaternion.identity);
        Vector3 dir = player.transform.position - transform.position;
        dir = dir.normalized;

        Projectile pro = plantProjectile.GetComponent<Projectile>();
        pro.direction = dir;
        Destroy(plantProjectile, 3f);
    }

    private void OnTryAttack(Transform player)
    {
        PlayerEquipmentController playerEquipmentController = player.GetComponent<PlayerEquipmentController>();
        if (playerEquipmentController == null) return;
        if (playerEquipmentController.currentItemEnum != neededEquipment) return;

        OnDestoryEvilPlant();
    }

    public void OnDestoryEvilPlant()
    {
        _gridMovementController.GiveCurrentNodeFree();
        isDead = true;
        anim.SetTrigger("onDie");
        onDie.Invoke();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, distanceBeforeAttack);
    }


}
