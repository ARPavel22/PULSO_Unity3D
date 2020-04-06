using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PULSO_RoomSwitchButton :  PULSO_InteractiveObject
{
    public GameObject openRoom;


    void Start()
    {
        PULSO_DemoRoomsManager.instance.buttons.Add(this);
    }

    public override void OnTouch(PULSO_HandpadNew pulsoHand)
    {
        PULSO_DemoRoomsManager.instance.CloseAll();

        gameObject.GetComponent<Renderer>().material.color = PULSO_DemoRoomsManager.instance.buttonSelected;

        openRoom.SetActive(true);
    }

    public override void StopTouch(PULSO_HandpadNew pulsoHand)
    {
        
    }
}
