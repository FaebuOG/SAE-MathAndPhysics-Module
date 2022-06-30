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
}
