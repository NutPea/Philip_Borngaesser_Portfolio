using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PickUpSaveHandler : MonoBehaviour
{
    public float xRotation = -30f;
    public List<PickUpableItems> pickUpAbleItems;
    private void Start()
    {
        List<PickUpSaveState> saveStates = SaveStateManager.instance.FindPickUpSaveStatesDependingOnScene(SceneManager.GetActiveScene().buildIndex);
        foreach (PickUpSaveState state in saveStates)
        {
            SpawnItem(state);
        }
    }

    void SpawnItem(PickUpSaveState state)
    {
        Vector3 spawnPosition = Vector3.zero;
        spawnPosition.x = state.xPosition;
        spawnPosition.y = state.yPosition;
        spawnPosition.z = state.zPosition;

        GameObject toSpawnObject = FindPrefab(state);

        GameObject prefab = Instantiate(toSpawnObject, spawnPosition, Quaternion.identity);
        PickUpSaver saver = prefab.GetComponent<PickUpSaver>();
        saver.GUID = state.GUID;


        GridMovementController prefabMovementController = prefab.GetComponent<GridMovementController>();
        prefabMovementController.PlaceOnNearestGridNode();

        prefab.transform.Rotate(new Vector3(-30, 0, 0), Space.Self);
    }

    void RotateAfterSpawn(Transform transform)
    {

    }

    GameObject FindPrefab(PickUpSaveState state)
    {
        GameObject foundObject = pickUpAbleItems[0].spawnAblePrefab;
        foreach (PickUpableItems items in pickUpAbleItems)
        {
            if (items.spawnablePickUpableItem == state.pickUpable)
            {
                foundObject = items.spawnAblePrefab;
            }
        }
        return foundObject;
    }
}

[System.Serializable]
public class PickUpableItems
{
    public Pickupable.Item spawnablePickUpableItem;
    public GameObject spawnAblePrefab;
}
