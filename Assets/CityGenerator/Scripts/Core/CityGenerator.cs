﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public struct AgentConfiguration
{
    public AbstractAgent agent;
    public int runs;
}

public class CityGenerator : MonoBehaviour
{
    public TerrainGenerator terrainGenerator;
    public RoadMeshGenerator roadMeshGenerator;
    public RoadNetwork roadNetwork;
    public int terrainGeneratorSeed;
    public AgentConfiguration[] agentsList;
    public int meshDimension;
    public float[,] terrainMap;
    public float waterlineHeight;
    public float maximumSlope;
    public float meshScale = 20.0f;
    public List<District> districtsMap;

    public float getPointHeight(float x, float y)
    {
        int x_i = (int)x;
        int y_i = (int)y;
        if (x_i >= meshDimension - 1)
            x_i = meshDimension - 2;
        if (y_i >= meshDimension - 1)
            y_i = meshDimension - 2;
        if (x_i < 0)
            x_i = 0;
        if (y_i < 0)
            y_i = 0;
        float h0 = terrainGenerator.meshGenerator.getPointHeight(x_i, y_i, meshDimension, terrainMap);
        float h1 = terrainGenerator.meshGenerator.getPointHeight(x_i + 1, y_i, meshDimension, terrainMap);
        float h2 = terrainGenerator.meshGenerator.getPointHeight(x_i, y_i + 1, meshDimension, terrainMap);
        float h3 = terrainGenerator.meshGenerator.getPointHeight(x_i + 1, y_i + 1, meshDimension, terrainMap);
        h0 = MathHelper.lerp(h0, h1, x - x_i);
        h2 = MathHelper.lerp(h2, h3, x - x_i);
        return MathHelper.lerp(h0, h2, y - y_i);
    }

    // Use this for initialization
    void Start()
    {
        GameObject crossroads = new GameObject("Crossroads");
        GameObject segments = new GameObject("Road Segments");

        System.DateTime t1 = System.DateTime.Now;
        terrainMap = terrainGenerator.GenerateTerrain(terrainGeneratorSeed, null, this);
        roadNetwork = new RoadNetwork();

        System.DateTime t2 = System.DateTime.Now;
        for (int i = 0; i < agentsList.Length; i++)
        {
            agentsList[i].agent.generator = this;
            for (int j = 0; j < agentsList[i].runs; j++)
                agentsList[i].agent.agentAction();
        }

        System.DateTime t3 = System.DateTime.Now;
        roadMeshGenerator.terrainMeshGenerator = terrainGenerator.meshGenerator;
        roadMeshGenerator.generateMesh(crossroads, segments, roadNetwork, this);

        System.DateTime t4 = System.DateTime.Now;
        districtsMap = DistrictsHelper.createDistrictsMap(roadNetwork, this);

        System.DateTime t5 = System.DateTime.Now;
        Debug.Log("Terrain generation time: " + (t2 - t1).ToString());
        Debug.Log("Agents processing time: " + (t3 - t2).ToString());
        Debug.Log("Roads generation time: " + (t4 - t3).ToString());
        Debug.Log("Districts creation time: " + (t5 - t4).ToString());
        Debug.Log("Road segments: " + roadNetwork.roadSegments.Count);
        Debug.Log("Crossroads: " + roadNetwork.crossroads.Count);
        Debug.Log("Districts: " + districtsMap.Count);

        terrainGenerator.terrainObject.transform.localScale *= meshScale;
        crossroads.transform.localScale *= meshScale;
        segments.transform.localScale *= meshScale;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
