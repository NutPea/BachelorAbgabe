using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_Star_Node :IHeapItem<A_Star_Node>
{
    public bool walkable;
    public bool nextToUnwalkable;
    public Vector2 worldPos;
    public int gridX;
    public int gridY;
    public int movementPenalty;


    public int g_Cost;
    public int h_Cost;
    public A_Star_Node parent;
    int heapIndex;

    public A_Star_Node(bool walkable , Vector2 worldPos,int gridX,int gridY,int movementPenalty)
    {
        this.walkable = walkable;
        this.worldPos = worldPos;

        this.gridX = gridX;
        this.gridY = gridY;
        this.movementPenalty = movementPenalty;
    }

    public int f_Cost
    {
        get
        {
            return g_Cost + h_Cost;
        }
    }

    public int HeapIndex { get => heapIndex; set => heapIndex = value; }


    public int CompareTo(A_Star_Node otherNote)
    {
        int compare = f_Cost.CompareTo(otherNote.f_Cost);
        if(compare == 0)
        {
            compare = h_Cost.CompareTo(otherNote.h_Cost);
        }
        return -compare;
    }
}
