using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
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

    
    
    public void GenerateMap()
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(MapWidth, MapHeight, Seed, NoiseScale, Octaves, Persistance, Lacunarity, Offset);

        MapDisplay mapDisplay = FindObjectOfType<MapDisplay>();
        mapDisplay.DrawNoiseMap(noiseMap);
    }

    private void OnValidate()
    {
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
