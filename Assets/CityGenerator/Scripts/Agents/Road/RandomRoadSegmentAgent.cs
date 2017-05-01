using UnityEngine;
using System.Collections;

public class RandomRoadSegmentAgent : AbstractAgent
{
    public override void agentAction()
    {
        RoadNetwork network = generator.roadNetwork;
        Crossroad cr0 = new Crossroad();
        int i = 0;
        do
        {
            cr0.x = Random.value * generator.meshDimension;
            cr0.y = Random.value * generator.meshDimension;
            i++;
        } while (RoadHelper.isUnderWaterline(cr0, generator) && i < 1000000);
        cr0.x = (int)cr0.x;
        cr0.y = (int)cr0.y;
        network.crossroads.Add(cr0);
    }
}
