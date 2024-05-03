using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnOnEvent : MonoBehaviour
{
    public GameObject spawnablePrefab;
    public bool destoryAfterTime;
    public float destroyAfterTimeTimer;
    public void _SpawnImidiatly()
    {
        GameObject prefab = Instantiate(spawnablePrefab, transform.position, Quaternion.identity);
        if (destoryAfterTime)
        {
            Destroy(prefab, destroyAfterTimeTimer);
        }
    }
}
