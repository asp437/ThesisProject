using System.Collections.Generic;
using UnityEngine;

public class CityCenterCommercialDistrictsAgent : AbstractAgent
{
    public float radiusMultiplier = 0.2f;
    public float outOfRadiusProb = 0.75f;
    private static List<District> residentialDistricts = null;

    private void createResidentialDistrictsList()
    {
        residentialDistricts = new List<District>();
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
            if (distanceToCenter < generator.cityRadius * radiusMultiplier)
                residentialDistricts.Add(district);
        }
    }

    public override void agentAction()
    {
        if (residentialDistricts == null)
            createResidentialDistrictsList();
        if (residentialDistricts.Count > 0)
        {
            District selectedDistrict = residentialDistricts[(int)Random.Range(0.0f, residentialDistricts.Count - 1)];
            if (Random.value < (outOfRadiusProb / selectedDistrict.cells.Count))
            {
                selectedDistrict.type = DistrictType.COMMERCIAL;
                residentialDistricts.Remove(selectedDistrict);
            }
        }
    }
}
