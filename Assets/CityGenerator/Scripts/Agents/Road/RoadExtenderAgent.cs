using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class RoadExtenderAgent : AbstractAgent
{
    public float minimumSegmentLength = 1.0f;
    public float maximumSegmentLength = 4.0f;
    public float extensionLength = 5.0f;
    public bool denseGenerator = false;
    public float denseProbability = 0.75f;

    public override void agentAction()
    {
        RoadNetwork network = generator.roadNetwork;
        Crossroad extensionOrigin = network.crossroads[Random.Range(0, network.crossroads.Count - 1)];
        if (denseGenerator && Random.value > denseProbability)
        {
            List<Crossroad> candidates = new List<Crossroad>();
            foreach (Crossroad cr in network.crossroads)
            {
                if (cr.adjacentSegemnts.Count > 1 && cr.adjacentSegemnts.Count < 4)
                    candidates.Add(cr);
            }
            if (candidates.Count > 0) { 
                extensionOrigin = candidates[Random.Range(0, candidates.Count - 1)];
            }
        }
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
        extension = direction * ((int)Random.Range(minimumSegmentLength, maximumSegmentLength));

        for (int i = 0; i < extensionOrigin.adjacentSegemnts.Count; i++)
        {
            Crossroad anotherCr = extensionOrigin.adjacentSegemnts[i].getEnd().Equals(extensionOrigin) ? extensionOrigin.adjacentSegemnts[i].getStart() : extensionOrigin.adjacentSegemnts[i].getEnd();
            Vector2 v0 = new Vector2(extension.x, extension.y), v1 = new Vector2(anotherCr.x - extensionOrigin.x, anotherCr.y - extensionOrigin.y);
            if (Mathf.Abs(Vector2.Angle(v0, v1)) < 10.0f || Mathf.Abs(Vector2.Angle(v0, v1)) > 350.0f)
                return;
        }

        Crossroad testCr = new Crossroad();
        testCr.x = extensionOrigin.x + extension.x;
        testCr.y = extensionOrigin.y + extension.y;
        for (int i = 0; i < network.roadSegments.Count; i++)
        {
            if (network.roadSegments[i].getStart() == extensionOrigin || network.roadSegments[i].getEnd() == extensionOrigin)
                continue;
            if (RoadHelper.areRoadsIntersects(extensionOrigin, testCr, network.roadSegments[i].getStart(), network.roadSegments[i].getEnd(), network.roadSegments[i].width))
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
        testCr = new Crossroad();
        testCr.x = cr1.x + direction.x * extensionLength;
        testCr.y = cr1.y + direction.y * extensionLength;
        
        List<KeyValuePair<float, KeyValuePair<RoadSegment, Vector2>>> intersections = new List<KeyValuePair<float, KeyValuePair<RoadSegment, Vector2>>>();
        for (int i = 0; i < network.roadSegments.Count; i++)
        {
            if (RoadHelper.areRoadsIntersects(cr1, testCr, network.roadSegments[i].getStart(), network.roadSegments[i].getEnd(), network.roadSegments[i].width))
            {
                if (network.roadSegments[i].getStart() == cr1 || network.roadSegments[i].getEnd() == cr1)
                    continue;

                Vector2 segmentAngle = new Vector2(cr1.x - testCr.x, cr1.y - testCr.y);
                Vector2 iSegmentAngle = new Vector2(network.roadSegments[i].getStart().x - network.roadSegments[i].getEnd().x,
                    network.roadSegments[i].getStart().y - network.roadSegments[i].getEnd().y);
                if (Mathf.Abs(Vector2.Angle(segmentAngle, iSegmentAngle)) <= 60.0f || Mathf.Abs(Vector2.Angle(segmentAngle, iSegmentAngle)) >= 300.0f)
                    continue;

                Vector2 intersectionPoint = RoadHelper.getIntersectionPoint(cr1, testCr, network.roadSegments[i].getStart(), network.roadSegments[i].getEnd());
                if (intersectionPoint == Vector2.zero)
                    continue;
                float distance = Vector2.Distance(new Vector2(cr1.x, cr1.y), intersectionPoint);
                intersections.Add(new KeyValuePair<float, KeyValuePair<RoadSegment, Vector2>>(distance, new KeyValuePair<RoadSegment, Vector2>(network.roadSegments[i], intersectionPoint)));

            }
        }
        Crossroad previousCr = cr1;
        intersections.Sort((a, b) => a.Key.CompareTo(b.Key));
        foreach (KeyValuePair<float, KeyValuePair<RoadSegment, Vector2>> intersection in intersections)
        {
            testCr = new Crossroad(intersection.Value.Value.x, intersection.Value.Value.y);
            if (RoadHelper.isUnderWaterline(testCr, generator) || RoadHelper.getSegmentSlope(previousCr, testCr, generator) >= generator.maximumSlope) //  
                continue;
            network.crossroads.Add(testCr);
            segment = new RoadSegment(previousCr, testCr);
            network.roadSegments.Add(segment);
            Crossroad intersectedStart = intersection.Value.Key.getStart();
            Crossroad intersectedEnd = intersection.Value.Key.getEnd();
            intersection.Value.Key.setEnd(null);
            intersection.Value.Key.setStart(null);
            network.roadSegments.Remove(intersection.Value.Key);
            segment = new RoadSegment(intersectedStart, testCr);
            network.roadSegments.Add(segment);
            segment = new RoadSegment(testCr, intersectedEnd);
            network.roadSegments.Add(segment);
            previousCr = testCr;
        }
    }
}
