using UnityEngine;
using System.Collections;

public class RoadConnectorAgent : AbstractAgent {
    protected bool hasIntersections(RoadNetwork network, Crossroad cr0, Crossroad cr1) {
        for (int i = 0; i < network.roadSegments.Count; i++) {
            if (RoadHelper.intersects(cr0, cr1, network.roadSegments[i].start, network.roadSegments[i].end))
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
            if (cr0.adjacentSegemnts.Count < 4) {
                cr0_founded = true;
                break;
            }
            cr0 = network.crossroads[Random.Range(0, network.crossroads.Count - 1)];
        }
        for (int i = 0; i < network.crossroads.Count; i++) {
            if (cr1.adjacentSegemnts.Count < 4 && cr1 != cr0 && !hasIntersections(network, cr0, cr1)) {
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
        //int angle = (int)Vector2.Angle(dir, new Vector2(10, 0));
        //if (angle % 90 > 1) {
        //    return;
        //}
        //Debug.Log("test");
        if (Vector2.Distance(v0, v1) > 1.0f) {
            dir.Normalize();
            Vector2 v2 = dir + v0;
            Crossroad old_cr2 = cr0;
            int t = 0;
            RoadSegment segment;
            while (Vector2.Distance(v2, v1) > 1.0f && t < 150) {
                segment = new RoadSegment();
                Crossroad cr2 = new Crossroad();
                cr2.x = v2.x;
                cr2.y = v2.y;
                network.crossroads.Add(cr2);
                segment.setStart(old_cr2);
                segment.setEnd(cr2);
                network.roadSegments.Add(segment);
                v2 = v2 + dir;
                old_cr2 = cr2;
                t++;
            }
            segment = new RoadSegment();
            segment.setStart(old_cr2);
            segment.setEnd(cr1);
            network.roadSegments.Add(segment);
        } else {
            RoadSegment segment = new RoadSegment();
            segment.setStart(cr0);
            segment.setEnd(cr1);
            network.roadSegments.Add(segment);
        }
    }
}
