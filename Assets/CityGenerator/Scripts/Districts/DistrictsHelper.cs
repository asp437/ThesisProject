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
            foreach (DistrictCell cell in district.cells)
            {
                count++;
                result.x += cell.x;
                result.y += cell.y;
            }
        }
        result /= count;
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
}
