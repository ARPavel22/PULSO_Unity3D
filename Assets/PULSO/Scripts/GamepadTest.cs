using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class GamepadTest : MonoBehaviour
{

    public Text txt;

    public enum InputType
    {
        KeyOrMouseButton,
        MouseMovement,
        JoystickAxis,
    };
    SerializedProperty axisArray;

    string stringAll;

    Object inputManager;

    void Start()
    {
        inputManager = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset")[0];

        SerializedObject obj = new SerializedObject(inputManager);

        axisArray = obj.FindProperty("m_Axes");

        if (axisArray.arraySize == 0)
            Debug.Log("No Axes");
    }


    void Update()
    {
        stringAll = "";

        SerializedObject obj = new SerializedObject(inputManager);

        axisArray = obj.FindProperty("m_Axes");

        if (axisArray.arraySize == 0)
            Debug.Log("No Axes");

        for (int i = 0; i < axisArray.arraySize; ++i)
        {
            var axis = axisArray.GetArrayElementAtIndex(i);

            var name = axis.FindPropertyRelative("m_Name").stringValue;
            var axisVal = axis.FindPropertyRelative("axis").intValue;
            var inputType = (InputType)axis.FindPropertyRelative("type").intValue;

            stringAll += name + " ";
            stringAll += axisVal + " ";
            stringAll += inputType + "\n";
        }


        txt.text = stringAll;

        /*
        for (int i = 1; i <= 28; i++)
        {
            if (Input.GetAxis("Axis" + i) > 0)
            {
                Debug.LogFormat("Move Axis{0}", i);
            }
        }
        */
    }
}
