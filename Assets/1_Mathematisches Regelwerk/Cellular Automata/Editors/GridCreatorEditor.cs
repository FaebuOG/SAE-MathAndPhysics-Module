using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GridCreator))]
[CanEditMultipleObjects]
public class GridCreatorEditor : Editor
{
    [Header("Array Values")]
    SerializedProperty Width;
    SerializedProperty Height;

    [Header("Probabilitys")]
    SerializedProperty StartingProbability;
    SerializedProperty BurningProbability;
    
    [Header("Prefabs")]
    SerializedProperty CellPrefab;
    SerializedProperty GridPrefab;
    GridCreator _target;
    
    private void OnEnable()
    {
        Width = serializedObject.FindProperty("width");
        Height = serializedObject.FindProperty("height");

        StartingProbability = serializedObject.FindProperty("StartingProbability");
        BurningProbability = serializedObject.FindProperty("BurningProbability");
        
        CellPrefab = serializedObject.FindProperty("cellPrefab");
        GridPrefab = serializedObject.FindProperty("gridPrefab");
        _target = (GridCreator)target;
    }
    
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(Width);
        EditorGUILayout.PropertyField(Height);
        
        EditorGUILayout.PropertyField(StartingProbability);
        EditorGUILayout.PropertyField(BurningProbability);

        EditorGUILayout.PropertyField(CellPrefab);
        EditorGUILayout.PropertyField(GridPrefab);
        
        if (GUILayout.Button(new GUIContent("GenerateGrid", "generate a new grid")) == true)
            _target.GenerateGrid();
        if (GUILayout.Button(new GUIContent("DeleteGrid", "delete the current grid")) == true)
            _target.DeleteGrid();
        serializedObject.ApplyModifiedProperties();
    }
}
