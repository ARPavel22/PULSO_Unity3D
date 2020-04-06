using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PULSO_DemoRoomsManager : MonoBehaviour
{
    public static PULSO_DemoRoomsManager instance;
    public List<PULSO_RoomSwitchButton> buttons = new List<PULSO_RoomSwitchButton>();

    public Color buttonNorm;
    public Color buttonSelected;

    public void Awake()
    {
        instance = this;
    }

    void Start()
    {
        CloseAll();
    }
    

    void Update()
    {

    }

    public void CloseAll()
    {
        int n = transform.childCount;
        for (int i = 0; i < n; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }

        foreach (PULSO_RoomSwitchButton b in buttons)
        {
            b.gameObject.GetComponent<Renderer>().material.color = buttonNorm;
        }
    }
}
