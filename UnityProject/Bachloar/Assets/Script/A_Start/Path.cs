using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path 
{
    public readonly Vector2[] lookPoints;
    public readonly Line[] turnBounderies;
    public readonly int finisedLineIndex;

    public Path(Vector2[] waypoints,Vector2 startPos,float turnDst)
    {
        lookPoints = waypoints;
        turnBounderies = new Line[lookPoints.Length];
        finisedLineIndex = turnBounderies.Length - 1;

        Vector2 prewPoint = startPos;
        for(int i = 0; i < lookPoints.Length; i++)
        {
            Vector2 currentPoint = lookPoints[i];
            Vector2 dirToCurrentPoint = (currentPoint - prewPoint).normalized;
            Vector2 turnBoundaryPoint = (i == finisedLineIndex) ? currentPoint : currentPoint - dirToCurrentPoint * turnDst;
            turnBounderies[i] = new Line(turnBoundaryPoint, prewPoint-dirToCurrentPoint * turnDst);
            prewPoint = turnBoundaryPoint;
        }

    }

    public void DrawWithGizmos()
    {
        Gizmos.color = Color.black;
        foreach(Vector2 p in lookPoints)
        {
            Gizmos.DrawCube(p, Vector3.one);
        }

        Gizmos.color = Color.white;
        foreach(Line l in turnBounderies)
        {
            l.DrawWithGizmos(10);
        }
    }
  
}
