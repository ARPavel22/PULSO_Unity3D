using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;

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
    //public int lstFingerCrossDirection = 0;

    void Start()
    {
        
    }


    void Update()
    {
        if (pulsoHnd.figers[0].rootNodeAngle_01 > 0.5f && pulsoHnd.figers[3].rootNodeAngle_01 > 0.5f && pulsoHnd.figers[4].rootNodeAngle_01 > 0.5f)
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
                    PlyStep();
                }

                nextStepTimer = 1f;

                //lstFingerCrossDirection = -1;
            }

            if (distPrev < 0f && dist > 0f)
            {
                if (nextStepTimer > 0f)
                {
                    PlyStep();
                }

                nextStepTimer = 1f;
                //lstFingerCrossDirection = 1;
            }

            distPrev = dist;
        }

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
