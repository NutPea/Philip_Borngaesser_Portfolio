using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NodeData", menuName = "NodeData")]
public class NodeData : ScriptableObject
{
    public List<GameObject> grasPrefabs;
    public List<GameObject> stonePrefabs;
    public List<GameObject> flowerPrefabs;
}
