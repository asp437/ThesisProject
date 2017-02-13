using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndustrialDistrictAgent : AbstractAgent
{
    public int industrialThreshold = 15;
    public float radiusMultiplier = 0.9f;
    public float outOfRadiusProb = 0.5f;

    public override void agentAction()
    {
        foreach (District district in generator.districtsMap)
        {
            int count = 0;
            Vector2 position = new Vector2();
            foreach (DistrictCell cell in district.cells)
            {
                count++;
                position.x += cell.x;
                position.y += cell.y;
            }
            position /= count;
            if (count > industrialThreshold)
            {
                district.type = DistrictType.INDUSTRIAL;
            }
            else
            {
                float distanceToCenter = Vector2.Distance(position, generator.cityCenter);
                if (distanceToCenter > generator.cityRadius * radiusMultiplier && Random.value < outOfRadiusProb)
                    district.type = DistrictType.INDUSTRIAL;
                else
                    district.type = DistrictType.RESIDENTIAL;
            }
        }
    }
}
