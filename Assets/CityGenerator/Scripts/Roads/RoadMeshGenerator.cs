using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoadMeshGenerator : MonoBehaviour
{
    public const float scaleMultiplier = 0.05f;
    public Material roadMaterial;
    private const int verticesPerCross = 4;
    private const float heightScale = 0.005f;
    public TerrainMeshGenerator terrainMeshGenerator;

    protected void connectClosest(List<Vector3> vertices, List<Vector2> uvs, List<int> indices, int s, int e)
    {
        List<Vector3> sVertices = new List<Vector3>();
        List<Vector3> eVertices = new List<Vector3>();
        for (int i = 0; i < 4; i++)
        {
            sVertices.Add(vertices[verticesPerCross * s + i]);
            eVertices.Add(vertices[verticesPerCross * e + i]);
        }
        Vector3 v = sVertices[0];
        int closest0 = 0; float d0 = Vector3.Distance(v, eVertices[0]);
        int closest1 = 1; float d1 = Vector3.Distance(v, eVertices[1]);
        for (int j = 1; j < 4; j++)
        {
            float d = Vector3.Distance(v, eVertices[j]);
            if (d < d0)
            {
                d1 = d0;
                closest1 = closest0;
                d0 = d;
                closest0 = j;
            }
            else if (d < d1)
            {
                closest1 = j;
                d1 = d;
            }
        }
        int closest2 = 0; float d2 = Vector3.Distance(eVertices[closest0], sVertices[0]);
        int closest3 = 1; float d3 = Vector3.Distance(eVertices[closest0], sVertices[1]);
        for (int j = 1; j < 4; j++)
        {
            float d = Vector3.Distance(eVertices[closest0], sVertices[j]);
            if (d < d2)
            {
                d3 = d2;
                closest3 = closest2;
                d2 = d;
                closest2 = j;
            }
            else if (d < d3)
            {
                closest3 = j;
                d3 = d;
            }
        }

        indices.Add(verticesPerCross * s + closest3);
        indices.Add(verticesPerCross * s + closest2);
        indices.Add(verticesPerCross * e + closest0);
        indices.Add(verticesPerCross * e + closest1);
        uvs[verticesPerCross * s + closest3] = new Vector2(0.0f, 0.0f);
        uvs[verticesPerCross * s + closest2] = new Vector2(1.0f, 1.0f);
        uvs[verticesPerCross * s + closest0] = new Vector2(0.0f, 1.0f);
        uvs[verticesPerCross * s + closest1] = new Vector2(1.0f, 0.0f);
    }

    public void generateMesh(GameObject crossroadsGO, GameObject segmentsGO, RoadNetwork roadNetwork, CityGenerator cityGenerator)
    {
        Mesh crossroadsMesh = new Mesh();
        Mesh segmentsMesh = new Mesh();
        List<Vector3> vertices = new List<Vector3>();
        List<Vector2> crossroadsUVs = new List<Vector2>();
        List<Vector2> segmentsUVs;
        List<int> crossroadsIndices = new List<int>();
        List<int> segmentsIndices = new List<int>();

        for (int i = 0; i < roadNetwork.crossroads.Count; i++)
        {
            Vector3 v0 = new Vector3(), v1 = new Vector3(), v2 = new Vector3(), v3 = new Vector3();
            v0.x = roadNetwork.crossroads[i].x - 1 * scaleMultiplier;
            v0.z = roadNetwork.crossroads[i].y - 1 * scaleMultiplier;
            v0.y = cityGenerator.getPointHeight(v0.x, v0.z) + 1.0f * heightScale;

            v1.x = roadNetwork.crossroads[i].x - 1 * scaleMultiplier;
            v1.z = roadNetwork.crossroads[i].y + 1 * scaleMultiplier;
            v1.y = cityGenerator.getPointHeight(v1.x, v1.z) + 1.0f * heightScale;

            v2.x = roadNetwork.crossroads[i].x + 1 * scaleMultiplier;
            v2.z = roadNetwork.crossroads[i].y + 1 * scaleMultiplier;
            v2.y = cityGenerator.getPointHeight(v2.x, v2.z) + 1.0f * heightScale;

            v3.x = roadNetwork.crossroads[i].x + 1 * scaleMultiplier;
            v3.z = roadNetwork.crossroads[i].y - 1 * scaleMultiplier;
            v3.y = cityGenerator.getPointHeight(v3.x, v3.z) + 1.0f * heightScale;

            vertices.Add(v0);
            vertices.Add(v1);
            vertices.Add(v2);
            vertices.Add(v3);
            crossroadsUVs.Add(new Vector2(0.0f, 0.0f));
            crossroadsUVs.Add(new Vector2(1.0f, 0.0f));
            crossroadsUVs.Add(new Vector2(1.0f, 1.0f));
            crossroadsUVs.Add(new Vector2(0.0f, 1.0f));
            crossroadsIndices.Add(verticesPerCross * i + 2);
            crossroadsIndices.Add(verticesPerCross * i + 3);
            crossroadsIndices.Add(verticesPerCross * i);
            crossroadsIndices.Add(verticesPerCross * i + 1);
        }
        segmentsUVs = new List<Vector2>(crossroadsUVs);
        for (int i = 0; i < roadNetwork.roadSegments.Count; i++)
        {
            int s = roadNetwork.crossroads.IndexOf(roadNetwork.roadSegments[i].start);
            int e = roadNetwork.crossroads.IndexOf(roadNetwork.roadSegments[i].end);
            connectClosest(vertices, segmentsUVs, segmentsIndices, s, e);
        }

        crossroadsMesh.SetVertices(vertices);
        segmentsMesh.SetVertices(vertices);

        List<int> reversedIndices = new List<int>(crossroadsIndices);
        reversedIndices.Reverse();
        crossroadsIndices.AddRange(reversedIndices);
        reversedIndices = new List<int>(segmentsIndices);
        reversedIndices.Reverse();
        segmentsIndices.AddRange(reversedIndices);

        crossroadsMesh.SetIndices(crossroadsIndices.ToArray(), MeshTopology.Quads, 0);
        crossroadsMesh.RecalculateNormals();
        crossroadsMesh.RecalculateBounds();
        crossroadsMesh.SetUVs(0, crossroadsUVs);

        segmentsMesh.SetIndices(segmentsIndices.ToArray(), MeshTopology.Quads, 0);
        segmentsMesh.RecalculateNormals();
        segmentsMesh.RecalculateBounds();
        segmentsMesh.SetUVs(0, segmentsUVs);

        if (crossroadsGO.GetComponent<MeshFilter>() == null)
        {
            crossroadsGO.AddComponent<MeshFilter>();
        }
        if (crossroadsGO.GetComponent<MeshRenderer>() == null)
        {
            crossroadsGO.AddComponent<MeshRenderer>();
        }
        ((MeshFilter)(crossroadsGO.GetComponent<MeshFilter>())).mesh = crossroadsMesh;
        ((MeshRenderer)(crossroadsGO.GetComponent<MeshRenderer>())).material = roadMaterial;

        if (segmentsGO.GetComponent<MeshFilter>() == null)
        {
            segmentsGO.AddComponent<MeshFilter>();
        }
        if (segmentsGO.GetComponent<MeshRenderer>() == null)
        {
            segmentsGO.AddComponent<MeshRenderer>();
        }
        ((MeshFilter)(segmentsGO.GetComponent<MeshFilter>())).mesh = segmentsMesh;
        ((MeshRenderer)(segmentsGO.GetComponent<MeshRenderer>())).material = roadMaterial;
    }
}
