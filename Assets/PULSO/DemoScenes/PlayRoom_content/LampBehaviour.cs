using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampBehaviour : PULSO_InteractiveObject
{
    public GameObject light;


    void Start()
    {
        light.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnTouch(PULSO_Handpad pulsoHand)
    {
        Debug.Log("click");
        light.SetActive(!light.activeSelf);
        Vibrate(1f, 1f, 0.2f, pulsoHand.handSide);
    }
}
