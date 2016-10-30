using UnityEngine;
using System.Collections;

public class RoadConnectorAgent : AbstractAgent {
    public override void agentAction() {
        RoadNetwork network = generator.roadNetwork;
        RoadSegment segment = new RoadSegment();
        Crossroad cr0, cr1;
        while (true) {
            cr0 = network.crossroads[Random.Range(0, network.crossroads.Count - 1)];
            if (cr0.adjacentSegemnts.Count == 1)
                break;
        }
        while (true) {
            cr1 = network.crossroads[Random.Range(0, network.crossroads.Count - 1)];
            if (cr1.adjacentSegemnts.Count == 1 && cr1 != cr0)
                break;
        }

        segment.setStart(cr0);
        segment.setEnd(cr1);
        network.roadSegments.Add(segment);
    }
}
