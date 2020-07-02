using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PULSO_HandSubCollider : MonoBehaviour
{
    public enum Type
    {
        palmSide,
        big,
        pointer,
        middle,
        ring,
        pinky
    }

    public Type colliderType;
    public PULSO_HandGrabber grabber;
    public Collider[] lastColliders;
    public float radius;
    public bool isHit = false;

    void Start()
    {
        radius = GetComponent<SphereCollider>().radius;
    }

    public void CastPhysics()
    {
        Collider[] currentColliders = Physics.OverlapSphere(transform.position, radius);

        if (currentColliders.Length > 0)
        {
            isHit = true;

            if (lastColliders.Length > 0)
            {

                for (int i = 0; i < currentColliders.Length; i++)
                {
                    bool found = false;
                    for (int t = 0; t < lastColliders.Length; t++)
                    {
                        if (currentColliders[i] == lastColliders[t])
                        {
                            found = true;
                            break;
                        }
                    }

                    if (found)
                    {
                        grabber.TouchStay(this, currentColliders[i]);
                    }
                    else
                    {
                        grabber.TouchStart(this, currentColliders[i]);
                    }
                }

                for (int i = 0; i < lastColliders.Length; i++)
                {
                    bool isStop = true;
                    for (int t = 0; t < currentColliders.Length; t++)
                    {
                        if (currentColliders[t] == lastColliders[i])
                        {
                            isStop = false;
                            break;
                        }
                    }

                    if (isStop)
                    {
                        grabber.TouchEnd(this, lastColliders[i]);
                    }
                }
            }
            else
            {
                for (int i = 0; i < currentColliders.Length; i++)
                {
                    grabber.TouchStart(this, currentColliders[i]);
                }
            }

        }
        else
        {
            for (int i = 0; i < lastColliders.Length; i++)
            {
                bool isStop = true;
                for (int t = 0; t < currentColliders.Length; t++)
                {
                    if (currentColliders[i] == lastColliders[t])
                    {
                        isStop = false;
                        break;
                    }
                }

                if (isStop)
                {
                    grabber.TouchEnd(this, lastColliders[i]);
                }
            }

            isHit = false;
        }

        lastColliders = currentColliders;
    }



    /*
    private void OnTriggerEnter(Collider other)
    {
        if (grabber.debugTouches)
        {
            Debug.Log(colliderType.ToString() + " touch object: " + other.attachedRigidbody.name);
        }

        //lastCollider = other;
        grabber.TouchStart(this, other);
    }

    private void OnTriggerStay(Collider other)
    {
        if (grabber.debugTouches)
        {
            if (lastCollider != other)
            {
                Debug.Log(colliderType.ToString() + " stay object: " + other.attachedRigidbody.name);
                lastCollider = other;
            }
        }

        grabber.TouchStay(this, other);
    }

    private void OnTriggerExit(Collider other)
    {
        if (grabber.debugTouches)
        {
            Debug.Log(colliderType.ToString() + " UNtouch object: " + other.attachedRigidbody.name);
        }

        lastCollider = null;
        grabber.TouchEnd(this, other);
    }
    */
}
