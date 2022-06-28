using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waves : MonoBehaviour
{
    public int Dimension = 10;
    public Octave[] Octaves;
    
    protected MeshFilter MeshFilter;
    protected Mesh Mesh;
    
    
    // Start is called before the first frame update
    void Start()
    {
        // Mesh Setup
        Mesh = new Mesh();
        Mesh.name = gameObject.name;

        Mesh.vertices = GenerateVerts();
        Mesh.triangles = GenerateTries();
        Mesh.RecalculateBounds();

        MeshFilter = gameObject.AddComponent<MeshFilter>();
        MeshFilter.mesh = Mesh;
    }

    private Vector3[] GenerateVerts()
    {
        var verts = new Vector3[(Dimension + 1) * (Dimension + 1)];
        
        // equaly distributed verts
        for (int x = 0; x <= Dimension; x++)
            for (int z = 0; z <= Dimension; z++)
            verts[index(x, z)] = new Vector3(x, 0, z);
        
        return verts;
    }

    private int index(int x, int z)
    {
        return x * (Dimension + 1) + z;
    }

    private int[] GenerateTries()
    {
        var tries = new int[Mesh.vertices.Length * 6];
        
        // two triangles are on the tile
        for (int x = 0; x < Dimension; x++)
        {
            for (int z = 0; z < Dimension; z++)
            {
                // goes trough all 6 vertices points from Mesh
                tries[index(x, z) * 6 + 0] = index(x, z);
                tries[index(x, z) * 6 + 1] = index(x + 1,z + 1);
                tries[index(x, z) * 6 + 2] = index(x + 1, z);
                tries[index(x, z) * 6 + 3] = index(x, z);
                tries[index(x, z) * 6 + 4] = index(x, z + 1);
                tries[index(x, z) * 6 + 5] = index(x + 1,z + 1);
            }
        }

        return tries;
    }

    // Update is called once per frame
    void Update()
    {
        var verts = Mesh.vertices;
        for (int x = 0; x <= Dimension; x++)
        {
            for (int z = 0; z <= Dimension; z++)
            {
                var y = 0f;
                for (int o = 0; o < Octaves.Length; o++)
                {
                    if (Octaves[o].alternate)
                    {
                        var per1 = Mathf.PerlinNoise((x * Octaves[o].scale.x) / Dimension, (z * Octaves[o].scale.y) / Dimension) * Mathf.PI * 2f;
                        
                        // Adds a cosinus wave
                        y += Mathf.Cos(Octaves[o].speed.magnitude * Time.time) * Octaves[o].height;
                    }
                }
                
                verts[index(x, z)] = new Vector3(x, y, z);
            }
        }

        Mesh.vertices = verts;
    }

    [Serializable] public struct Octave
    {
        public Vector2 speed;
        public Vector2 scale;
        public float height;
        public bool alternate;
    }
}
