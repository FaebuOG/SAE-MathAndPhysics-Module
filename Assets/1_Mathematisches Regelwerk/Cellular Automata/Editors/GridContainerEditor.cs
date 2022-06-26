using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GridContainer))]
[CanEditMultipleObjects]
public class GridContainerEditor : Editor
{
    SerializedProperty Width;
    SerializedProperty Height;
    SerializedProperty CellPrefab;

    private void OnEnable()
    {
        Width = serializedObject.FindProperty("width");
        Height = serializedObject.FindProperty("height");
        CellPrefab = serializedObject.FindProperty("cellPrefab");
    }
    //public override void OnInspectorGUI()
    //{
    //    serializedObject.Update();

    //}
}
