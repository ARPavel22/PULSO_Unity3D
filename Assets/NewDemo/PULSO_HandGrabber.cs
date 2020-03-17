using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PULSO_HandGrabber : MonoBehaviour
{

    public Transform handRoot;

    public PULSO_Draggable inHand = null;


    public PULSO_HandpadNew pulsoHand;

    public PULSO_SignRecognizer signsListener;


    public Transform pointerPoint;
    public Transform bigPoint;

    public bool useSpherecast = true;

    private PULSO_HandSubCollider[] handColliders;

    /*
    RaycastHit hit;
    public float radius;

    public Transform finger0;
    public float finger0radius;
    public bool isHit = false;
    public Collider[] hitColliders;
    */

    public bool debugTouches = false;

    public void InitSubCoiders()
    {
        List<PULSO_HandSubCollider> handCollidersTmp = GameObject.FindObjectsOfType<PULSO_HandSubCollider>().ToList();
        handCollidersTmp.RemoveAll(x => x.grabber != this);
       

        if (useSpherecast)
        {
            foreach (var subCollider in handCollidersTmp)
            {
                subCollider.GetComponent<Collider>().enabled = false;
            }
        }

        handColliders = handCollidersTmp.ToArray();
    }


    void Awake()
    {
        InitSubCoiders();
    }

    public float GetPinchPower()
    {
        return Vector3.Distance(pointerPoint.position, bigPoint.position);
    }

    public void TouchStart(PULSO_HandSubCollider handCollider, Collider other)
    {
        if (other.attachedRigidbody == null)
        {
            return;
        }

        PULSO_InteractiveObject touched = other.attachedRigidbody.GetComponent<PULSO_InteractiveObject>();

        /*
        if (touched is PULSO_Draggable draggable)
        {
            if (draggable != null && inHand == null)
            {
                if (signsListener._lastRecoSign.Name == draggable.grabSign)
                {
                    Debug.Log("GET");
                    draggable.Get(handRoot);
                    inHand = draggable;
                }
            }
        }
        else 
        */

        if (touched is PULSO_CallibrationZone callibrationZone)
        {
            callibrationZone.HandIn(pulsoHand);
        }

        if (touched is LampBehaviour lamp)
        {
            lamp.Click(pulsoHand);
        }
    }

    public void TouchStay(PULSO_HandSubCollider handCollider, Collider other)
    {
        if (other.attachedRigidbody == null)
        {
            return;
        }


        PULSO_InteractiveObject touched = other.attachedRigidbody.GetComponent<PULSO_InteractiveObject>();

        if (touched is PULSO_Draggable draggable)
        {
            if (draggable != null && inHand == null)
            {
                if (draggable.grabSign == "Pinch")
                {
                    if (GetPinchPower() < 0.04f)
                    {
                        if (handCollider.colliderType == PULSO_HandSubCollider.Type.pointer)
                        {
                            Debug.Log("GET");
                            draggable.Get(pulsoHand);
                            inHand = draggable;
                        }
                    }
                    else
                    {
                        //Debug.Log(GetPinchPower());
                    }
                }
                else
                {

                    if (signsListener.GetSignPercent(draggable.grabSign) >= 90f)
                    {
                        if (draggable.grabSign == "Pinch_1")
                        {
                            if (handCollider.colliderType == PULSO_HandSubCollider.Type.pointer)
                            {
                                Debug.Log("GET");
                                draggable.Get(pulsoHand);
                                inHand = draggable;
                            }
                        }
                        else
                        {
                            Debug.Log("GET");
                            draggable.Get(pulsoHand);
                            inHand = draggable;
                        }
                    }
                }
            }
        }

        /*
        else if (touched is PULSO_CallibrationZone callibrationZone)
        {
            callibrationZone.HandIn(pulsoHand);
        }
        */
    }

    public void TouchEnd(PULSO_HandSubCollider handCollider, Collider other)
    {
        if (other.attachedRigidbody == null)
        {
            return;
        }


        PULSO_InteractiveObject untouched = other.attachedRigidbody.GetComponent<PULSO_InteractiveObject>();

        if (untouched is PULSO_Draggable draggable)
        {
            /*
            if (draggable != null && inHand == null)
            {
                if (signsListener._lastRecoSign.Name == draggable.grabSign)
                {
                    Debug.Log("GET");
                    draggable.Get(handRoot);
                    inHand = draggable;
                }
            }
            */
        }
        else if (untouched is PULSO_CallibrationZone callibrationZone)
        {
            callibrationZone.HandOut(pulsoHand);
            pulsoHand.StopCallibration();
        }
    }

    void Update()
    {
        if (inHand != null)
        {
            if (inHand.grabSign == "Pinch")
            {
                if (GetPinchPower() > 0.06f)
                {
                    Debug.Log("PUT");
                    inHand.Put();
                    inHand = null;
                }
            }
            else
            {
                if (signsListener.GetSignPercent(inHand.grabSign) < 80f)
                {
                    Debug.Log("PUT");
                    inHand.Put();
                    inHand = null;
                }
            }
        }
        else
        {
            if (useSpherecast)
            {
                for (int i = 0; i < handColliders.Length; i++)
                {
                    handColliders[i].CastPhysics();
                }
            }
        }
  
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying && useSpherecast)
        {
            for (int i = 0; i < handColliders.Length; i++)
            {
                if (handColliders[i].isHit)
                {
                    Gizmos.color = Color.red;
                }
                else
                {
                    Gizmos.color = Color.green;
                }

                Gizmos.DrawWireSphere(handColliders[i].transform.position, handColliders[i].radius);
            }
        }
    }
}
