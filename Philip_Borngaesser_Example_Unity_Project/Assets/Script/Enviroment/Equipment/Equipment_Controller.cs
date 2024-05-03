using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(GridMovementController))]
public class Equipment_Controller : MonoBehaviour
{
    GridMovementController _gridMovementController;
    public Equipment.Item equipment;
    private bool isEquiped;
    public bool saveEquipment = true;
    public bool IsEquiped
    {
        get { return isEquiped; }
        set { isEquiped = value;}
    }

    public bool stopStartMovement;

    public bool dropItemTest;

    public GameObject dropedSpriteRoot;
    public GameObject equipedSpriteRoot;

    public Transform player;


    [HideInInspector] public UnityEvent<Equipment.Item, int, MovementNode> onDropEquipment = new UnityEvent<Equipment.Item, int, MovementNode>();
    public UnityEvent onEquip = new UnityEvent();


    private void Awake()
    {
        _gridMovementController = GetComponent<GridMovementController>();
        _gridMovementController.placeOnMovementGridOnStart = false;
        _gridMovementController.OnOccupiedGridMovementRequest.AddListener(EquipToPlayer);
    }


    private void Start()
    {
        if (!IsEquiped && !stopStartMovement)
        {
            _gridMovementController.PlaceOnNearestGridNode();
            equipedSpriteRoot.SetActive(false);
            dropedSpriteRoot.SetActive(true);
        }
    }


    public void EquipToPlayer(Transform playerTransform)
    {
        PlayerEquipmentController playerEquipmentController = playerTransform.GetComponent<PlayerEquipmentController>();
        if (playerEquipmentController == null) return;
        if (!playerEquipmentController.canPickUp) return;
        if (playerEquipmentController.currentEquipment == null)
        {
            //Equip to Player
            Equip(playerEquipmentController);

        }
        else
        {
            //Replace Equipment
            playerEquipmentController.currentEquipment.DropEquipment(playerEquipmentController);
            Equip(playerEquipmentController);
        }
    }

    private void Equip(PlayerEquipmentController playerEquipmentController)
    {
        playerEquipmentController.EquipEquipment(this, equipment);
        equipedSpriteRoot.SetActive(true);
        dropedSpriteRoot.SetActive(false);
        _gridMovementController.enabled = false;
        if (_gridMovementController.currentMovementNode != null)
        {
            _gridMovementController.currentMovementNode.isOccupied = false;
        }
        IsEquiped = true;

        transform.localPosition = Vector3.zero;
        transform.localScale = new Vector3(1,1,1);
        onEquip.Invoke();
    }

    public void DropEquipment(PlayerEquipmentController playerEquipmentController)
    {
        MovementNode dropNode = playerEquipmentController.GetComponent<GridMovementController>().currentMovementNode.FindFreeNodeAround();
        if (dropNode == null) return;

        PlaceAtMovementNode(dropNode,false);

        playerEquipmentController.ResetSpriteHierachy();
        playerEquipmentController.currentEquipment = null;
        playerEquipmentController.currentItemEnum = Equipment.Item.None;

        if (playerEquipmentController.isSaveable) SaveStateManager.instance.onPlayerDropEquipment.Invoke(equipment, SceneManager.GetActiveScene().buildIndex, dropNode);
    }

    public void DropEquipmentAtNodeDirectly(PlayerEquipmentController playerEquipmentController,MovementNode node)
    {
        if (node == null) return;

        PlaceAtMovementNode(node, true);

        playerEquipmentController.ResetSpriteHierachy();
        playerEquipmentController.currentEquipment = null;
        playerEquipmentController.currentItemEnum = Equipment.Item.None;

    }

    public void PlaceAtMovementNode(MovementNode dropNode,bool immediately)
    {
        _gridMovementController.enabled = true;
        if (immediately)
        {
            _gridMovementController.MoveToTargetNodeImmediately(dropNode);
        }
        else
        {
            _gridMovementController.MoveToTargetNode(dropNode);
        }
        equipedSpriteRoot.SetActive(false);
        dropedSpriteRoot.SetActive(true);
        transform.parent = null;
        IsEquiped = false;
        transform.localScale = new Vector3(1, 1, 1);
        transform.eulerAngles = Vector3.zero;
    }
}
