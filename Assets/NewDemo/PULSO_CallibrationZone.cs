using UnityEngine;


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


    public void HandIn(PULSO_HandpadNew pulsoHand)
    {
        pulsoHand.CallibrateFinger(fingerID);
        render.material = handInMat;
    }


    public void HandOut(PULSO_HandpadNew pulsoHand)
    {
        pulsoHand.StopCallibration();
        render.material = handOutMat;
    }
}
