using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndustrialThresholdAgent : AbstractAgent
{
    public int industrialThreshold = 15;
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
        }
    }
}
