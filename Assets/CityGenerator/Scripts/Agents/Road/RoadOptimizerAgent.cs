using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadOptimizerAgent : AbstractAgent
{
    // Removes crossroads that divide straight road into two sections
    public override void agentAction()
    {
        Debug.Log("Pre optimization crossroads count:  " + generator.roadNetwork.crossroads.Count);
        Debug.Log("Pre optimization road segments count:  " + generator.roadNetwork.roadSegments.Count);
        for (int i = 0; i < generator.roadNetwork.crossroads.Count; i++)
        {
            if (generator.roadNetwork.crossroads[i].adjacentSegemnts.Count == 1)
            {
                RoadSegment segment = generator.roadNetwork.crossroads[i].adjacentSegemnts[0];
                segment.setStart(null);
                segment.setEnd(null);
                generator.roadNetwork.roadSegments.Remove(segment);
                generator.roadNetwork.crossroads.RemoveAt(i);
                i = 0; // Restart loop in order to avoid indexing problems
            }
        }
        for (int i = 0; i < generator.roadNetwork.crossroads.Count; i++)
        {
            Crossroad cr = generator.roadNetwork.crossroads[i];
            if (cr.adjacentSegemnts.Count == 2)
            {
                RoadSegment segment1 = cr.adjacentSegemnts[0], segment2 = cr.adjacentSegemnts[1];
                if ((segment1.getStart().x == segment1.getEnd().x && segment2.getStart().x == segment2.getEnd().x && segment1.getStart().x == segment2.getEnd().x) ||
                     segment1.getStart().y == segment1.getEnd().y && segment2.getStart().y == segment2.getEnd().y && segment1.getStart().y == segment2.getEnd().y)
                {
                    // Straight roads
                    Crossroad start = segment1.getStart() == cr ? segment1.getEnd() : segment1.getStart();
                    Crossroad end = segment2.getStart() == cr ? segment2.getEnd() : segment2.getStart();
                    segment1.setStart(null);
                    segment1.setEnd(null);
                    segment2.setStart(null);
                    segment2.setEnd(null);
                    generator.roadNetwork.crossroads.Remove(cr);
                    generator.roadNetwork.roadSegments.Remove(segment1);
                    generator.roadNetwork.roadSegments.Remove(segment2);
                    RoadSegment newSegment = new RoadSegment(start, end);
                    generator.roadNetwork.roadSegments.Add(newSegment);
                    // i = 0; // Restart loop in order to avoid indexing problems
                    i--;
                }
            }
        }
        Debug.Log("Post optimization crossroads count:  " + generator.roadNetwork.crossroads.Count);
        Debug.Log("Post optimization road segments count:  " + generator.roadNetwork.roadSegments.Count);
    }
}
