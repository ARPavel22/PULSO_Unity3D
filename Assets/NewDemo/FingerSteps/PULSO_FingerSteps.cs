using extOSC.Components.Informers;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;
using UnityEngine.UI;

public class PULSO_FingerSteps : MonoBehaviour
{
    public PULSO_HandpadNew pulsoHnd;

    public AudioSource snd;

    public AudioClip[] steps;
    int currentStep = 0;

    public float dist;
    public float distPrev;


    public bool wolking = false;
    public float nextStepTimer = 0f;
    public float lstFingerCrossDist = 0f;

    public bool redyToMove = false;

    public Slider speedslider;

    public Text sttusLbel;

    void Start()
    {
        
    }


    void Update()
    {
        redyToMove = pulsoHnd.figers[0].rootNodeAngle_01 < 0.5f && pulsoHnd.figers[3].rootNodeAngle_01 < 0.5f && pulsoHnd.figers[4].rootNodeAngle_01 < 0.5f;

        if (redyToMove)
        {

            if (nextStepTimer > 0f)
            {
                nextStepTimer -= Time.deltaTime;
            }

            if (nextStepTimer < 0f)
            {
                nextStepTimer = 0f;
                wolking = false;
            }


            dist = pulsoHnd.figers[1].rootNodeAngle_01 - pulsoHnd.figers[2].rootNodeAngle_01;

            if (distPrev > 0f && dist < 0f)
            {
                if (nextStepTimer > 0f)
                {
                    lstFingerCrossDist = Mathf.Lerp(lstFingerCrossDist, nextStepTimer - 0.1f, 0.5f);
                    wolking = true;
                    PlyStep();
                }

                nextStepTimer = 0.5f;
            }

            if (distPrev < 0f && dist > 0f)
            {
                if (nextStepTimer > 0f)
                {
                    lstFingerCrossDist = Mathf.Lerp(lstFingerCrossDist, nextStepTimer - 0.1f, 0.5f);
                    wolking = true;
                    PlyStep();
                }

                nextStepTimer = 0.5f;
            }

            distPrev = dist;
        }
        else
        {
            lstFingerCrossDist = 0f;
            nextStepTimer = 0f;
            wolking = false;
        }


        speedslider.value = lstFingerCrossDist * 2f;
        sttusLbel.text = wolking ? "move" : "stay";
    }

    void PlyStep()
    {
        snd.clip = steps[currentStep];
        snd.Play();

        currentStep++;

        if (currentStep >= steps.Length)
        {
            currentStep = 0;
        }
    }
}
