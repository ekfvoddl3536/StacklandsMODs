// The MIT License (MIT)
// Copyright (c) 2023-2026. Super Comic (ekfvoddl3535@naver.com)

using UnityEngine;

namespace SmartFactory;

internal static unsafe class NetworkArrows
{
    [SkipLocalsInit, MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Vector2 GetPointOnCardEdge(in Vector2 start, in Vector2 end, in Bounds bounds)
    {
        const int LENGTH = 4;

        var boundsMin = bounds.min;
        var boundsMax = bounds.max;

        Vector2* corners = stackalloc Vector2[LENGTH];

        corners[0] = new Vector2(boundsMin.x, boundsMin.z);
        corners[1] = new Vector2(boundsMax.x, boundsMin.z);
        corners[2] = new Vector2(boundsMax.x, boundsMax.z);
        corners[3] = new Vector2(boundsMin.x, boundsMax.z);

        for (int i = 0; i < LENGTH; ++i)
        {
            var c1 = corners[i];
            var c2 = corners[(i + 1) & 3];

            if (MathHelper.LineSegmentsIntersection(start, end, c1, c2, out var intersection, out var _))
                return intersection;
        }

        return start;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 TransformToEdge(in Vector3 start, in Vector3 end, in Bounds bounds)
    {
        const float K_EXTRA_SIDE_DISTANCE = 0.01f;

        var v2_start = new Vector2(start.x, start.z);
        var v2_end = new Vector2(end.x, end.z);

        var pointOnCardEdge = GetPointOnCardEdge(v2_start, v2_end, bounds);

        return
            new Vector3(pointOnCardEdge.x, 0f, pointOnCardEdge.y) +
            (start - end).normalized *
            K_EXTRA_SIDE_DISTANCE;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void DrawArrow(CardData from, CardData to)
    {
        var fromPosV3 = from.MyGameCard.transform.position;
        var toPosV3 = to.transform.position;

        var end = TransformToEdge(fromPosV3, toPosV3, to.MyGameCard.GetBounds());
        var edge = TransformToEdge(fromPosV3, end, from.MyGameCard.GetBounds());

        DrawManager.instance.DrawShape(new ConveyorArrow
        {
            Start = edge,
            End = end,
            // Color = Color.red,
        });
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void DrawArrow(CardData from, Vector3 to)
    {
        var pos = from.MyGameCard.transform.position;

        var end = TransformToEdge(pos, to, new Bounds(to, new Vector3(0.5f, 0, 0.5f)));
        var edge = TransformToEdge(pos, end, from.MyGameCard.GetBounds());

        DrawManager.instance.DrawShape(new ConveyorArrow
        {
            Start = edge,
            End = end
        });
    }

    internal static void ReDraw(LogicBase me)
    {
        var outputs = me.OutputNodes;
        for (int i = 0; i < outputs.Length; ++i)
            if (outputs[i] != (object)null)
                DrawArrow(me, outputs[i]);
    }

    // WIP
    internal static void OnUpdateArrowToMouse(LogicBase _)
    {
        // if (fromCard == (object)null ||
        //     fromCard == (object)WorldManager.instance.HoveredDraggable)
        //     return;
        // 
        // DrawArrow(fromCard, WorldManager.instance.mouseWorldPosition);
    }
}