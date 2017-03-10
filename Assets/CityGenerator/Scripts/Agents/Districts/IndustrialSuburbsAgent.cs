using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndustrialSuburbsAgent : AbstractAgent
{
    public float radiusMultiplier = 0.9f;
    public float outOfRadiusProb = 0.5f;

    public override void agentAction()
    {
        if (Random.value < outOfRadiusProb)
        {
            List<District> outOfRadiusDistricts = new List<District>();
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
                float distanceToCenter = Vector2.Distance(position, generator.cityCenter);
                if (distanceToCenter > generator.cityRadius * radiusMultiplier)
                    outOfRadiusDistricts.Add(district);
            }
            if (outOfRadiusDistricts.Count > 0)
                outOfRadiusDistricts[(int)Random.Range(0.0f, outOfRadiusDistricts.Count - 1)].type = DistrictType.INDUSTRIAL;
        }
    }
}
