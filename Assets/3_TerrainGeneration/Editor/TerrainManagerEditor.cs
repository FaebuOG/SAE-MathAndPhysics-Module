using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TerrainManager))]
public class TerrainManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        TerrainManager terrainManager = (TerrainManager) target;

        // Automatic Update of the map when values changed
        if (DrawDefaultInspector())
        {
            if (terrainManager.AutoUpdate)
            {
                terrainManager.GenerateTerrain();
            }
                
        }
        if (GUILayout.Button("Generate Map"))
        {
            terrainManager.GenerateTerrain();
        }
    }
}
