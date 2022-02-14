using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_Star_Pathfinding : MonoBehaviour
{

    PathRequestManager requestManager;
    public A_Star_Grid grid;

    private void Awake()
    {
        requestManager = GetComponent<PathRequestManager>();
    }

    private void Update()
    {
        
    }
    IEnumerator FindPath(Vector2 startPos,Vector2 targetPos)
    {
        Vector2[] waypoints = new Vector2[0];
        bool pathSuccess = false;

        A_Star_Node startNode = grid.WordToNode(startPos);
        A_Star_Node targetNode = grid.WordToNode(targetPos);

        if (startNode.walkable && targetNode.walkable)
        {

            Heap<A_Star_Node> openSet = new Heap<A_Star_Node>(grid.MaxSize);

            HashSet<A_Star_Node> closedSet = new HashSet<A_Star_Node>();
            openSet.Add(startNode);
            while (openSet.Count > 0)
            {
                A_Star_Node currentNode = openSet.RemoveFirst();
                closedSet.Add(currentNode);

                if (currentNode == targetNode)
                {
                    //PathFound;
                    pathSuccess = true;
                    break;
                }

                foreach (A_Star_Node neighbour in grid.GetNeigboures(currentNode))
                {
                    if (!neighbour.walkable || closedSet.Contains(neighbour))
                    {
                        continue;
                    }
                    int newMovementCost = currentNode.g_Cost + GetDistance(currentNode, neighbour) + neighbour.movementPenalty;
                    if (newMovementCost < neighbour.g_Cost || !openSet.Contains(neighbour))
                    {
                        neighbour.g_Cost = newMovementCost;
                        neighbour.h_Cost = GetDistance(neighbour, targetNode);
                        neighbour.parent = currentNode;

                        if (!openSet.Contains(neighbour))
                        {
                            openSet.Add(neighbour);
                        }
                        else
                        {
                            openSet.UpdateItem(neighbour);
                        }
                    }
                }

            }
        }
        yield return null;
        if (pathSuccess)
        {
            waypoints = RetracePath(startNode, targetNode);
            pathSuccess = waypoints.Length > 0;
        }
        requestManager.FinishedProcessingPath(waypoints, pathSuccess);
    }

    public void StartFindPath(Vector2 start, Vector2 pathEnd)
    {
        StartCoroutine(FindPath(start, pathEnd));
    }

    Vector2[] RetracePath(A_Star_Node startNode,A_Star_Node endNode)
    {
        List<A_Star_Node> path = new List<A_Star_Node>();
        A_Star_Node currentNode =endNode;

        while(currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        Vector2[] waypoints = SimplerPath(path);
        Array.Reverse(waypoints);
        return waypoints;
    }

    Vector2[] SimplerPath(List<A_Star_Node> path )
    {
        List<Vector2> waypoints = new List<Vector2>();
        Vector2 directionOld = Vector2.zero;

        for(int i = 1; i < path.Count; i++)
        {
            Vector2 directionNew = new Vector2(path[i - 1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY);
            if(directionNew != directionOld)
            {
                waypoints.Add(path[i].worldPos);
            }
            directionOld = directionNew;
        }
        return waypoints.ToArray();
    }

    int GetDistance(A_Star_Node nodeA , A_Star_Node nodeB)
    {
        int distX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int distY = Mathf.Abs(nodeA.gridY - nodeB.gridY);
        if(distX > distY)
        {
            return 14 * distY + 10 * (distX - distY);
        }
        else
        {
            return 14 * distX + 10 * (distY - distX);
        }

    }
}
