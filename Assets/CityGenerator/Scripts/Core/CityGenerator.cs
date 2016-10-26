using UnityEngine;
using System.Collections;

public class CityGenerator : MonoBehaviour {
    public TerrainGenerator terrainGenerator;
    public RoadMeshGenerator roadMeshGenerator;
    public RoadNetwork roadNetwork;
    public int terrainGeneratorSeed;
    public AbstractAgent[] agentsList;

    // Use this for initialization
    void Start () {
        float[,] terrainMap = terrainGenerator.GenerateTerrain(terrainGeneratorSeed, null);
        roadNetwork = new RoadNetwork();
        roadMeshGenerator = new RoadMeshGenerator();
        roadMeshGenerator.generateMesh(new GameObject(), roadNetwork, terrainMap);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
