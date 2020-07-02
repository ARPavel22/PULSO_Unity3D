using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PULSO_Draggable))]
public class PULSO_Draggable_Editor : Editor
{
    public string newName;

    public override void OnInspectorGUI()
    {
        PULSO_Draggable myTarget = (PULSO_Draggable)target;

        DrawDefaultInspector();
        newName = EditorGUILayout.TextField("NAME: ", newName);
       
        if (GUILayout.Button("Set GRAB POSE"))
        {
            myTarget.SetGrabPose(newName);
        }

        if (GUILayout.Button("Save Json"))
        {
          //  myTarget.SaveJsonSigns();
        }
    }
}
