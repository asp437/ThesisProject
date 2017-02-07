using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class RoadHelper
{
    public static float crossroadRoadOffset = 0.1f;

    // Line segment intersection algorithm by Bryce Boe
    // More info: http://bryceboe.com/2006/10/23/line-segment-intersection-algorithm/
    protected static bool ccw(Crossroad A, Crossroad B, Crossroad C)
    {
        return (C.y - A.y) * (B.x - A.x) > (B.y - A.y) * (C.x - A.x);
    }

    protected static bool ccw(Vector2 A, Vector2 B, Vector2 C)
    {
        return (C.y - A.y) * (B.x - A.x) > (B.y - A.y) * (C.x - A.x);
    }

    public static bool areLinesIntersects(Vector2 a, Vector2 b, Vector2 c, Vector2 d)
    {
        return ccw(a, c, d) != ccw(b, c, d) && ccw(a, b, c) != ccw(a, b, d);
    }

    public static bool areRoadsIntersects(RoadSegment rs0, RoadSegment rs1)
    {
        bool lineIntersection = ccw(rs0.start, rs1.start, rs1.end) != ccw(rs0.end, rs1.start, rs1.end) && ccw(rs0.start, rs0.end, rs1.start) != ccw(rs0.start, rs0.end, rs1.end);
        if (lineIntersection)
            return true;
        Vector2[] rs0StartPoints = getRoadOffsetVectors(rs0.start, rs0.end, rs0.width);
        Vector2[] rs0EndPoints = getRoadOffsetVectors(rs0.end, rs0.start, rs0.width);
        Vector2[] rs1StartPoints = getRoadOffsetVectors(rs1.start, rs1.end, rs1.width);
        Vector2[] rs1EndPoints = getRoadOffsetVectors(rs1.end, rs1.start, rs1.width);

        for (int rs0SPoint = 0; rs0SPoint < rs0StartPoints.Length; rs0SPoint++)
            for (int rs0EPoint = 0; rs0EPoint < rs0EndPoints.Length; rs0EPoint++)
                for (int rs1SPoint = 0; rs1SPoint < rs1StartPoints.Length; rs1SPoint++)
                    for (int rs1EPoint = 0; rs1EPoint < rs1EndPoints.Length; rs1EPoint++)
                        if (areLinesIntersects(rs0StartPoints[rs0SPoint], rs0EndPoints[rs0EPoint], rs1StartPoints[rs1SPoint], rs1EndPoints[rs1EPoint]))
                            return true;

        return false;
    }

    public static Vector2[] getRoadOffsetVectors(Crossroad main, Crossroad other, float width)
    {
        Vector2[] result = new Vector2[2];
        Vector3 pointA = new Vector3(main.x, 0, main.y);
        Vector3 pointB = new Vector3(other.x, 0, other.y);

        Vector3 segvec = (pointA - pointB).normalized;
        pointA -= segvec * crossroadRoadOffset * 0.5f;
        pointA += segvec * crossroadRoadOffset * 0.5f;

        Vector3 per = Vector3.Cross(pointA - pointB, Vector3.down).normalized;

        // Use pointB as a temporary variable
        pointB = pointA + per * (0.5f * width);
        result[0] = new Vector2(pointB.x, pointB.z);

        pointB = pointA - per * (0.5f * width);
        result[1] = new Vector2(pointB.x, pointB.z);

        return result;
    }

    public static Vector2 getIntersectionPoint(Crossroad a, Crossroad b, Crossroad c, Crossroad d)
    {
        float x12 = a.x - b.x;
        float x34 = c.x - d.x;
        float y12 = a.y - b.y;
        float y34 = c.y - d.y;

        float f = x12 * y34 - y12 * x34;
        if (Math.Abs(f) < 0.01)
        {
            return Vector2.zero;
        }
        else
        {
            float tmp0 = a.x * b.y - a.y * b.x;
            float tmp1 = c.x * d.y - c.y * d.x;

            return new Vector2((tmp0 * x34 - tmp1 * x12) / f, (tmp0 * y34 - tmp1 * y12) / f);
        }
    }

    public static bool isUnderWaterline(Crossroad crossroad, CityGenerator cityGenerator)
    {
        return cityGenerator.getPointHeight(crossroad.x, crossroad.y) <= cityGenerator.terrainGenerator.meshGenerator.scaleHeight(cityGenerator.waterlineHeight);
    }

    public static float getSegmentSlope(Crossroad cr0, Crossroad cr1, CityGenerator cityGenerator)
    {
        float startHeight = cityGenerator.getPointHeight(cr0.x, cr0.y);
        float endHeight = cityGenerator.getPointHeight(cr1.x, cr1.y);
        float segmentLength = (float)Math.Sqrt(Math.Pow(Math.Abs(cr0.x - cr1.x), 2) + Math.Pow(Math.Abs(cr0.y - cr1.y), 2));
        float slope = (float)(Math.Atan(Math.Abs(startHeight - endHeight) / segmentLength) * (180.0f / Math.PI));
        return float.IsNaN(slope) ? 0.0f : slope;
    }

    public static bool hasRoadAt(RoadNetwork roadNetwork, float x1, float y1, float x2, float y2)
    {
        if (x1 > x2)
        {
            float tmp = x2;
            x2 = x1;
            x1 = tmp;
        }
        if (y1 > y2)
        {
            float tmp = y2;
            y2 = y1;
            y1 = tmp;
        }
        foreach (RoadSegment segment in roadNetwork.roadSegments)
        {
            if (segment.start.x == segment.end.x && x1 == x2 && x1 == segment.start.x) // Road segment and tested segment are horizontal
            {
                if (segment.start.y <= y1 && segment.end.y >= y2 ||
                    segment.start.y >= y1 && segment.end.y <= y2)
                    return true;
            }
            else if (segment.start.y == segment.end.y && y1 == y2 && y1 == segment.start.y) // Road segment and tested segment are vertical
            {
                if (segment.start.x <= x1 && segment.end.x >= x2 ||
                    segment.start.x >= x1 && segment.end.x <= x2)
                    return true;
            }
        }
        return false;
    }
}
