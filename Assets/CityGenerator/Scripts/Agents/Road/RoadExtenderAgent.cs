using UnityEngine;
using System.Collections;

public class RoadExtenderAgent : AbstractAgent {
    private const float scale = 0.2f;
    public override void agentAction() {
        RoadNetwork network = generator.roadNetwork;
        Crossroad cr0 = network.crossroads[Random.Range(0, network.crossroads.Count - 1)];
        Vector2 dir;
        switch (Random.Range(0, 3)) {
            case 0:
                dir = new Vector2(0, 1);
                break;
            case 1:
                dir = new Vector2(0, -1);
                break;
            case 2:
                dir = new Vector2(1, 0);
                break;
            case 3:
            default:
                dir = new Vector2(-1, 0);
                break;
        }
        dir *= Random.Range(0.4f, 1.0f) * scale;
        int count = Random.Range(1, 25);

        for (int i = 0; i < cr0.adjacentSegemnts.Count; i++) {
            Crossroad anotherCr = cr0.adjacentSegemnts[i].end == cr0 ? cr0.adjacentSegemnts[i].start : cr0.adjacentSegemnts[i].end;
            Vector2 v0 = new Vector2(dir.x, dir.y), v1 = new Vector2(anotherCr.x - cr0.x, anotherCr.y  - cr0.y);
            if (Vector2.Angle(v0, v1) < 1)
                return;
        }

        // TODO: Improve blocking creation of road close to some parallel roads
        Crossroad testCr = new Crossroad(), testCr1 = new Crossroad();
        testCr.x = cr0.x + dir.x * 5 + dir.y * 5;
        testCr.y = cr0.y + dir.y * 5 + dir.x * 5;

        testCr1.x = cr0.x + dir.x * 5;
        testCr1.y = cr0.y + dir.y * 5;

        for (int i = 0; i < network.roadSegments.Count; i++)
            if (RoadHelper.intersects(testCr1, testCr, network.roadSegments[i].start, network.roadSegments[i].end))
                return;

        testCr.x = cr0.x + dir.x * 5 + dir.y * -5;
        testCr.y = cr0.y + dir.y * 5 + dir.x * -5;
        for (int i = 0; i < network.roadSegments.Count; i++)
            if (RoadHelper.intersects(testCr1, testCr, network.roadSegments[i].start, network.roadSegments[i].end))
                return;

        Crossroad testCrossroad = new Crossroad();
        testCrossroad.x = cr0.x + dir.x * count;
        testCrossroad.y = cr0.y + dir.y * count;
        for (int i = 0; i < network.roadSegments.Count; i++) {
            if (network.roadSegments[i].start == cr0 || network.roadSegments[i].end == cr0)
                continue;
            if (RoadHelper.intersects(cr0, testCrossroad, network.roadSegments[i].start, network.roadSegments[i].end))
                return;
        }

        Crossroad old_cr1 = cr0;
        for (int i = 0; i < count; i++) {
            Crossroad cr1 = new Crossroad();
            cr1.x = old_cr1.x + dir.x;
            cr1.y = old_cr1.y + dir.y;
            if (cr1.x > generator.meshDimension - 1 || cr1.x < 1 || cr1.y > generator.meshDimension - 1 || cr1.y < 1)
                return;
            network.crossroads.Add(cr1);
            RoadSegment segment = new RoadSegment();
            segment.setStart(old_cr1);
            segment.setEnd(cr1);
            network.roadSegments.Add(segment);
            old_cr1 = cr1;
        }
        testCr = new Crossroad();
        testCr.x = old_cr1.x + dir.x * 5;
        testCr.y = old_cr1.y + dir.y * 5;

        for (int i = 0; i < network.roadSegments.Count; i++) {
            if (RoadHelper.intersects(old_cr1, testCr, network.roadSegments[i].start, network.roadSegments[i].end))
            {
                RoadSegment segment = new RoadSegment();
                segment.setStart(old_cr1);
                if (Vector2.Distance(new Vector2(old_cr1.x, old_cr1.y), new Vector2(network.roadSegments[i].start.x, network.roadSegments[i].start.y)) < 
                    Vector2.Distance(new Vector2(old_cr1.x, old_cr1.y), new Vector2(network.roadSegments[i].end.x, network.roadSegments[i].end.y)))
                    segment.setEnd(network.roadSegments[i].start);
                else
                    segment.setEnd(network.roadSegments[i].end);
                network.roadSegments.Add(segment);
                break;
            }
        }
    }
}
