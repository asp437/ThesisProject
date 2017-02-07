using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistrictsHelper {
    // Vector2 position (in district) describes one cell with size (1, 1) and points to upper left corner.

    protected static District detectDistrict(RoadNetwork roadNetwork, float dimension, int startX, int startY, bool[,] visited)
    {
        District result = new District();
        Queue<Vector2> bfsQueue = new Queue<Vector2>();
        bfsQueue.Enqueue(new Vector2(startX, startY));
        while (bfsQueue.Count > 0)
        {
            Vector2 position = bfsQueue.Dequeue();
            if (position.x < 0 || position.x >= dimension || position.y < 0 || position.y >= dimension)
                continue;
            result.squares.Add(position);
            visited[(int)position.x, (int)position.y] = true;
            if (!RoadHelper.hasRoadAt(roadNetwork, position.x, position.y, position.x, position.y + 1)) // Left
                bfsQueue.Enqueue(new Vector2(position.x - 1, position.y));
            if (!RoadHelper.hasRoadAt(roadNetwork, position.x, position.y, position.x + 1, position.y)) // Up
                bfsQueue.Enqueue(new Vector2(position.x, position.y - 1));
            if (!RoadHelper.hasRoadAt(roadNetwork, position.x + 1, position.y, position.x + 1, position.y + 1)) // Right
                bfsQueue.Enqueue(new Vector2(position.x + 1, position.y));
            if (!RoadHelper.hasRoadAt(roadNetwork, position.x, position.y + 1, position.x + 1, position.y + 1)) // Down
                bfsQueue.Enqueue(new Vector2(position.x, position.y + 1));
        }
        return result;
    }

    public static List<District> createDistrictsMap(RoadNetwork roadNetwork, CityGenerator generator)
    {
        List<District> result = new List<District>();
        int dimension = generator.meshDimension;
        bool[,] visited = new bool[dimension, dimension];
        for (int x = 0; x < dimension; x++)
            for (int y = 0; y < dimension; y++)
                visited[x, y] = false;

        for (int x = 0; x < dimension; x++)
            for (int y = 0; y < dimension; y++)
                if (!visited[x,y]) // Ignore visited cells
                {
                    District district = detectDistrict(roadNetwork, dimension, x, y, visited);
                    result.Add(district);
                }

        return result;
    }
}
