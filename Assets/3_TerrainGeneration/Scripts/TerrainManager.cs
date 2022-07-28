using System;
using UnityEngine;

public class TerrainManager : MonoBehaviour
{
    private enum DrawMode
    {
        NoiseMap,
        ColourMap,
        DrawMesh
    }
    [SerializeField] private DrawMode drawMode;
    [SerializeField] private TerrainScriptableObject terrainScriptableObject;
    
    [Range(0,100)] [SerializeField] private int terrainSize;
    [Range(0,100)] [SerializeField] private float noiseScale;
    [Range(0,  1)] [SerializeField] private float persistance;
    [Range(0,  5)] [SerializeField] private float lacunarity;
    [Range(0, 10)] [SerializeField] private int octaves;
    private int terrainWidth;
    private int terrainHeight;
    
    [Header("Mesh Settings")]
    [Tooltip("Changes the terrain height")]
    [SerializeField] private float meshHeightMultiplier;
    [Tooltip("Changes how much the Mesh Height Multiplier affects the mesh at certain regions")]
    [SerializeField] private AnimationCurve meshHeightCurve;
    
    [SerializeField] private TerrainType[] regions;
    [SerializeField] private Vector2 offset;
    [SerializeField] private int seed;
    
    [Header("Inspector")] // for the editor
    public bool AutoUpdate;

    public void Start()
    {
        GenerateTerrain();
    }
    
    #region Enable/Disable
    public virtual void OnEnable()
    {
        SetupTerrainFromConfiguration();
    }
    // Gets all the values from the Titan Scriptable Object
    public virtual void SetupTerrainFromConfiguration()
    {
        terrainSize = terrainScriptableObject.TerrainSize;
        noiseScale = terrainScriptableObject.NoiseScale;
        persistance = terrainScriptableObject.Persistance;
        lacunarity = terrainScriptableObject.Lacunarity;
        octaves = terrainScriptableObject.Octaves;
    
        meshHeightMultiplier = terrainScriptableObject.MeshHeightMultiplier;
        meshHeightCurve = terrainScriptableObject.MeshHeightCurve;
    
        regions = terrainScriptableObject.Regions;
        offset = terrainScriptableObject.Offset;
        seed = terrainScriptableObject.Seed;
    }
    #endregion

    
    
    public void GenerateTerrain()
    {
        // Map (width & height) needs to be identical 
        terrainWidth = terrainSize;
        terrainHeight = terrainSize;
        
        // Create a noise map with parameters from inspector
        float[,] noiseMap = Noise.GenerateNoiseMap(terrainWidth, terrainHeight, seed, noiseScale, octaves, persistance, lacunarity, offset);

        // Terrain type color
        Color[] colourMap = new Color[terrainWidth * terrainHeight];
        for (int y = 0; y < terrainHeight; y++)
        {
            for (int x = 0; x < terrainWidth; x++)
            {
                float currentHeight = noiseMap[x, y];
                for (int i = 0; i < regions.Length; i++)
                {
                    if (currentHeight <= regions[i].Height)
                    {
                        colourMap[y * terrainWidth + x] = regions[i].Colour;
                        break;
                    }
                }
            }
        }
        
        // Displays the correct map based on what is selected in the Inspector
        MapDisplay mapDisplay = FindObjectOfType<MapDisplay>();
        if (drawMode == DrawMode.NoiseMap)
        {
            mapDisplay.DrawTexture(TextureGenerator.TextureFromHeightMap(noiseMap));
        }
        else if (drawMode == DrawMode.ColourMap)
        {
            mapDisplay.DrawTexture(TextureGenerator.TextureFromColourMap(colourMap, terrainWidth, terrainHeight));
        }
        else if (drawMode == DrawMode.DrawMesh)
        {
            mapDisplay.DrawMesh(MeshGenerator.GenerateTerrainMesh(noiseMap, meshHeightMultiplier, meshHeightCurve),
                TextureGenerator.TextureFromColourMap(colourMap, terrainWidth, terrainHeight));
        }
        
    }

    private void OnValidate()
    {
        // Limits and instant fix
        if (terrainWidth < 1)
            terrainWidth = 1;
        if (terrainHeight < 1)
            terrainHeight = 1;
        if (lacunarity < 1)
            lacunarity = 1;
        if (octaves < 0)
            octaves = 0;
    }
    
    
}

[System.Serializable]
public struct TerrainType
{
    public string Name;
    public float Height;
    public Color Colour;
}
