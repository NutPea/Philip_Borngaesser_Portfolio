#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Events;
using UnityEngine;
using UnityEngine.Events;

[CustomEditor(typeof(MovementGridDekorater))]
public class MovementGridDekoraterEditor : Editor
{
    bool hasBeenInit = false;
    NodeData data;

    int walkableChoise = 0;
    string[] walkableOptions = new string[]
    {
        "Walkable", "NotWalkable",
    };

    int dekorationChoise = 0;

    string[] dekorations = new string[]
    {
        "Gras", "Stone", "Flower"
    };


    public override void OnInspectorGUI()
    {
        if (!hasBeenInit)
        {
            if(data == null) data = (NodeData)AssetDatabase.LoadAssetAtPath("Assets/Art/Node/NodeData/NodeData.asset", typeof(NodeData));
            hasBeenInit = true;
        }

        base.OnInspectorGUI();
        walkableChoise = EditorGUILayout.Popup("Spawn on walkable/not walkable", walkableChoise, walkableOptions);
        dekorationChoise = EditorGUILayout.Popup("Choose Dekorations", dekorationChoise, dekorations);

        if (GUILayout.Button("GenerateDekoration"))
        {
            HandleSpawn();
        }

        if (GUILayout.Button("Remove choosen Childs"))
        {
            RemoveAllDeko();
        }
    }


    private void HandleSpawn()
    {
        MovementGridDekorater gridDeko = target as MovementGridDekorater;
        MovementGrid grid = gridDeko.GetComponent<MovementGrid>();
        bool isWalkable = walkableChoise == 0;
        List<MovementNode> nodes = grid.GetWalkableMovementNodes(isWalkable);
        float positionRange = grid.movementNodeSize/2;

        foreach(MovementNode node in nodes)
        {
            int randomeSpawnAmount = UnityEngine.Random.Range(gridDeko.minSpawnAmount, gridDeko.maxSpawnAmount);

            for (int index = 0; index < randomeSpawnAmount; index++)
            {
                float randomeXPos = UnityEngine.Random.Range(-positionRange, positionRange);
                float randomeYPos = UnityEngine.Random.Range(-positionRange, positionRange);
                Vector3 newPos = node.transform.position + new Vector3(randomeXPos, randomeYPos, 0);
                Vector3 dir = newPos - node.transform.position;
                bool isRight = dir.x < 0;
                bool isFlipped = UnityEngine.Random.Range(0.0f, 1.0f) < 0.5f;

                float scaleX = UnityEngine.Random.Range(gridDeko.minScaleDistance.x, gridDeko.maxScaleDistance.x);
                float scaleY = UnityEngine.Random.Range(gridDeko.minScaleDistance.y, gridDeko.maxScaleDistance.y);

                float xPercentage = 1 - (Math.Abs((node.transform.position.x - newPos.x)) / positionRange);
                float yPercentage = 1 - (Math.Abs((node.transform.position.y - newPos.y)) / positionRange);

                float percentage = (xPercentage + yPercentage) / 2;

                switch (dekorationChoise)
                {
                    case 0:
                        GameObject spawnedGrasPrefab = (GameObject)PrefabUtility.InstantiatePrefab(data.grasPrefabs[UnityEngine.Random.Range(0, data.grasPrefabs.Count)]);
                        SetDekoAnim(node, newPos, isRight, isFlipped, scaleX, scaleY, xPercentage, yPercentage, percentage, spawnedGrasPrefab);
                        break;
                    case 1:
                        GameObject spawnedStonePrefab = (GameObject)PrefabUtility.InstantiatePrefab(data.stonePrefabs[UnityEngine.Random.Range(0, data.stonePrefabs.Count)]);
                        SetDekoAnim(node, newPos, isRight, isFlipped, scaleX, scaleY, xPercentage, yPercentage, percentage, spawnedStonePrefab);
                        break;
                    case 2:
                        GameObject spawnedFlowerPrefab = (GameObject)PrefabUtility.InstantiatePrefab(data.flowerPrefabs[UnityEngine.Random.Range(0, data.flowerPrefabs.Count)]);
                        SetDekoAnim(node, newPos, isRight, isFlipped, scaleX, scaleY, xPercentage, yPercentage, percentage, spawnedFlowerPrefab);
                        break;
                }
            }
        }
        hasBeenInit = false;
    }

    private  void SetDekoAnim(MovementNode node, Vector3 newPos, bool isRight, bool isFlipped, float scaleX, float scaleY, float xPercentage, float yPercentage, float percentage, GameObject spawnedPrefab)
    {
        DekoAnim deko = spawnedPrefab.GetComponent<DekoAnim>();
        deko.observedNode = node;
        deko.xPercentage = xPercentage;
        deko.yPercentage = yPercentage;
        deko.percentage = percentage;

        UnityAction<bool> methodDelegate = System.Delegate.CreateDelegate(typeof(UnityAction<bool>), deko, "PlayAnim") as UnityAction<bool>;
        UnityEventTools.AddPersistentListener(node.onArrivedAtNode, methodDelegate);

        deko.isRight = isRight;
        spawnedPrefab.transform.parent = node.transform;
        if (isFlipped) scaleX = -scaleX;

        spawnedPrefab.transform.localScale = new Vector3(scaleX, scaleY, 1);
        spawnedPrefab.transform.position = newPos;
    }

    private void RemoveAllDeko()
    {
        MovementGridDekorater gridDeko = target as MovementGridDekorater;
        MovementGrid grid = gridDeko.GetComponent<MovementGrid>();
        bool isWalkable = walkableChoise == 0;
        List<MovementNode> nodes = grid.GetWalkableMovementNodes(isWalkable);
        foreach (MovementNode node in nodes)
        {
            for (int index = 0; index < 10; index++)
            {
                foreach (Transform child in node.transform)
                {
                    DestroyImmediate(child.gameObject);
                }
            }
        }
    }
    

}
#endif
