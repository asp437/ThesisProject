using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistrictsGenerator : AbstractAgent
{
    protected static District detectDistrict(RoadNetwork roadNetwork, float dimension, int startX, int startY, bool[,] visited)
    {
        District result = new District();
        Queue<Vector2> bfsQueue = new Queue<Vector2>();
        bfsQueue.Enqueue(new Vector2(startX, startY));
        while (bfsQueue.Count > 0)
        {
            Vector2 position = bfsQueue.Dequeue();
            int px = (int)position.x, py = (int)position.y;
            if (px < 0 || px >= dimension || py < 0 || py >= dimension || visited[px, py])
                continue;

            result.cells.Add(new DistrictCell(position));
            visited[px, py] = true;

            if (RoadHelper.hasRoadAt(roadNetwork, position.x, position.y, position.x, position.y + 1)) // Left
                result.cells[result.cells.Count - 1].edgeLeft = true; // Update attribute for last added element
            if (RoadHelper.hasRoadAt(roadNetwork, position.x, position.y, position.x + 1, position.y)) // Up
                result.cells[result.cells.Count - 1].edgeUp = true; // Update attribute for last added element
            if (RoadHelper.hasRoadAt(roadNetwork, position.x + 1, position.y, position.x + 1, position.y + 1)) // Right
                result.cells[result.cells.Count - 1].edgeRight = true; // Update attribute for last added element
            if (RoadHelper.hasRoadAt(roadNetwork, position.x, position.y + 1, position.x + 1, position.y + 1)) // Down
                result.cells[result.cells.Count - 1].edgeBottom = true; // Update attribute for last added element



            if (px > 0 && !visited[px - 1, py] &&
                !RoadHelper.hasRoadAt(roadNetwork, position.x, position.y, position.x, position.y + 1)) // Left
                bfsQueue.Enqueue(new Vector2(position.x - 1, position.y));

            if (py > 0 && !visited[px, py - 1] &&
                !RoadHelper.hasRoadAt(roadNetwork, position.x, position.y, position.x + 1, position.y)) // Up
                bfsQueue.Enqueue(new Vector2(position.x, position.y - 1));

            if ((px + 1) < dimension && !visited[px + 1, py] &&
                !RoadHelper.hasRoadAt(roadNetwork, position.x + 1, position.y, position.x + 1, position.y + 1)) // Right
                bfsQueue.Enqueue(new Vector2(position.x + 1, position.y));

            if ((py + 1) < dimension && !visited[px, py + 1] &&
                !RoadHelper.hasRoadAt(roadNetwork, position.x, position.y + 1, position.x + 1, position.y + 1)) // Down
                bfsQueue.Enqueue(new Vector2(position.x, position.y + 1));
        }
        return result;
    }

    public override void agentAction()
    {
        List<District> result = new List<District>();
        int dimension = generator.meshDimension;
        bool[,] visited = new bool[dimension, dimension];
        for (int x = 0; x < dimension; x++)
            for (int y = 0; y < dimension; y++)
                visited[x, y] = false;

        for (int x = 0; x < dimension; x++)
            for (int y = 0; y < dimension; y++)
                if (!visited[x, y]) // Ignore visited cells
                {
                    District district = detectDistrict(generator.roadNetwork, dimension, x, y, visited);
                    bool internalDistrict = true;
                    foreach (DistrictCell cell in district.cells)
                    {
                        if (cell.x == 0 || cell.x == dimension - 1 || cell.y == 0 || cell.y == dimension - 1)
                            internalDistrict = false;
                    }
                    if (internalDistrict)
                        result.Add(district);
                }
        generator.districtsMap = result;
        generator.cityCenter = DistrictsHelper.getCityCenterPoint(generator.districtsMap, generator);
        generator.cityRadius = DistrictsHelper.getCityRadius(generator.districtsMap, generator);
    }
}
