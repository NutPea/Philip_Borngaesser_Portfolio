using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnableSaveHandler : MonoBehaviour
{
    public List<SpawnableItems> spawnableItems;
    private void Start()
    {
        List<SpawnableSaveState> saveStates = SaveStateManager.instance.GetAvaibleSaveStates(SceneManager.GetActiveScene().buildIndex);
        foreach(SpawnableSaveState state in saveStates)
        {
            SpawnItem(state);
        }
    }

    void SpawnItem(SpawnableSaveState state)
    {
        Vector3 spawnPosition = Vector3.zero;
        spawnPosition.x = state.xPosition;
        spawnPosition.y = state.yPosition;
        spawnPosition.z = state.zPosition;

        GameObject toSpawnObject = FindPrefab(state);

        GameObject prefab = Instantiate(toSpawnObject, spawnPosition, Quaternion.identity);
        SpawnableSaver saver = prefab.GetComponent<SpawnableSaver>();
        saver.usedKeySaveState = state;

        GridMovementController prefabMovementController = prefab.GetComponent<GridMovementController>();
        prefabMovementController.PlaceOnNearestGridNode();
    }

    GameObject FindPrefab(SpawnableSaveState state)
    {
        GameObject foundObject = spawnableItems[0].spawnAblePrefab;
        foreach(SpawnableItems items in spawnableItems)
        {
            if(items.spawnableItem == state.item)
            {
                foundObject = items.spawnAblePrefab;
            }
        }
        return foundObject;
    }


}
[System.Serializable]
public class SpawnableItems
{
    public Spawnables.Item spawnableItem;
    public GameObject spawnAblePrefab;
}