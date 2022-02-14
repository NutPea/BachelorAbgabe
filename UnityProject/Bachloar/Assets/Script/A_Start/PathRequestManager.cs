using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PathRequestManager : MonoBehaviour
{
    Queue<PathRequest> pathRequestQueue = new Queue<PathRequest>();
    PathRequest currentPathRequest;

    public static PathRequestManager instance;
    A_Star_Pathfinding pathfinding;

    bool isProzessingPath;

    private void Awake()
    {
        instance = this;
        pathfinding = GetComponent<A_Star_Pathfinding>();
    }

    public static void RequestPath(Vector2 pathStart, Vector2 pathEnd,Action<Vector2[],bool> callback)
    {
        PathRequest newRequest = new PathRequest(pathStart, pathEnd, callback);
        instance.pathRequestQueue.Enqueue(newRequest);
        instance.TryProcessNext();
    }

    private void TryProcessNext()
    {
        if(!isProzessingPath && pathRequestQueue.Count > 0)
        {
            currentPathRequest = pathRequestQueue.Dequeue();
            isProzessingPath = true;
            pathfinding.StartFindPath(currentPathRequest.start, currentPathRequest.pathEnd);
        }
    }

    public void FinishedProcessingPath(Vector2[] path,bool success)
    {
        currentPathRequest.callback(path, success);
        isProzessingPath = false;
        TryProcessNext();
    }

    public struct PathRequest
    {
        public Vector2 start;
        public Vector2 pathEnd;
        public Action<Vector2[], bool> callback;

        public PathRequest(Vector2 start,Vector2 end,Action<Vector2[],bool> callback)
        {
            this.start = start;
            this.pathEnd = end;
            this.callback = callback;
        }
    }
}
