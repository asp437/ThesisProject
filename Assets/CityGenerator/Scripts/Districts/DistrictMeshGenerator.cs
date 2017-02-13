using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct DistrictBuildingMesh
{
    public GameObject prefab;
    public DistrictType type;
}

public class DistrictMeshGenerator : MonoBehaviour
{
    public Material districtMaterial;
    public float heightScale = 0.005f;
    public float roadEdgeOffset = 0.05f;
    public DistrictBuildingMesh[] residentialBuildings;
    public DistrictBuildingMesh[] commercialBuildings;
    public DistrictBuildingMesh[] industrialBuildings;
    public DistrictBuildingMesh[] recreationalBuildings;
    public bool generateBuildings = true;
    public bool generatePlanes = true;
    public Color residentialZoneColor;
    public Color commercialZoneColor;
    public Color industrialZoneColor;
    public Color recreationalZoneColor;

    public void generateMesh(GameObject districtsParentGameObject, List<District> districtsMap, CityGenerator generator)
    {
        int districtNumber = 1;
        foreach (District district in districtsMap)
        {
            Debug.Log("District type: " + district.type.ToString());
            Mesh mesh = new Mesh();
            List<Vector3> vertices = new List<Vector3>();
            List<int> indices = new List<int>();
            GameObject districtGameObject = new GameObject("District #" + districtNumber.ToString());
            districtNumber++;
            districtGameObject.transform.parent = districtsParentGameObject.transform;
            Color districtColor = new Color();
            switch (district.type)
            {
                case DistrictType.RESIDENTIAL:
                    districtColor = residentialZoneColor;
                    break;
                case DistrictType.INDUSTRIAL:
                    districtColor = industrialZoneColor;
                    break;
                case DistrictType.RECREATIONAL:
                    districtColor = recreationalZoneColor;
                    break;
                case DistrictType.COMMERCIAL:
                    districtColor = commercialZoneColor;
                    break;
            }
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
                if (generateBuildings)
                {
                    float maxHeight = 0.0f;
                    maxHeight = Mathf.Max(maxHeight, generator.getPointHeight(cell.x, cell.y) + 1.0f * heightScale);
                    maxHeight = Mathf.Max(maxHeight, generator.getPointHeight(cell.x + 1, cell.y) + 1.0f * heightScale);
                    maxHeight = Mathf.Max(maxHeight, generator.getPointHeight(cell.x + 1, cell.y + 1) + 1.0f * heightScale);
                    maxHeight = Mathf.Max(maxHeight, generator.getPointHeight(cell.x, cell.y + 1) + 1.0f * heightScale);
                    GameObject buildingGameObject = null;
                    if (district.type == DistrictType.RESIDENTIAL && residentialBuildings.Length > 0)
                    {
                        int index = (int)Random.Range(0, residentialBuildings.Length - 0.5f);
                        buildingGameObject = Instantiate(residentialBuildings[index].prefab, districtGameObject.transform);
                    }
                    if (district.type == DistrictType.COMMERCIAL && commercialBuildings.Length > 0)
                    {
                        int index = (int)Random.Range(0, commercialBuildings.Length - 0.5f);
                        buildingGameObject = Instantiate(commercialBuildings[index].prefab, districtGameObject.transform);
                    }
                    if (district.type == DistrictType.INDUSTRIAL && industrialBuildings.Length > 0)
                    {
                        int index = (int)Random.Range(0, industrialBuildings.Length - 0.5f);
                        buildingGameObject = Instantiate(industrialBuildings[index].prefab, districtGameObject.transform);
                    }
                    if (district.type == DistrictType.RECREATIONAL && recreationalBuildings.Length > 0)
                    {
                        int index = (int)Random.Range(0, recreationalBuildings.Length - 0.5f);
                        buildingGameObject = Instantiate(recreationalBuildings[index].prefab, districtGameObject.transform);
                    }
                    if (buildingGameObject != null)
                        buildingGameObject.transform.position = new Vector3(cell.x, maxHeight, cell.y);
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
            if (generatePlanes)
            {
                ((MeshFilter)(districtGameObject.GetComponent<MeshFilter>())).mesh = mesh;
                Material material = new Material(districtMaterial);
                material.SetColor("_Color", districtColor);
                ((MeshRenderer)(districtGameObject.GetComponent<MeshRenderer>())).material = material;
            }
        }
    }
}
