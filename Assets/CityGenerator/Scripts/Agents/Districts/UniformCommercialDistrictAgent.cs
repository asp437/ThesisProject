using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniformCommercialDistrictAgent : AbstractAgent
{
    public float probability;
    public float minimumDistance = 5;
    public int maxSize = 4;
    public override void agentAction()
    {
        List<District> residentialDistricts = new List<District>();
        foreach (District district in generator.districtsMap)
        {
            if (district.type == DistrictType.RESIDENTIAL)
                residentialDistricts.Add(district);
        }
        if (residentialDistricts.Count > 0)
        {
            District district = residentialDistricts[(int)Random.Range(0, residentialDistricts.Count - 1)];
            int count = district.cells.Count;
            if (district.cells.Count > maxSize || Random.value >= probability * Mathf.Sqrt(1.0f / count))
                return;
            float minimumDistancePre = DistrictsHelper.getCommercialDistrictsMinimumDistance(generator);
            float maximumDistancePre = DistrictsHelper.getCommercialDistrictsMaximumDistance(generator);

            district.type = DistrictType.COMMERCIAL;

            float minimumDistancePost = DistrictsHelper.getCommercialDistrictsMinimumDistance(generator);
            float maximumDistancePost = DistrictsHelper.getCommercialDistrictsMaximumDistance(generator);

            // Revert changes in case of bad decision
            if (minimumDistancePost < minimumDistance)
                district.type = DistrictType.RESIDENTIAL;
            if (maximumDistancePre < maximumDistancePost)
                district.type = DistrictType.RESIDENTIAL;
        }
    }
}
