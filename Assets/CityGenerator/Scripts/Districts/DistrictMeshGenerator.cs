using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistrictMeshGenerator : MonoBehaviour
{
    public Material districtMaterial;
    public float heightScale = 0.005f;
    public float roadEdgeOffset = 0.05f;

    public void generateMesh(GameObject districtsParentGameObject, List<District> districtsMap, CityGenerator generator)
    {
        int districtNumber = 1;
        foreach (District district in districtsMap)
        {
            Mesh mesh = new Mesh();
            List<Vector3> vertices = new List<Vector3>();
            List<int> indices = new List<int>();
            GameObject districtGameObject = new GameObject("District #" + districtNumber.ToString());
            districtNumber++;
            districtGameObject.transform.parent = districtsParentGameObject.transform;
            foreach (DistrictCell cell in district.cells)
            {
                Vector3 ul = new Vector3(cell.x, generator.getPointHeight(cell.x, cell.y) + 1.0f * heightScale, cell.y);
                Vector3 ur = new Vector3(cell.x + 1, generator.getPointHeight(cell.x + 1, cell.y) + 1.0f * heightScale, cell.y);
                Vector3 br = new Vector3(cell.x + 1, generator.getPointHeight(cell.x + 1, cell.y + 1) + 1.0f * heightScale, cell.y + 1);
                Vector3 bl = new Vector3(cell.x, generator.getPointHeight(cell.x, cell.y + 1) + 1.0f * heightScale, cell.y + 1);
                if (cell.edgeLeft)
                {
                    ul.x += 1.0f * roadEdgeOffset;
                    bl.x += 1.0f * roadEdgeOffset;
                }
                if (cell.edgeRight)
                {
                    ur.x -= 1.0f * roadEdgeOffset;
                    br.x -= 1.0f * roadEdgeOffset;
                }
                if (cell.edgeUp)
                {
                    ul.z += 1.0f * roadEdgeOffset;
                    ur.z += 1.0f * roadEdgeOffset;
                }
                if (cell.edgeBottom)
                {
                    bl.z -= 1.0f * roadEdgeOffset;
                    br.z -= 1.0f * roadEdgeOffset;
                }
                vertices.Add(ul);
                vertices.Add(ur);
                vertices.Add(br);
                vertices.Add(bl);
                for (int i = 1; i <= 4; i++)
                {
                    indices.Add(vertices.Count - i);
                }
            }
            mesh.SetVertices(vertices);
            mesh.SetIndices(indices.ToArray(), MeshTopology.Quads, 0);
            if (districtGameObject.GetComponent<MeshFilter>() == null)
            {
                districtGameObject.AddComponent<MeshFilter>();
            }
            if (districtGameObject.GetComponent<MeshRenderer>() == null)
            {
                districtGameObject.AddComponent<MeshRenderer>();
            }
            ((MeshFilter)(districtGameObject.GetComponent<MeshFilter>())).mesh = mesh;
            Material material = new Material(districtMaterial);
            material.SetColor("_Color", Random.ColorHSV());
            ((MeshRenderer)(districtGameObject.GetComponent<MeshRenderer>())).material = material;
        }
    }
}
