using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{

    public enum DrawMode
    {
        NoiseMap,
        ColourMap,
        DrawMesh
    }

    [SerializeField] private DrawMode drawMode;
    
    [Header("Perlin Noise Values")]
    [Range(0,100)] public int MapWidth;
    [Range(0,100)] public int MapHeight;
    [Range(0,100)] public float NoiseScale;
    
    [Range(0,  1)] public float Persistance;
    public float Lacunarity;
    public int Octaves;
    public int Seed;
    public Vector2 Offset;
    public bool AutoUpdate;
    public TerrainType[] Regions;
    
    
    
    public void GenerateMap()
    {
        // Create a noise map with parameters from inspector
        float[,] noiseMap = Noise.GenerateNoiseMap(MapWidth, MapHeight, Seed, NoiseScale, Octaves, Persistance, Lacunarity, Offset);

        // Terrain Type
        Color[] colourMap = new Color[MapWidth * MapHeight];
        for (int y = 0; y < MapHeight; y++)
        {
            for (int x = 0; x < MapWidth; x++)
            {
                float currentHeight = noiseMap[x, y];
                for (int i = 0; i < Regions.Length; i++)
                {
                    if (currentHeight <= Regions[i].Height)
                    {
                        colourMap[y * MapWidth + x] = Regions[i].Colour;
                        
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
            mapDisplay.DrawTexture(TextureGenerator.TextureFromColourMap(colourMap, MapWidth, MapHeight));
        }
        else if (drawMode == DrawMode.DrawMesh)
        {
            mapDisplay.DrawMesh(MeshGenerator.GenerateTerrainMesh(noiseMap),
                TextureGenerator.TextureFromColourMap(colourMap, MapWidth, MapHeight));
        }
        
    }

    private void OnValidate()
    {
        // Limits and instant fix
        if (MapWidth < 1)
            MapWidth = 1;
        if (MapHeight < 1)
            MapHeight = 1;
        if (Lacunarity < 1)
            Lacunarity = 1;
        if (Octaves < 0)
            Octaves = 0;
    }
}

[System.Serializable]
public struct TerrainType
{
    public string Name;
    public float Height;
    public Color Colour;
}
