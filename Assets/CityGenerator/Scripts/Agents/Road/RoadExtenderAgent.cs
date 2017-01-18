using UnityEngine;
using System.Collections;

public class RoadExtenderAgent : AbstractAgent {
    private const float scale = 0.2f;
    public override void agentAction() {
        RoadNetwork network = generator.roadNetwork;
        Crossroad extensionOrigin = network.crossroads[Random.Range(0, network.crossroads.Count - 1)];
        Vector2 extensionDirection;
        RoadSegment segment;
        switch (Random.Range(0, 3)) {
            case 0: // Right
                extensionDirection = new Vector2(0, 1);
                break;
            case 1: // Left
                extensionDirection = new Vector2(0, -1);
                break;
            case 2: // Up
                extensionDirection = new Vector2(1, 0);
                break;
            case 3: // Down
            default:
                extensionDirection = new Vector2(-1, 0);
                break;
        }
        extensionDirection *= Random.Range(0.4f, 20.0f) * scale;

        for (int i = 0; i < extensionOrigin.adjacentSegemnts.Count; i++) {
            Crossroad anotherCr = extensionOrigin.adjacentSegemnts[i].end == extensionOrigin ? extensionOrigin.adjacentSegemnts[i].start : extensionOrigin.adjacentSegemnts[i].end;
            Vector2 v0 = new Vector2(extensionDirection.x, extensionDirection.y), v1 = new Vector2(anotherCr.x - extensionOrigin.x, anotherCr.y  - extensionOrigin.y);
            if (Vector2.Angle(v0, v1) < 1)
                return;
        }
        
        Crossroad testCr = new Crossroad();
        testCr.x = extensionOrigin.x + extensionDirection.x;
        testCr.y = extensionOrigin.y + extensionDirection.y;
        for (int i = 0; i < network.roadSegments.Count; i++) {
            if (network.roadSegments[i].start == extensionOrigin || network.roadSegments[i].end == extensionOrigin)
                continue;
            if (RoadHelper.areRoadsIntersects(new RoadSegment(extensionOrigin, testCr), network.roadSegments[i]))
                return;
        }
        
        Crossroad cr1 = new Crossroad(extensionOrigin.x + extensionDirection.x, extensionOrigin.y + extensionDirection.y);
        if (cr1.x > generator.meshDimension - 1 || cr1.x < 1 || cr1.y > generator.meshDimension - 1 || cr1.y < 1)
            return;
        network.crossroads.Add(cr1);
        segment = new RoadSegment(extensionOrigin, cr1);
        network.roadSegments.Add(segment);

        testCr = new Crossroad();
        testCr.x = cr1.x + Mathf.Clamp01(cr1.x - extensionOrigin.x) * 3;
        testCr.y = cr1.y + Mathf.Clamp01(cr1.y - extensionOrigin.y) * 3;
        segment = new RoadSegment(cr1, testCr);

        for (int i = 0; i < network.roadSegments.Count; i++)
        {
            if (RoadHelper.areRoadsIntersects(segment, network.roadSegments[i]))
            {
                Vector2 intersectionPoint = RoadHelper.getIntersectionPoint(segment.start, segment.end, network.roadSegments[i].start, network.roadSegments[i].end);
                if (intersectionPoint == Vector2.zero)
                    continue;
                testCr = new Crossroad(intersectionPoint.x, intersectionPoint.y);
                network.crossroads.Add(testCr);
                segment.setStart(cr1);
                segment.setEnd(testCr);
                network.roadSegments.Add(segment);
                return;
            }
        }
    }
}
