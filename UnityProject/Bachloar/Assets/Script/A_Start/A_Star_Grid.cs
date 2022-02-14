using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_Star_Grid : MonoBehaviour
{
    public bool displayGridGizmos;
    public Transform player;
    public LayerMask unwalkableMask;
    LayerMask walkableMask;
    Dictionary<int, int> walkableRegionsDictonary = new Dictionary<int, int>();

    public Vector2 gridWorldSize;
    public float nodeRadius;
    A_Star_Node[,] grid;

    float nodeDiameter;
    int gridSizeX, gridSizeY;
    internal List<A_Star_Node> path;

    void Awake()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);

        GenerateGrid();
    }

    public int MaxSize
    {
        get
        {
            return gridSizeX * gridSizeY;
        }
    }

    void GenerateGrid()
    {
        grid = new A_Star_Node[gridSizeX, gridSizeY];
        Vector2 worldBottomLeft = (Vector2)transform.position - Vector2.right * gridWorldSize.x / 2 - Vector2.up * gridWorldSize.y / 2;

        for(int x = 0; x < gridSizeX; x++)
        {

            for (int y = 0; y < gridSizeY; y++)
            {
                Vector2 worldPoint = worldBottomLeft + Vector2.right * (x * nodeDiameter + nodeRadius) + Vector2.up * (y * nodeDiameter + nodeRadius);
                bool walkable = !(Physics2D.OverlapCircle(worldPoint, nodeRadius,unwalkableMask));
                int movementPenalty = 0;
                if (walkable)
                {
                    Collider2D col = Physics2D.OverlapCircle(worldPoint, nodeRadius - 0.05f,walkableMask);
                    if(col != null)
                    {
                        walkableRegionsDictonary.TryGetValue(col.gameObject.layer, out movementPenalty);
                    }


                }
                grid[x, y] = new A_Star_Node(walkable, worldPoint,x,y,movementPenalty);
            }
        }

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                if (!grid[x, y].walkable)
                {
                    grid[x, y+1].nextToUnwalkable = true;
                    grid[x,y-1].nextToUnwalkable = true;
                    grid[x+1, y+1].nextToUnwalkable = true;
                    grid[x+1, y-1].nextToUnwalkable = true;
                    grid[x-1, y+1].nextToUnwalkable = true;
                    grid[x-1, y-1].nextToUnwalkable = true;

                    grid[x+1,y].nextToUnwalkable = true;
                    grid[x-1,y].nextToUnwalkable = true;
                }
            }
        }

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                if (grid[x, y].nextToUnwalkable)
                {
                    grid[x, y].walkable = false;
                }
            }
        }
    }


    public A_Star_Node WordToNode(Vector2 worldPos)
    {
        float percentX = (worldPos.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPos.y + gridWorldSize.y / 2) / gridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);

        return grid[x, y];
    }

    public List<A_Star_Node> GetNeigboures(A_Star_Node node)
    {
        List<A_Star_Node> neighbours = new List<A_Star_Node>();
        for(int x = -1; x<= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue;

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;
                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbours;
    }

    private void OnDrawGizmos()
    {

        if (grid != null && displayGridGizmos)
        {
        Gizmos.DrawWireCube(transform.position, new Vector2(gridWorldSize.x, gridWorldSize.y));
            foreach (A_Star_Node n in grid)
            {
                if (n.walkable)
                {
                    Color w = Color.white;
                    w.a = 0.6f;
                    Gizmos.color = w;
                }
                else
                {
                    Color r = Color.blue;
                    r.a = 0.6f;
                    Gizmos.color = r;
                }
                Gizmos.DrawCube(n.worldPos, new Vector3(1, -1, 1) * (nodeDiameter - 0.1f));
            }
        }
    }

    [System.Serializable]
    public class TerrainType
    {
        public LayerMask terrainMask;
        public int terrainPenalty;
    }
}
