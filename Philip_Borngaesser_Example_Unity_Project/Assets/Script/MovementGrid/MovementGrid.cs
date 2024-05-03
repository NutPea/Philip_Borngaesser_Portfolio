using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MovementGrid : MonoBehaviour
{

    public static MovementGrid instance;
    public Vector2 gridSize;
    public float movementNodeSize;
    public LayerMask notWalkableLayerMask;
    public LayerMask sceneTransitionLayerMask;

    [Header("Grid")]
    public Grid grid;
    public bool hasBeenGenerated;
    private int gridSizeX;
    private int gridSizeY;
    public bool showGizmo;

    public int MaxGridSize { get {
            return (int)(gridSize.x * gridSize.y);
        }
    }

    [System.Serializable]
    public class Grid
    {
        public List<GridRow> colum = new List<GridRow>();
    }

    [System.Serializable]
    public class GridRow
    {
        public string rowNumber;
        public GridRow(string rowNumber)
        {
            this.rowNumber = rowNumber;
        }
        public List<MovementNode> row = new List<MovementNode>();
    }


    public void Awake()
    {
        showGizmo = false;
        GenerateMovementGrid();
        instance = this;
    }

    public void GenerateMovementGrid()
    {
        if (hasBeenGenerated == true) return;
        hasBeenGenerated = true;
        gridSizeX = Mathf.RoundToInt(gridSize.x / movementNodeSize);
        gridSizeY = Mathf.RoundToInt(gridSize.y / movementNodeSize);


        //Start Left Down 
        float xStartPosition = -(movementNodeSize/2 * (int)gridSize.x) + transform.position.x;
        float yStartPosition = -(movementNodeSize / 2 * (int)gridSize.y) + transform.position.y;
        float xPosition = xStartPosition;
        float yPosition = yStartPosition;

        for(int y = 0; y < gridSize.y; y++)
        {
            grid.colum.Add(new GridRow("Row " + y.ToString()));

            for(int x = 0; x < gridSize.x; x++)
            {
                GameObject targetNodeGameobject = new GameObject();
                targetNodeGameobject.name = "MovementNode(" + x + " , " + y +")";
                targetNodeGameobject.transform.parent = transform;
                MovementNode targetNode = targetNodeGameobject.AddComponent<MovementNode>();
                grid.colum[y].row.Add(targetNode);

                //movementGrid[x, y] = targetNode;
                targetNode.gridX = x;
                targetNode.gridY = y;
                //IsWalkable or Not
                targetNodeGameobject.transform.position = new Vector3(xPosition, yPosition, 0);
                xPosition += movementNodeSize;
                
            }
            xPosition = xStartPosition;
            yPosition += movementNodeSize;
        }


        for(int y = 0; y < gridSize.y; y++)
        {
            for(int x = 0; x<gridSize.x; x++)
            {
                int leftIndex = x - 1;
                int rightIndex = x + 1;
                int downIndex = y - 1;
                int upIndex = y + 1;
                //MovementNode targetNode =  movementGrid[x, y];
                MovementNode targetNode = grid.colum[y].row[x];

                bool walkable = !(Physics2D.OverlapCircle(targetNode.transform.position, movementNodeSize * 0.33f, notWalkableLayerMask));
                targetNode.isWalkable = walkable;

                if (leftIndex >= 0) targetNode.leftNeighbor = grid.colum[y].row[leftIndex];
                if (rightIndex < gridSize.x) targetNode.rightNeighbor = grid.colum[y].row[rightIndex];
                if (downIndex >= 0) targetNode.downNeighbor = grid.colum[downIndex].row[x];
                if (upIndex < gridSize.y) targetNode.upNeighbor = grid.colum[upIndex].row[x];
            }
        }
    }


    public void SetTransformToGrid(Transform value)
    {
        MovementNode node = WorldToMovementNode(value.position);
        value.transform.position = node.transform.position;
    }

    

    public MovementNode WorldToMovementNode(Vector3 worldPos)
    {

        MovementNode movementNode = new MovementNode();
        for (int y = 0; y < gridSize.y-1; y++)
        {
            for (int x = 0; x < gridSize.x-1; x++)
            {

                MovementNode obbservedNode = grid.colum[y].row[x];
                if (Vector2.Distance(obbservedNode.transform.position,worldPos) < movementNodeSize/2 - 0.01f)
                {
                    movementNode = obbservedNode;
                    break;
                }
            }
        }

        return movementNode;
    }


    public MovementNode GetMovementNode(Vector2 gridPosition)
    {
        return grid.colum[(int)gridPosition.x].row[(int)gridPosition.y];
    }
    public void ClearList()
    {
        showGizmo = false;

        for (int i = 0; i < 10;i++) {
            foreach (GridRow row in grid.colum)
            {
                foreach (MovementNode node in row.row)
                {
                    DestroyImmediate(node.gameObject);
                }
            }

            foreach (Transform child in transform)
            {
                DestroyImmediate(child.gameObject);
            }
            grid = new Grid();
        }


    }

    public List<MovementNode> GetWalkableMovementNodes(bool isWalkable)
    {
        List<MovementNode> nodeList = new List<MovementNode>();
        foreach(GridRow colum in grid.colum)
        {
            foreach(MovementNode node in colum.row)
            {
                if(node.isWalkable == isWalkable)
                {
                    nodeList.Add(node);
                }
            }
        }
        return nodeList;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if(showGizmo == true)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                for (int x = 0; x < gridSize.x; x++)
                {
                    MovementNode targetNode = grid.colum[y].row[x];
                    Gizmos.color = Color.gray;
                    Gizmos.DrawWireCube(new Vector3(targetNode.transform.position.x, targetNode.transform.position.y, 0), new Vector3(movementNodeSize-0.01f, movementNodeSize-0.01f, 0));

                    if (targetNode.isWalkable && !targetNode.isOccupied) Gizmos.color = Color.green;
                    else if (targetNode.isOccupied) Gizmos.color = Color.yellow;
                    else Gizmos.color = Color.red;
                    Handles.Label(targetNode.transform.position,"("+ x+","+y+")");

                    Gizmos.DrawWireCube(new Vector3(targetNode.transform.position.x, targetNode.transform.position.y, 0), new Vector3(movementNodeSize / 2 - 0.01f, movementNodeSize / 2 - 0.01f, 0));
                }
            }
        }
    }
#endif

}
