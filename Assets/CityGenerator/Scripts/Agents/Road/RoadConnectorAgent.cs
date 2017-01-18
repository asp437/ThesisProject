using UnityEngine;
using System.Collections;

public class RoadConnectorAgent : AbstractAgent {
    public const float scale = 0.15f;

    protected bool hasIntersections(RoadNetwork network, Crossroad cr0, Crossroad cr1) {
        for (int i = 0; i < network.roadSegments.Count; i++) {
            if (RoadHelper.areRoadsIntersects(new RoadSegment(cr0, cr1), network.roadSegments[i]))
                return true;
        }
        return false;
    }

    public override void agentAction() {
        RoadNetwork network = generator.roadNetwork;
        Crossroad cr0 = network.crossroads[Random.Range(0, network.crossroads.Count - 1)];
        Crossroad cr1 = network.crossroads[Random.Range(0, network.crossroads.Count - 1)];
        bool cr0_founded = false, cr1_founded = false;
        for (int i = 0; i < network.crossroads.Count; i++) {
            if (cr0.adjacentSegemnts.Count < 2) {
                cr0_founded = true;
                break;
            }
            cr0 = network.crossroads[Random.Range(0, network.crossroads.Count - 1)];
        }
        for (int i = 0; i < network.crossroads.Count; i++) { // Not guarantee checking all possible crossrods
            if (cr1.adjacentSegemnts.Count < 2 && cr1 != cr0 && !hasIntersections(network, cr0, cr1)) {
                cr1_founded = true;
                break;
            }
            cr1 = network.crossroads[Random.Range(0, network.crossroads.Count - 1)];
        }
        if (!cr0_founded || !cr1_founded)
            return;

        Vector2 v0 = new Vector2(cr0.x, cr0.y);
        Vector2 v1 = new Vector2(cr1.x, cr1.y);
        Vector2 dir = v1 - v0;
        
        int angle = (int)Vector2.Angle(dir, new Vector2(10, 0));
        if (angle % 90 > 1) {
            return;
        }

        RoadSegment segment = new RoadSegment(cr0, cr1);
        for (int i = 0; i < network.roadSegments.Count; i++)
            if (RoadHelper.areRoadsIntersects(segment, network.roadSegments[i]))
                return;

        network.roadSegments.Add(segment);
    }
}
