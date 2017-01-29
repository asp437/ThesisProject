using UnityEngine;
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

    // Use this for initialization
    void Start()
    {
        System.DateTime t1 = System.DateTime.Now;
        float[,] terrainMap = terrainGenerator.GenerateTerrain(terrainGeneratorSeed, null, this);
        roadNetwork = new RoadNetwork();
        System.DateTime t2 = System.DateTime.Now;

        for (int i = 0; i < agentsList.Length; i++)
        {
            agentsList[i].agent.generator = this;
            for (int j = 0; j < agentsList[i].runs; j++)
            {
                agentsList[i].agent.agentAction();
            }
        }
        System.DateTime t3 = System.DateTime.Now;
        roadMeshGenerator.terrainMeshGenerator = terrainGenerator.meshGenerator;
        roadMeshGenerator.generateMesh(new GameObject(), roadNetwork, terrainMap, meshDimension);
        System.DateTime t4 = System.DateTime.Now;
        Debug.Log("Terrain generation time: " + (t2 - t1).ToString());
        Debug.Log("Agents processing time: " + (t3 - t2).ToString());
        Debug.Log("Roads generation time: " + (t4 - t3).ToString());
        Debug.Log("Road segments: " + roadNetwork.roadSegments.Count);
        Debug.Log("Crossroads: " + roadNetwork.crossroads.Count);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
