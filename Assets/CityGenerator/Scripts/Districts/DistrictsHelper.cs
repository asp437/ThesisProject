using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistrictsHelper
{
    public static Vector2 getCityCenterPoint(List<District> districtsList, CityGenerator generator)
    {
        Vector2 result = new Vector2();
        int count = 0;
        foreach (District district in districtsList)
        {
            Vector2 districtCenter = new Vector2();
            foreach (DistrictCell cell in district.cells)
            {
                districtCenter.x += cell.x;
                districtCenter.y += cell.y;
                count++;
            }
            result += districtCenter / district.cells.Count;
        }
        result /= districtsList.Count;
        return result;
    }

    public static float getCityRadius(List<District> districtsList, CityGenerator generator)
    {
        float result = 0.0f;
        foreach (District district in districtsList)
        {
            foreach (DistrictCell cell in district.cells)
            {
                float distance = Vector2.Distance(new Vector2(cell.x + 0.5f, cell.y + 0.5f), generator.cityCenter);
                if (distance > result)
                    result = distance;
            }
        }
        return result;
    }

    public static float getCommercialDistrictsMinimumDistance(CityGenerator generator)
    {
        float result = float.MaxValue;
        List<District> commercialDistricts = new List<District>();
        foreach (District district in generator.districtsMap)
        {
            if (district.type == DistrictType.COMMERCIAL)
                commercialDistricts.Add(district);
        }

        foreach (District district in commercialDistricts)
        {
            foreach (District comDistrict in commercialDistricts)
            {
                if (district == comDistrict)
                    continue;
                float distance = district.getDistanceTo(comDistrict, generator.roadNetwork);
                if (distance < result)
                    result = distance;
            }
        }
        return result;
    }

    public static float getCommercialDistrictsMaximumDistance(CityGenerator generator)
    {
        float result = float.MinValue;
        List<District> commercialDistricts = new List<District>();
        foreach (District district in generator.districtsMap)
        {
            if (district.type == DistrictType.COMMERCIAL)
                commercialDistricts.Add(district);
        }

        foreach (District district in generator.districtsMap)
        {
            if (district.type != DistrictType.RESIDENTIAL)
                continue;

            float minDistance = float.MaxValue;
            foreach (District comDistrict in commercialDistricts)
            {
                float distance = district.getDistanceTo(comDistrict, generator.roadNetwork);
                if (distance < minDistance)
                    minDistance = distance;
            }

            if (minDistance > result)
                result = minDistance;
        }

        return result;
    }
}
