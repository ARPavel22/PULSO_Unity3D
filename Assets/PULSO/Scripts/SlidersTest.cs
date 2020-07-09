using PULSO;
using UnityEngine;
using UnityEngine.UI;

public class SlidersTest : MonoBehaviour
{
    public Slider[] sliders;

    void Update()
    {
        if (PULSO_Manager.LeftHand.fingersInitComplete)
        {
            for (int i = 0; i < sliders.Length; i++)
            {
                sliders[i].value = PULSO_Manager.LeftHand.fingers[i].rootNodeAngle_01;
            }
        }
    }
}
