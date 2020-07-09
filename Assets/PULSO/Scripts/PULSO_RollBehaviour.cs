using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PULSO_RollBehaviour : MonoBehaviour
{
    public float distanceToActivate = 0.1f;

    public PULSO_Handpad pulso;

    public Material defaultMat;
    public Material selectedMat;
    public Renderer hightLightBody;

    public bool isSelected = false;

    public float angleInStart = 0f;

    public Transform rotBody;

    public bool isLocarRot = false;
    public bool testCloseHand = false;

    public float currentAngle;

    public Vector3 rotOffset;

    void Start()
    {
        if (rotBody == null)
        {
            rotBody = this.transform;
        }

        hightLightBody.material = defaultMat;
    }

    public void Init(float initRot)
    {
        currentAngle = initRot;
        rotBody.localRotation = Quaternion.Euler(0f, currentAngle * 360f, 0f);
    }


    void Update()
    {
        bool prevSelect = isSelected;

        if (Vector3.Distance(transform.position, pulso.pointerRoot.position) < distanceToActivate)
        {
            isSelected = true;
        }
        else
        {
            isSelected = false;
        }

        if (prevSelect && !isSelected)
        {
            OnDeselect();
        }
        else if (!prevSelect && isSelected)
        {
            OnSelect();
        }

        if (isSelected)
        {
            if (IsHandClosed() || testCloseHand)
            {
                currentAngle = pulso.fingers[0].rootNodeAngle_01;
                rotBody.localRotation = Quaternion.Euler(0f, currentAngle * 360f, 0f);
            }
        }
    }

    bool IsHandClosed()
    {
        ///if (pulso.bigFinger.rootNodeAngle_01 > 0.5f)
        //{
        //    return false;
        //}

        for (int i = 1; i < pulso.fingers.Length; i++)
        {
            if (pulso.fingers[i].rootNodeAngle_01 > 0.5f)
            {
                return false;
            }
        }

        return true;
    }

    void OnSelect()
    {
        hightLightBody.material = selectedMat;
    }


    void OnDeselect()
    {
        hightLightBody.material = defaultMat;
    }


    void OnDrawGizmos()
    {
        //if (Application.isPlaying)
        //{
            Gizmos.DrawWireSphere(transform.position, distanceToActivate);
        //}
    }
}
