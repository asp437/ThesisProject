using UnityEngine;
using System.Collections;

public class RandomRoadSegmentAgent : AbstractAgent
{
    public override void agentAction()
    {
        RoadNetwork network = generator.roadNetwork;
        Crossroad cr0 = new Crossroad();
        cr0.x = Random.value * generator.meshDimension;
        cr0.y = Random.value * generator.meshDimension;
        network.crossroads.Add(cr0);
    }
}
