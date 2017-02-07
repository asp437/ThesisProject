using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistrictMeshGenerator : MonoBehaviour
{
    public Material districtMaterial;
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
            foreach (Vector2 cell in district.cells)
            {
                vertices.Add(new Vector3(cell.x, generator.getPointHeight(cell.x, cell.y), cell.y));
                vertices.Add(new Vector3(cell.x + 1, generator.getPointHeight(cell.x + 1, cell.y), cell.y));
                vertices.Add(new Vector3(cell.x + 1, generator.getPointHeight(cell.x + 1, cell.y + 1), cell.y + 1));
                vertices.Add(new Vector3(cell.x, generator.getPointHeight(cell.x, cell.y + 1), cell.y + 1));
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
