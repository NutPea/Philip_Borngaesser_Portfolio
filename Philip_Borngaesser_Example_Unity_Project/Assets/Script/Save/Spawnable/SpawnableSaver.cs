using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnableSaver : MonoBehaviour
{
    public Spawnables.Item spawnableItem;
    public bool canNotBeSaved;
    public SpawnableSaveState usedKeySaveState;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(0.5f);
        usedKeySaveState = SaveStateManager.instance.SaveSpawnable(this);
    }

    public void RemoveSpawnable()
    {
        SaveStateManager.instance.spawnableSaveStates.Remove(usedKeySaveState);
    }
}
