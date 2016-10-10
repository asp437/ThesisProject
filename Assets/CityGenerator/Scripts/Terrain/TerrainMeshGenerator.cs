using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TerrainMeshGenerator : MonoBehaviour {
    public float heightScale = 512;
    // Mesh generator filter function parameters
    public float meshExponentFactor = 30.0f;
    public float meshFactor = 1.7f;
    public float meshExponentPower = 2.0f;
    public Gradient gradient;

    public Texture2D GenerateDiffuseMap(float[,] terrainMap) {
        int dimension = terrainMap.GetLength(0);
        Texture2D diffuse = new Texture2D(dimension, dimension);
        for (int x = 0; x < dimension; x++) {
            for (int y = 0; y < dimension; y++) {
                diffuse.SetPixel(x, y, gradient.Evaluate(terrainMap[x, y]));
            }
        }
        diffuse.Apply();
        return diffuse;
    }

    public void GenerateMesh(GameObject terrainObject, float[,] terrainMap, int dimension) {
        Mesh mesh = new Mesh();
        float step = terrainMap.GetLength(0) * 1.0f / dimension;
        Vector3[] vertices = new Vector3[dimension * dimension]; // Vertices matrix
        Vector2[] uvs = new Vector2[dimension * dimension]; // UV texture coordiantes for each vertex
        List<int> indices = new List<int>(); // Indices of each edge
                                             // Generation of vertices matrix
        for (int x = 0; x < dimension; x++) {
            // Use filter function in order to generate more feasible terrain
            float h = meshExponentFactor * Mathf.Pow(meshFactor * terrainMap[(int)(x * step), 0], meshExponentPower);
            vertices[x] = new Vector3(x, h, 0);
            uvs[x] = new Vector2(x * 1.0f / dimension, 0 * 1.0f / dimension);
        }
        // Generation of edges
        for (int i = dimension; i < dimension * dimension; i++) {
            int x = i % dimension;
            int y = i / dimension;
            float h = meshExponentFactor * Mathf.Pow(meshFactor * terrainMap[(int)(x * step), (int)(y * step)], meshExponentPower);
            vertices[i] = new Vector3(x, h, y);
            uvs[i] = new Vector2(x * 1.0f / dimension, y * 1.0f / dimension);
            if (x == 0) {
                indices.Add(i - dimension);
                indices.Add(i);
                indices.Add(i - dimension + 1);
            } else if (x == dimension - 1) {
                indices.Add(i - dimension);
                indices.Add(i - 1);
                indices.Add(i);
            } else {
                indices.Add(i - dimension);
                indices.Add(i - 1);
                indices.Add(i);
                indices.Add(i - dimension);
                indices.Add(i);
                indices.Add(i - dimension + 1);
            }
        }
        mesh.SetVertices(new List<Vector3>(vertices));
        mesh.SetIndices(indices.ToArray(), MeshTopology.Triangles, 0);
        mesh.SetUVs(0, new List<Vector2>(uvs));
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        if (terrainObject.GetComponent<MeshFilter>() == null) {
            terrainObject.AddComponent<MeshFilter>();
        }
        if (terrainObject.GetComponent<MeshRenderer>() == null){
            terrainObject.AddComponent<MeshRenderer>();
        }
        if (terrainObject.GetComponent<MeshCollider>() == null) {
            terrainObject.AddComponent<MeshCollider>();
        }
        ((MeshFilter)(terrainObject.GetComponent<MeshFilter>())).mesh = mesh;
        ((MeshRenderer)(terrainObject.GetComponent<MeshRenderer>())).material.mainTexture = GenerateDiffuseMap(terrainMap);
        ((MeshCollider)(terrainObject.GetComponent<MeshCollider>())).sharedMesh = mesh;
    }
}
