using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoadMeshGenerator : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    protected float getPointHeight(float x, float y, float[,] terrainMap) {
        int x_i = (int)x;
        int y_i = (int)y;
        return terrainMap[x_i, y_i];
    }

    public void generateMesh(GameObject gameObject, RoadNetwork roadNetwork, float[,] terrainMap) {
        Mesh mesh = new Mesh();
        List<Vector3> vertices = new List<Vector3>(); // Vertices matrix
        List<int> indices = new List<int>();

        for (int i = 0; i < roadNetwork.crossroads.Count; i++) {
            Vector3 v0 = new Vector3(), v1 = new Vector3();
            float z = getPointHeight(roadNetwork.crossroads[i].x, roadNetwork.crossroads[i].y, terrainMap);
            Vector2 n = new Vector2(0, 0);
            for (int j = 0; j < roadNetwork.crossroads[i].adjacentSegemnts.Count; j++) {
                Crossroad anotherCrossroad = roadNetwork.crossroads[i].adjacentSegemnts[j].start == roadNetwork.crossroads[i] ? roadNetwork.crossroads[i].adjacentSegemnts[j].end : roadNetwork.crossroads[i].adjacentSegemnts[j].start;
                float dx = roadNetwork.crossroads[i].x - anotherCrossroad.x;
                float dy = roadNetwork.crossroads[i].y - anotherCrossroad.y;
                Vector2 tn = new Vector2(-dy, dx);
                tn.Normalize();
                n += tn;
            }
            n.Normalize();
            v0.x = roadNetwork.crossroads[i].x + 2.5f * n.y;
            v0.z = roadNetwork.crossroads[i].y - 2.5f * n.x;
            v0.y = z;
            v1.x = roadNetwork.crossroads[i].x - 2.5f * n.y;
            v1.z = roadNetwork.crossroads[i].y + 2.5f * n.x;
            v1.y = z;
            vertices.Add(v0);
            vertices.Add(v1);
            Debug.Log("added 2 vertices");
        }
        for (int i = 0; i < roadNetwork.roadSegments.Count; i++) {
            int s = roadNetwork.crossroads.IndexOf(roadNetwork.roadSegments[i].start);
            int e = roadNetwork.crossroads.IndexOf(roadNetwork.roadSegments[i].end);
            indices.Add(2*s);
            indices.Add(2*s + 1);
            indices.Add(2 * e);

            indices.Add(2 * s + 1);
            indices.Add(2 * e + 1);
            indices.Add(2 * e);
            Debug.Log("added 2 triangles");
        }

        mesh.SetVertices(vertices);
        mesh.SetIndices(indices.ToArray(), MeshTopology.Triangles, 0);
        // mesh.RecalculateNormals();
        // mesh.RecalculateBounds();
        if (gameObject.GetComponent<MeshFilter>() == null) {
            gameObject.AddComponent<MeshFilter>();
        }
        if (gameObject.GetComponent<MeshRenderer>() == null) {
            gameObject.AddComponent<MeshRenderer>();
        }
        ((MeshFilter)(gameObject.GetComponent<MeshFilter>())).mesh = mesh;
    }
}
