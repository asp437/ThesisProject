using UnityEngine;
using System.Collections;

[System.Serializable]
public struct AgentConfiguration {
    public AbstractAgent agent;
    public int runs;
}

public class CityGenerator : MonoBehaviour {
    public TerrainGenerator terrainGenerator;
    public RoadMeshGenerator roadMeshGenerator;
    public RoadNetwork roadNetwork;
    public int terrainGeneratorSeed;
    public AgentConfiguration[] agentsList;
    public int meshDimension;

    // Use this for initialization
    void Start () {
        float[,] terrainMap = terrainGenerator.GenerateTerrain(terrainGeneratorSeed, null, this);
        roadNetwork = new RoadNetwork();

        for (int i = 0; i < agentsList.Length; i++) {
            agentsList[i].agent.generator = this;
            for (int j = 0; j < agentsList[i].runs; j++) {
                agentsList[i].agent.agentAction();
            }
        }
        roadMeshGenerator.terrainMeshGenerator = terrainGenerator.meshGenerator;
        roadMeshGenerator.generateMesh(new GameObject(), roadNetwork, terrainMap, meshDimension);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
