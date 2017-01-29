using UnityEngine;
using System.Collections;

public class RoadConnectorAgent : AbstractAgent
{
    public const float scale = 0.15f;

    protected bool hasIntersections(RoadNetwork network, Crossroad cr0, Crossroad cr1)
    {
        for (int i = 0; i < network.roadSegments.Count; i++)
        {
            if (RoadHelper.areRoadsIntersects(new RoadSegment(cr0, cr1), network.roadSegments[i]))
                return true;
        }
        return false;
    }

    public override void agentAction()
    {
        RoadNetwork network = generator.roadNetwork;
        Crossroad cr0 = network.crossroads[Random.Range(0, network.crossroads.Count)];
        Crossroad cr1 = network.crossroads[Random.Range(0, network.crossroads.Count)];
        bool cr0_founded = false, cr1_founded = false;
        for (int i = 0; i < network.crossroads.Count; i++)
        {
            if (cr0.adjacentSegemnts.Count < 3)
            {
                cr0_founded = true;
                break;
            }
            cr0 = network.crossroads[Random.Range(0, network.crossroads.Count)];
        }
        for (int i = 0; i < network.crossroads.Count; i++)
        { // Not guarantee checking all possible crossrods
            if (cr1.adjacentSegemnts.Count < 3 && cr1 != cr0 && !hasIntersections(network, cr0, cr1))
            {
                cr1_founded = true;
                break;
            }
            cr1 = network.crossroads[Random.Range(0, network.crossroads.Count)];
        }
        if (!cr0_founded || !cr1_founded)
            return;

        RoadSegment segment = new RoadSegment(cr0, cr1);
        network.roadSegments.Add(segment);
        Debug.Log("Roads connected");
    }
}
