using UnityEngine;
using System.Collections;

public class CityGenerator : MonoBehaviour {
    public TerrainGenerator terrainGenerator;
    public RoadMeshGenerator roadMeshGenerator;
    public RoadNetwork roadNetwork;
    public int terrainGeneratorSeed;
    public AbstractAgent[] agentsList;
    public int meshDimension;

    // Use this for initialization
    void Start () {
        float[,] terrainMap = terrainGenerator.GenerateTerrain(terrainGeneratorSeed, null, this);
        roadNetwork = new RoadNetwork();
        roadMeshGenerator = new RoadMeshGenerator(terrainGenerator.meshGenerator);
        roadMeshGenerator.generateMesh(new GameObject(), roadNetwork, terrainMap, meshDimension);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
