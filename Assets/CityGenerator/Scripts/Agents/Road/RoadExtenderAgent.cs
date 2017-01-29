using UnityEngine;
using System.Collections;

public class RoadExtenderAgent : AbstractAgent
{
    private const float scale = 1.0f;
    public override void agentAction()
    {
        RoadNetwork network = generator.roadNetwork;
        Crossroad extensionOrigin = network.crossroads[Random.Range(0, network.crossroads.Count - 1)];
        Vector2 extension;
        Vector2 direction;
        RoadSegment segment;
        switch (Random.Range(0, 3))
        {
            case 0: // Right
                direction = new Vector2(0, 1);
                break;
            case 1: // Left
                direction = new Vector2(0, -1);
                break;
            case 2: // Up
                direction = new Vector2(1, 0);
                break;
            case 3: // Down
            default:
                direction = new Vector2(-1, 0);
                break;
        }
        extension = direction * ((int)Random.Range(1.0f, 4.0f)) * scale;

        for (int i = 0; i < extensionOrigin.adjacentSegemnts.Count; i++)
        {
            Crossroad anotherCr = extensionOrigin.adjacentSegemnts[i].end.Equals(extensionOrigin) ? extensionOrigin.adjacentSegemnts[i].start : extensionOrigin.adjacentSegemnts[i].end;
            Vector2 v0 = new Vector2(extension.x, extension.y), v1 = new Vector2(anotherCr.x - extensionOrigin.x, anotherCr.y - extensionOrigin.y);
            if (Mathf.Abs(Vector2.Angle(v0, v1)) < 10.0f || Mathf.Abs(Vector2.Angle(v0, v1)) > 350.0f)
                return;
        }

        Crossroad testCr = new Crossroad();
        testCr.x = extensionOrigin.x + extension.x;
        testCr.y = extensionOrigin.y + extension.y;
        for (int i = 0; i < network.roadSegments.Count; i++)
        {
            if (network.roadSegments[i].start == extensionOrigin || network.roadSegments[i].end == extensionOrigin)
                continue;
            if (RoadHelper.areRoadsIntersects(new RoadSegment(extensionOrigin, testCr), network.roadSegments[i]))
                return;
        }

        Crossroad cr1 = new Crossroad(extensionOrigin.x + extension.x, extensionOrigin.y + extension.y);
        if (cr1.x > generator.meshDimension - 1 || cr1.x < 1 || cr1.y > generator.meshDimension - 1 || cr1.y < 1)
            return;
        if (RoadHelper.isUnderWaterline(cr1, generator) || RoadHelper.getSegmentSlope(extensionOrigin, cr1, generator) >= generator.maximumSlope)
            return;
        network.crossroads.Add(cr1);
        segment = new RoadSegment(extensionOrigin, cr1);
        network.roadSegments.Add(segment);
        // return;
        testCr = new Crossroad();
        testCr.x = cr1.x + direction.x * 5;
        testCr.y = cr1.y + direction.y * 5;
        segment = new RoadSegment(cr1, testCr);

        for (int i = 0; i < network.roadSegments.Count; i++)
        {
            if (RoadHelper.areRoadsIntersects(segment, network.roadSegments[i]))
            {
                if (network.roadSegments[i].start == cr1 || network.roadSegments[i].end == cr1)
                    continue;

                Vector2 segmentAngle = new Vector2(segment.start.x - segment.end.x, segment.start.y - segment.end.y);
                Vector2 iSegmentAngle = new Vector2(network.roadSegments[i].start.x - network.roadSegments[i].end.x,
                    network.roadSegments[i].start.y - network.roadSegments[i].end.y);
                if (Mathf.Abs(Vector2.Angle(segmentAngle, iSegmentAngle)) <= 60.0f || Mathf.Abs(Vector2.Angle(segmentAngle, iSegmentAngle)) >= 300.0f)
                    continue;

                Vector2 intersectionPoint = RoadHelper.getIntersectionPoint(segment.start, segment.end, network.roadSegments[i].start, network.roadSegments[i].end);
                if (intersectionPoint == Vector2.zero)
                    continue;

                testCr = new Crossroad(intersectionPoint.x, intersectionPoint.y);
                if (RoadHelper.isUnderWaterline(testCr, generator) || RoadHelper.getSegmentSlope(cr1, testCr, generator) >= generator.maximumSlope) //  
                    return;
                network.crossroads.Add(testCr);
                segment = new RoadSegment(cr1, testCr);
                network.roadSegments.Add(segment);
                Crossroad intersectedStart = network.roadSegments[i].start;
                Crossroad intersectedEnd = network.roadSegments[i].end;
                network.roadSegments[i].setEnd(null);
                network.roadSegments[i].setStart(null);
                network.roadSegments.Remove(network.roadSegments[i]);
                segment = new RoadSegment(intersectedStart, testCr);
                network.roadSegments.Add(segment);
                segment = new RoadSegment(testCr, intersectedEnd);
                network.roadSegments.Add(segment);
                break;
            }
        }
    }
}
