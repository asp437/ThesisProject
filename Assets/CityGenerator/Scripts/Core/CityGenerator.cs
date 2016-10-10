using UnityEngine;
using System.Collections;

public class CityGenerator : MonoBehaviour {
    public TerrainGenerator terrainGenerator;
    public int terrainGeneratorSeed;
    public AbstractAgent[] agentsList;

    // Use this for initialization
    void Start () {
        terrainGenerator.GenerateTerrain(terrainGeneratorSeed, null);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
