using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PULSO_InteractiveObject : MonoBehaviour
{
    public void Vibrate(float freq, float amp, float time, OVRInput.Controller handSide)
    {
        StartCoroutine(Vibrate_process(freq, amp, time, handSide));
    }

    IEnumerator Vibrate_process(float freq, float amp, float time, OVRInput.Controller handSide)
    {
        OVRInput.SetControllerVibration(freq, amp, handSide);
        yield return new WaitForSecondsRealtime(time);
        OVRInput.SetControllerVibration(0f, 0f, handSide);
    }

    public virtual void OnTouch(PULSO_HandpadNew pulsoHand)
    {

    }

    public virtual void StopTouch(PULSO_HandpadNew pulsoHand)
    {

    }
}
