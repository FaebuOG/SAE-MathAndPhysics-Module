using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FloatingObject))]
[CanEditMultipleObjects]
public class FloatingObjectEditor : Editor
{
        SerializedProperty floaters;
       SerializedProperty floatersUnderWater;
       SerializedProperty underWaterDrag;
       SerializedProperty underWaterAngularDrag;
       
       SerializedProperty airDrag;
       SerializedProperty airAngularDrag;
       SerializedProperty floatingPower;
       SerializedProperty waterHeight;
       
       SerializedProperty underWater;

       int selectionGridIndex = 0;
   
       private void OnEnable()
       {
           floaters = serializedObject.FindProperty("floaters");

           airDrag = serializedObject.FindProperty("airDrag");
           airAngularDrag = serializedObject.FindProperty("airAngularDrag");
           underWaterDrag = serializedObject.FindProperty("underWaterDrag");
           underWaterAngularDrag = serializedObject.FindProperty("underWaterAngularDrag");
           floatingPower = serializedObject.FindProperty("floatingPower");
           
           waterHeight = serializedObject.FindProperty("waterHeight");
           floatersUnderWater = serializedObject.FindProperty("floatersUnderWater");
           underWater = serializedObject.FindProperty("underWater");
           
       }

       public override void OnInspectorGUI()
       {
           serializedObject.Update();

           selectionGridIndex = GUILayout.SelectionGrid(selectionGridIndex,
               new string[] {"Physic Values", "Floaters"}, 2);

           if (selectionGridIndex == 0) // -> Normal GameObject
           {
               EditorGUILayout.LabelField("Physic Values");
               EditorGUILayout.PropertyField(airDrag);
               EditorGUILayout.PropertyField(airAngularDrag);
               EditorGUILayout.PropertyField(underWaterDrag);
               EditorGUILayout.PropertyField(underWaterAngularDrag);
               EditorGUILayout.PropertyField(floatingPower);
               GUILayout.Space(30f);
               
               EditorGUILayout.LabelField("Water Level");
               EditorGUILayout.PropertyField(waterHeight);
               EditorGUILayout.PropertyField(floatersUnderWater);
           }
           else if (selectionGridIndex == 1) // -> Ship
           {
               EditorGUILayout.LabelField("Floaters");
               
               EditorGUILayout.PropertyField(floaters);
               
           }
           
           serializedObject.ApplyModifiedProperties();
       }
}
