using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "Terrain Types", menuName = "ScriptableObjects/Terrain")]
public class TerrainScriptableObject : ScriptableObject
{
    [Header("Perlin Noise Values")]
    [Tooltip("Changes x & y values of the map")]
    [Range(0,100)] public int TerrainSize;
    [Tooltip("Changes the noise from perlin noise")]
    [Range(0,100)] public float NoiseScale;
    [Tooltip("Persistance controls the amplitude from an octave")]
    [Range(0,  1)] public float Persistance;
    [Tooltip("Lacunarity controls the frequency from an octave")]
    [Range(0,  5)] public float Lacunarity;
    [Tooltip("Use more octaves for more detail")]
    [Range(0, 10)] public int Octaves;

    [Header("Mesh Settings")]
    [Tooltip("Changes the terrain height")]
    public float MeshHeightMultiplier;
    [Tooltip("Changes how much the Mesh Height Multiplier affects the mesh at certain regions")]
    public AnimationCurve MeshHeightCurve;
    
    [Header("Terrain Settings")]
    [Tooltip("Color settings for different height regions")]
    public TerrainType[] Regions;
    [Tooltip("Moves the landscape with offset values")]
    public Vector2 Offset;
    public int Seed;
}
