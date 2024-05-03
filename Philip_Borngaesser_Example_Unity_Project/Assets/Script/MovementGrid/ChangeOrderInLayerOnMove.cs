using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GridMovementController))]
public class ChangeOrderInLayerOnMove : MonoBehaviour
{
    private GridMovementController _gridMovementController;
    public List<int> startOrderInLayer = new List<int>();
    public List<SpriteRenderer> spriteRenderers = new List<SpriteRenderer>();

    public int onMoveOrderInLayerAdder = 400;
    

    void Awake()
    {
        _gridMovementController = GetComponent<GridMovementController>();
        _gridMovementController.OnGridMovementStart.AddListener(OnChangeOrderInLayer);
        _gridMovementController.OnGridMovementEnd.AddListener(OnResetOrderInLayer);
        _gridMovementController.OnGridInteractStart.AddListener(OnChangeOrderInLayer);
        _gridMovementController.OnGridInteractEnd.AddListener(OnChangeOrderInLayer);
    }

    private void OnChangeOrderInLayer(MovementNode arg0)
    {
        OnChangeOrderInLayer();
    }

    private void Start()
    {
        spriteRenderers = new List<SpriteRenderer>(GetComponentsInChildren<SpriteRenderer>());
        foreach(SpriteRenderer renderer in spriteRenderers)
        {
            startOrderInLayer.Add(renderer.sortingOrder);
        }
    }

    private void OnResetOrderInLayer()
    {
        for(int index = 0; index < spriteRenderers.Count; index++)
        {
            spriteRenderers[index].sortingOrder = startOrderInLayer[index];
        }

    }

    private void OnChangeOrderInLayer()
    {
        for (int index = 0; index < spriteRenderers.Count; index++)
        {
            spriteRenderers[index].sortingOrder = startOrderInLayer[index] + onMoveOrderInLayerAdder;
        }
    }


}
