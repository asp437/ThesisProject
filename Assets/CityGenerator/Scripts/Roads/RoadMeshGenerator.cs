using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoadMeshGenerator : MonoBehaviour
{
    public const float scaleMultiplier = 0.05f;
    public Material crossroadMaterial;
    public Material roadSegmentMaterial;
    private const int verticesPerCross = 4;
    private const float heightScale = 0.005f;
    public TerrainMeshGenerator terrainMeshGenerator;

    protected void connectClosest(List<Vector3> crossroadsVertices, List<Vector3> segmentsVertices, List<Vector2> uvs,
        List<int> indices, int s, int e, CityGenerator cityGenerator)
    {
        List<Vector3> sVertices = new List<Vector3>();
        List<Vector3> eVertices = new List<Vector3>();
        for (int i = 0; i < 4; i++)
        {
            sVertices.Add(crossroadsVertices[verticesPerCross * s + i]);
            eVertices.Add(crossroadsVertices[verticesPerCross * e + i]);
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

        //int subsegmentsCount = (int) Mathf.Abs(Vector3.Distance(crossroadsVertices[verticesPerCross * s + closest3],
        //    crossroadsVertices[verticesPerCross * e + closest1]));
        int subsegmentsCount = 0;
        Vector3 direction = new Vector3();
        if (crossroadsVertices[verticesPerCross * e + closest0].x - crossroadsVertices[verticesPerCross * s + closest3].x >= 0.9f)
        {
            direction = new Vector3(1.0f, 0.0f, 0.0f);
            subsegmentsCount = (int)(crossroadsVertices[verticesPerCross * e + closest0].x - crossroadsVertices[verticesPerCross * s + closest3].x);
        }
        else if (crossroadsVertices[verticesPerCross * e + closest0].x - crossroadsVertices[verticesPerCross * s + closest3].x <= -0.9f)
        {
            direction = new Vector3(-1.0f, 0.0f, 0.0f);
            subsegmentsCount = -(int)(crossroadsVertices[verticesPerCross * e + closest0].x - crossroadsVertices[verticesPerCross * s + closest3].x);
        }
        else if (crossroadsVertices[verticesPerCross * e + closest0].y - crossroadsVertices[verticesPerCross * s + closest3].y >= 0.9f)
        {
            direction = new Vector3(0.0f, 1.0f, 0.0f);
            subsegmentsCount = (int)(crossroadsVertices[verticesPerCross * e + closest0].y - crossroadsVertices[verticesPerCross * s + closest3].y);
        }
        else if (crossroadsVertices[verticesPerCross * e + closest0].y - crossroadsVertices[verticesPerCross * s + closest3].y <= -0.9f)
        {
            direction = new Vector3(0.0f, -1.0f, 0.0f);
            subsegmentsCount = -(int)(crossroadsVertices[verticesPerCross * e + closest0].y - crossroadsVertices[verticesPerCross * s + closest3].y);
        }

        int subsegment;
        Vector3 vector;
        for (subsegment = 0; subsegment < subsegmentsCount; subsegment++)
        {
            vector = crossroadsVertices[verticesPerCross * s + closest3] + direction * subsegment;
            vector.y = cityGenerator.getPointHeight(vector.x, vector.z) + 1.0f * heightScale;
            segmentsVertices.Add(vector);

            vector = crossroadsVertices[verticesPerCross * s + closest2] + direction * subsegment;
            vector.y = cityGenerator.getPointHeight(vector.x, vector.z) + 1.0f * heightScale;
            segmentsVertices.Add(vector);

            vector = crossroadsVertices[verticesPerCross * s + closest3] + direction * (subsegment + 1);
            vector.y = cityGenerator.getPointHeight(vector.x, vector.z) + 1.0f * heightScale;
            segmentsVertices.Add(vector);

            vector = crossroadsVertices[verticesPerCross * s + closest2] + direction * (subsegment + 1);
            vector.y = cityGenerator.getPointHeight(vector.x, vector.z) + 1.0f * heightScale;
            segmentsVertices.Add(vector);

            indices.Add(segmentsVertices.Count - 4);
            indices.Add(segmentsVertices.Count - 3);
            indices.Add(segmentsVertices.Count - 2);
            indices.Add(segmentsVertices.Count - 1);

            uvs.Add(new Vector2(0.0f, 0.0f));
            uvs.Add(new Vector2(0.0f, 1.0f));
            uvs.Add(new Vector2(1.0f, 1.0f));
            uvs.Add(new Vector2(1.0f, 0.0f));
        }

        vector = crossroadsVertices[verticesPerCross * s + closest3] + direction * subsegment;
        vector.y = cityGenerator.getPointHeight(vector.x, vector.z) + 1.0f * heightScale;
        segmentsVertices.Add(vector);

        vector = crossroadsVertices[verticesPerCross * s + closest2] + direction * subsegment;
        vector.y = cityGenerator.getPointHeight(vector.x, vector.z) + 1.0f * heightScale;
        segmentsVertices.Add(vector);

        segmentsVertices.Add(crossroadsVertices[verticesPerCross * e + closest0]);
        segmentsVertices.Add(crossroadsVertices[verticesPerCross * e + closest1]);
        
        indices.Add(segmentsVertices.Count - 4);
        indices.Add(segmentsVertices.Count - 3);
        indices.Add(segmentsVertices.Count - 2);
        indices.Add(segmentsVertices.Count - 1);

        uvs.Add(new Vector2(0.0f, 0.0f));
        uvs.Add(new Vector2(0.0f, 1.0f));
        uvs.Add(new Vector2(1.0f, 1.0f));
        uvs.Add(new Vector2(1.0f, 0.0f));

    }

    protected void generateSegmentsMesh(List<Vector3> crossroadsVertices, GameObject segmentsGO, RoadNetwork roadNetwork, CityGenerator cityGenerator)
    {
        Mesh segmentsMesh = new Mesh();
        List<Vector2> segmentsUVs = new List<Vector2>();
        List<int> segmentsIndices = new List<int>();
        List<Vector3> vertices = new List<Vector3>();

        for (int i = 0; i < roadNetwork.roadSegments.Count; i++)
        {
            int s = roadNetwork.crossroads.IndexOf(roadNetwork.roadSegments[i].start);
            int e = roadNetwork.crossroads.IndexOf(roadNetwork.roadSegments[i].end);
            connectClosest(crossroadsVertices, vertices, segmentsUVs, segmentsIndices, s, e, cityGenerator);
        }

        segmentsMesh.SetVertices(vertices);
        List<int> reversedIndices = new List<int>(segmentsIndices);
        reversedIndices.Reverse();
        segmentsIndices.AddRange(reversedIndices);

        segmentsMesh.SetIndices(segmentsIndices.ToArray(), MeshTopology.Quads, 0);
        segmentsMesh.RecalculateNormals();
        segmentsMesh.RecalculateBounds();
        segmentsMesh.SetUVs(0, segmentsUVs);


        if (segmentsGO.GetComponent<MeshFilter>() == null)
        {
            segmentsGO.AddComponent<MeshFilter>();
        }
        if (segmentsGO.GetComponent<MeshRenderer>() == null)
        {
            segmentsGO.AddComponent<MeshRenderer>();
        }
        ((MeshFilter)(segmentsGO.GetComponent<MeshFilter>())).mesh = segmentsMesh;
        ((MeshRenderer)(segmentsGO.GetComponent<MeshRenderer>())).material = roadSegmentMaterial;
    }

    public void generateMesh(GameObject crossroadsGO, GameObject segmentsGO, RoadNetwork roadNetwork, CityGenerator cityGenerator)
    {
        Mesh crossroadsMesh = new Mesh();
        List<Vector3> vertices = new List<Vector3>();
        List<Vector2> crossroadsUVs = new List<Vector2>();
        List<int> crossroadsIndices = new List<int>();

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

        crossroadsMesh.SetVertices(vertices);

        List<int> reversedIndices = new List<int>(crossroadsIndices);
        reversedIndices.Reverse();
        crossroadsIndices.AddRange(reversedIndices);

        crossroadsMesh.SetIndices(crossroadsIndices.ToArray(), MeshTopology.Quads, 0);
        crossroadsMesh.RecalculateNormals();
        crossroadsMesh.RecalculateBounds();
        crossroadsMesh.SetUVs(0, crossroadsUVs);

        if (crossroadsGO.GetComponent<MeshFilter>() == null)
        {
            crossroadsGO.AddComponent<MeshFilter>();
        }
        if (crossroadsGO.GetComponent<MeshRenderer>() == null)
        {
            crossroadsGO.AddComponent<MeshRenderer>();
        }
        ((MeshFilter)(crossroadsGO.GetComponent<MeshFilter>())).mesh = crossroadsMesh;
        ((MeshRenderer)(crossroadsGO.GetComponent<MeshRenderer>())).material = crossroadMaterial;
        generateSegmentsMesh(vertices, segmentsGO, roadNetwork, cityGenerator);
    }
}
