using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Line 
{
    const float VERTICAL_LINE_GRADIENT = 1e5f;

    float gradient;
    float y_intercept;

    float gradientPerpendicular;

    Vector2 pointOnLine_1;
    Vector2 pointOnLine_2;

    bool approchedSide;

    public Line(Vector2 pointOnLine,Vector2 pointPerpendicularToLine)
    {
        float dx = pointOnLine.x - pointPerpendicularToLine.x;
        float dy = pointOnLine.y - pointPerpendicularToLine.y;

        if(dx == 0)
        {
            gradientPerpendicular = VERTICAL_LINE_GRADIENT;
        }
        else
        {
            gradientPerpendicular = dy / dx;
        }


        if(gradientPerpendicular == 0)
        {
            gradient = VERTICAL_LINE_GRADIENT;
        }
        else
        {
            gradient = -1 / gradientPerpendicular;
        }

        y_intercept = pointOnLine.y - gradient * pointOnLine.x;
        pointOnLine_1 = pointOnLine;
        pointOnLine_2 = pointOnLine + new Vector2(1, gradient);

        approchedSide = false;
        approchedSide = GetSide(pointPerpendicularToLine);
    }

    bool GetSide(Vector2 point)
    {
        return (point.x - pointOnLine_1.x) * (pointOnLine_2.y - pointOnLine_1.y) > (point.y - pointOnLine_1.y) * (pointOnLine_2.x - pointOnLine_1.x);
    }

    internal void DrawWithGizmos(float lenght)
    {
        Vector2 lineDIr = new Vector2(1, gradient).normalized;
        Vector2 lineCentre = new Vector2(pointOnLine_1.x, pointOnLine_1.y);
        Gizmos.DrawLine(lineCentre - lineDIr * lenght / 2f, lineCentre + lineDIr * lenght / 2f);
    }

    public bool HasCrossedLine(Vector2 point)
    {
        return GetSide(point) != approchedSide;
    }
}
