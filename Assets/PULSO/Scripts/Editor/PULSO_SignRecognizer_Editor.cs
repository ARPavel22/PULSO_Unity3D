using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(PULSO_SignRecognizer))]

public class PULSO_SignRecognizer_Editor : Editor
{
    public string newName;

    public override void OnInspectorGUI()
    {
        PULSO_SignRecognizer myTarget = (PULSO_SignRecognizer)target;

        DrawDefaultInspector();

        EditorGUILayout.Space();
        EditorGUILayout.Separator();
        EditorGUILayout.LabelField("Signs manager", EditorStyles.boldLabel);
        EditorGUILayout.HelpBox("Add new sign to editor & json file", MessageType.Info);

        newName = EditorGUILayout.TextField("NAME: ", newName);
        ///EditorGUILayout.LabelField("Level", myTarget.newSignName.ToString());
        ///
        if (GUILayout.Button("Add SIGN"))
        {
            myTarget.AddCurrentSignToLibrary(newName);
        }

        if (GUILayout.Button("Save Json"))
        {
            myTarget.SaveJsonSigns();
        }
    }
}
