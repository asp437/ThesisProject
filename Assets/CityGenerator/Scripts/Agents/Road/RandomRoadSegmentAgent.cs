using UnityEngine;
using System.Collections;

public class RandomRoadSegmentAgent : AbstractAgent {
    public override void agentAction() {
        RoadNetwork network = generator.roadNetwork;
        // RoadSegment segment = new RoadSegment();
        Crossroad cr0 = new Crossroad();
        cr0.x = Random.value * 64;
        cr0.y = Random.value * 64;

        /*
         * Vector2 v = new Vector2();
        if (Random.Range(0, 2) == 0) {
            v.x = Random.value;
        } else {
            v.y = Random.value;
        }
        
        v.Normalize();
        cr1.x = cr0.x + v.x;
        cr1.y = cr0.y + v.y;
        */

        // segment.setStart(cr0);
        // segment.setEnd(cr1);
        // network.roadSegments.Add(segment);
        network.crossroads.Add(cr0);
        // network.crossroads.Add(cr1);
    }
}
