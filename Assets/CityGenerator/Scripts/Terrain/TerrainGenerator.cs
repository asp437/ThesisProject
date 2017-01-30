using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TerrainGenerator : MonoBehaviour
{
    [System.Serializable]
    public struct NoiseConfiguration { public int amplitude; public float frequency; }

    public GameObject terrainObject;
    private int[] permutationTable; // Permutation table used for determined pseudorandom values generation
    public TerrainMeshGenerator meshGenerator;
    public int dimension;
    public List<NoiseConfiguration> noisesConfigurations; // Noise configuration

    // Functon for generation of noise with given frequency multiplier
    float[,] generateNoise(float mult)
    {
        float[,] noise = new float[dimension, dimension];
        for (int x = 0; x < dimension; x++)
            for (int y = 0; y < dimension; y++)
            {
                float r = PerlinNoise(new Vector2(x * mult / dimension, y * mult / dimension));
                r = (r + 1.0f) / 2.0f;
                noise[x, y] = r;
            }

        return noise;
    }

    public float[,] GenerateTerrain(int seed, GameObject inTerrainObject, CityGenerator generator)
    {
        if (inTerrainObject != null)
        {
            terrainObject = inTerrainObject;
        }
        if (terrainObject == null)
        {
            terrainObject = new GameObject("Terrain");
        }
        float[,] terrainMap = new float[dimension, dimension];
        Random.InitState(seed);
        int permutationTableSize = (dimension + 1) * (dimension + 1);
        permutationTable = new int[permutationTableSize];
        for (int i = 0; i < permutationTableSize; i++)
        {
            permutationTable[i] = ((int)(Random.value * Mathf.Pow(2, 30)));
        }

        List<float> amplitudes = new List<float>();
        List<float[,]> noises = new List<float[,]>();
        for (int i = 0; i < noisesConfigurations.Count; i++)
        {
            noises.Add(generateNoise(noisesConfigurations[i].frequency));
            amplitudes.Add(noisesConfigurations[i].amplitude);
        }

        float totalAmplitude = 0.0f;
        for (int i = 0; i < noises.Count; i++)
        {
            totalAmplitude += amplitudes[i];
        }
        for (int x = 0; x < dimension; x++)
            for (int y = 0; y < dimension; y++)
            {
                float r = 0.0f;
                for (int i = 0; i < noises.Count; i++)
                {
                    r += noises[i][x, y] * amplitudes[i];
                }
                r = r / totalAmplitude; // Normalize sum of noises
                r = MathHelper.quintic(MathHelper.quintic(r)); // Apply non-linear filter
                terrainMap[x, y] = r;
            }
        meshGenerator.GenerateMesh(terrainObject, terrainMap, generator.meshDimension);
        return terrainMap;
    }

    public GameObject GetTerrainObject()
    {
        return terrainObject;
    }

    // Get pseudorandom gradient for each keypoint of the noise
    Vector2 getRandomGradient(int x, int y)
    {
        Random.InitState(permutationTable[x * dimension + y]);
        Vector2 result = new Vector2((Random.value - 0.5f) * 2.0f, (Random.value - 0.5f) * 2.0f);
        return result.normalized;
    }

    private float PerlinNoise(Vector2 vec)
    {
        // Calculate position of nearest keypoint in up/left direction and distance in quad
        int left = Mathf.FloorToInt(vec.x), top = Mathf.FloorToInt(vec.y);
        Vector2 positionInQuad = new Vector2(vec.x - left, vec.y - top);
        // Get pseudorandom gradients for near keypoints
        Vector2 gradientTL = getRandomGradient(left, top);
        Vector2 gradientTR = getRandomGradient(left + 1, top);
        Vector2 gradientBL = getRandomGradient(left, top + 1);
        Vector2 gradientBR = getRandomGradient(left + 1, top + 1);
        // Calculate distance vectors to each of near keypoints
        Vector2 distanceToTL = new Vector2(positionInQuad.x, positionInQuad.y);
        Vector2 distanceToTR = new Vector2(positionInQuad.x - 1, positionInQuad.y);
        Vector2 distanceToBL = new Vector2(positionInQuad.x, positionInQuad.y - 1);
        Vector2 distanceToBR = new Vector2(positionInQuad.x - 1, positionInQuad.y - 1);
        // Calculate distance to each keypoint's gradient vector
        float tx1 = Vector2.Dot(distanceToTL, gradientTL);
        float tx2 = Vector2.Dot(distanceToTR, gradientTR);
        float bx1 = Vector2.Dot(distanceToBL, gradientBL);
        float bx2 = Vector2.Dot(distanceToBR, gradientBR);
        // Make interpolation non-linear
        float qx = MathHelper.quintic(positionInQuad.x), qy = MathHelper.quintic(positionInQuad.y);
        // Interpolate value
        float tx = MathHelper.lerp(tx1, tx2, qx), bx = MathHelper.lerp(bx1, bx2, qx);
        float tb = MathHelper.lerp(tx, bx, qy);
        return tb;
    }
}
