﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoadMeshGenerator : MonoBehaviour {
    public const float scaleMultiplier = 0.3f;
    private const int verticesPerCross = 4;
    private TerrainMeshGenerator _terrainMeshGenerator;

    public RoadMeshGenerator(TerrainMeshGenerator terrainMeshGenerator) {
	    _terrainMeshGenerator = terrainMeshGenerator;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    protected float getPointHeight(float x, float y, int dimension, float[,] terrainMap) {
        int x_i = (int)x;
        int y_i = (int)y;
        if (x_i >= dimension)
            x_i = dimension - 1;
        if (y_i >= dimension)
            y_i = dimension - 1;
        return _terrainMeshGenerator.getPointHeight(x_i, y_i, dimension, terrainMap);
    }

    protected void connectClosest(List<Vector3> vertices, List<int> indices, int s, int e) {
        List<Vector3> sVertices = new List<Vector3>();
        List<Vector3> eVertices = new List<Vector3>();
        for (int i = 0; i < 4; i++) {
            sVertices.Add(vertices[verticesPerCross * s + i]);
            eVertices.Add(vertices[verticesPerCross * e + i]);
        }
        Vector3 v = sVertices[0];
        int closest0 = 0; float d0 = Vector3.Distance(v, eVertices[0]);
        int closest1 = 1; float d1 = Vector3.Distance(v, eVertices[1]);
        for (int j = 1; j < 4; j++) {
            float d = Vector3.Distance(v, eVertices[j]);
            if (d < d0) {
                d1 = d0;
                closest1 = closest0;
                d0 = d;
                closest0 = j;
            } else if (d < d1) {
                closest1 = j;
                d1 = d;
            }
        }
        int closest2 = 0; float d2 = Vector3.Distance(eVertices[closest0], sVertices[0]);
        int closest3 = 1; float d3 = Vector3.Distance(eVertices[closest0], sVertices[1]);
        for (int j = 1; j < 4; j++) {
            float d = Vector3.Distance(eVertices[closest0], sVertices[j]);
            if (d < d2) {
                d3 = d2;
                closest3 = closest2;
                d2 = d;
                closest2 = j;
            } else if (d < d3) {
                closest3 = j;
                d3 = d;
            }
        }

        indices.Add(verticesPerCross * s + closest3);
        indices.Add(verticesPerCross * s + closest2);
        indices.Add(verticesPerCross * e + closest0);
        indices.Add(verticesPerCross * e + closest1);
    }

    public void generateMesh(GameObject gameObject, RoadNetwork roadNetwork, float[,] terrainMap, int dimension) {
        Mesh mesh = new Mesh();
        List<Vector3> vertices = new List<Vector3>(); // Vertices matrix
        List<int> indices = new List<int>();
        float halfLength = dimension / 2.0f;

        for (int i = 0; i < roadNetwork.crossroads.Count; i++) {
            Vector3 v0 = new Vector3(), v1 = new Vector3(), v2 = new Vector3(), v3 = new Vector3();
            float z = getPointHeight(roadNetwork.crossroads[i].x, roadNetwork.crossroads[i].y, dimension, terrainMap);
            v0.x = roadNetwork.crossroads[i].x - 1 * scaleMultiplier;
            v0.z = roadNetwork.crossroads[i].y - 1 * scaleMultiplier;
            v0.y = z;

            v1.x = roadNetwork.crossroads[i].x - 1 * scaleMultiplier;
            v1.z = roadNetwork.crossroads[i].y + 1 * scaleMultiplier;
            v1.y = z;

            v2.x = roadNetwork.crossroads[i].x + 1 * scaleMultiplier;
            v2.z = roadNetwork.crossroads[i].y + 1 * scaleMultiplier;
            v2.y = z;

            v3.x = roadNetwork.crossroads[i].x + 1 * scaleMultiplier;
            v3.z = roadNetwork.crossroads[i].y - 1 * scaleMultiplier;
            v3.y = z;

            vertices.Add(v0);
            vertices.Add(v1);
            vertices.Add(v2);
            vertices.Add(v3);
            indices.Add(verticesPerCross * i);
            indices.Add(verticesPerCross * i + 1);
            indices.Add(verticesPerCross * i + 2);
            indices.Add(verticesPerCross * i + 3);
        }
        for (int i = 0; i < roadNetwork.roadSegments.Count; i++) {
            int s = roadNetwork.crossroads.IndexOf(roadNetwork.roadSegments[i].start);
            int e = roadNetwork.crossroads.IndexOf(roadNetwork.roadSegments[i].end);
            connectClosest(vertices, indices, s, e);
        }

        mesh.SetVertices(vertices);
        List<int> reversedIndices = new List<int>(indices);
        reversedIndices.Reverse();
        indices.AddRange(reversedIndices);
        mesh.SetIndices(indices.ToArray(), MeshTopology.Quads, 0);
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
