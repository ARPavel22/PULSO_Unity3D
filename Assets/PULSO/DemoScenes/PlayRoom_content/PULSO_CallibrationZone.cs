﻿using UnityEngine;


public class PULSO_CallibrationZone : PULSO_InteractiveObject
{
    public int fingerID;

    public Material handOutMat;
    public Material handInMat;

    public Renderer render;
    

    void Start()
    {
        render.material = handOutMat;
    }


    public override void OnTouch(PULSO_Handpad pulsoHand)
    {
        pulsoHand.CallibrateFinger(fingerID);
        render.material = handInMat;
    }

    public override void StopTouch(PULSO_Handpad pulsoHand)
    {
        pulsoHand.StopCallibration();
        render.material = handOutMat;
    }
}
