using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUIHandler : MonoBehaviour
{

    public List<GameObject> healthIcons;
    public GameObject healthIconPrefab;
    public GameObject healthIconContainer;

    public GameObject player;
    HealthManager _healthManager;

    [Header("Preview")]
    public GameObject healthIconPreview;


    void Start()
    {
        healthIconContainer.SetActive(false);

        if (player == null) player = GameObject.FindGameObjectWithTag("Player");
        _healthManager = player.GetComponent<HealthManager>();
        _healthManager.OnCalculateDamage.AddListener(OnUpdateHealthUI);
        InitHealthUI();
    }

    public void InitHealthUI()
    {
        for(int index = 0; index < _healthManager.healthData.health; index++)
        {
            GameObject icon = Instantiate(healthIconPrefab, transform.position, Quaternion.identity);
            icon.name = "HealthIcon " + index;
            icon.transform.parent = healthIconContainer.transform;
            icon.transform.position = Vector3.zero;
        }


    }

    private void OnUpdateHealthUI(bool dead, int amount, Transform damageTrans)
    {
        UpdateHealthUI();
    }

    public void UpdateHealthUI()
    {
        for(int index = 0; index < _healthManager.healthData.health; index++)
        {
            if (_healthManager.currentHealth <= index){
                healthIcons[index].SetActive(false);
                continue;
            }

            healthIcons[index].SetActive(true);
        }


    }


}
