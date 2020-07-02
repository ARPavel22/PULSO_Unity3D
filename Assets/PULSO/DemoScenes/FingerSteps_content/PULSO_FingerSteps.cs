using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using TMPro.EditorUtilities;
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
    public Image sld;
    public Gradient gradient;

    public float speedMul = 6f;

    void Start()
    {
        
    }


    void Update()
    {
        redyToMove = pulsoHnd.fingers[0].rootNodeAngle_01 < 0.5f && pulsoHnd.fingers[3].rootNodeAngle_01 < 0.5f && pulsoHnd.fingers[4].rootNodeAngle_01 < 0.5f;

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

                lstFingerCrossDist = 0f;
                wolking = false;
            }


            dist = pulsoHnd.fingers[1].rootNodeAngle_01 - pulsoHnd.fingers[2].rootNodeAngle_01;

            if (distPrev > 0f && dist < 0f)
            {
                if (nextStepTimer > 0f)
                {
                    lstFingerCrossDist = Mathf.Lerp(lstFingerCrossDist, nextStepTimer * nextStepTimer, 0.5f);
                    wolking = true;
                    PlyStep();
                }

                nextStepTimer = 0.5f;
            }

            if (distPrev < 0f && dist > 0f)
            {
                if (nextStepTimer > 0f)
                {
                    lstFingerCrossDist = Mathf.Lerp(lstFingerCrossDist, nextStepTimer * nextStepTimer, 0.5f);
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


        sld.color = gradient.Evaluate(Mathf.Lerp(speedslider.value, lstFingerCrossDist * speedMul, 0.05f));
        speedslider.value = Mathf.Lerp(speedslider.value, lstFingerCrossDist * speedMul, 0.1f);
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
