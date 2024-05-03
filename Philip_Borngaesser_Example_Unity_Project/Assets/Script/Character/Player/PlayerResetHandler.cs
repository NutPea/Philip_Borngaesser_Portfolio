using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerResetHandler : MonoBehaviour
{
    private PlayerEquipmentController _playerEquipmentController;
    private PlayerStepHandler _playerStepHandler;
    private HealthManager _healthManager;
    private PlayerMovement _playerMovement;

    public string banStoneSceneName = "BanStone_Inside";
    public float restartTimer =3f;
    void Start()
    {
        _playerEquipmentController = GetComponent<PlayerEquipmentController>();
        _playerStepHandler = GetComponent<PlayerStepHandler>();
        _healthManager = GetComponent<HealthManager>();
        _playerMovement = GetComponent<PlayerMovement>();

        _playerStepHandler.onStepDead.AddListener(OnResetPlayer);
        _healthManager.OnDeath.AddListener(OnResetPlayer);


    }

    private void OnResetPlayer()
    {
        _playerEquipmentController.SetInputIsBlocked(true);
        _playerMovement.SetInputIsBlocked(true);
        if (_playerEquipmentController.currentItemEnum != Equipment.Item.None && _playerEquipmentController.currentEquipment != null)
        {
            _playerEquipmentController.currentEquipment.DropEquipment(_playerEquipmentController);
        }
        StartCoroutine(OnResetPlayerRoutine());
    }

    IEnumerator OnResetPlayerRoutine()
    {
        yield return new WaitForSeconds(restartTimer);
        //Play Dead Animation change scene at the end of animation
        CustomSceneLoader.instance.LoadScene(SceneNames.GetSceneName(SceneNames.Name.BANSTONE_INSIDE));
    }
}
