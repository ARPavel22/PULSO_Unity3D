using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using extOSC;
using System;

public class OSC_PC : MonoBehaviour
{

    public OSCReceiver reciever;
    public Transform controllerRight;



    // Start is called before the first frame update
    void Start()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            gameObject.SetActive(false);
            return;
        }

        reciever.Bind("/pulso/glove",  OnRecieve);
    }

    void OnRecieve(OSCMessage arg0)
    {
        //Debug.Log(arg0.ToString());

        controllerRight.transform.position = new Vector3(arg0.Values[0].FloatValue, arg0.Values[1].FloatValue, arg0.Values[2].FloatValue);
        controllerRight.transform.rotation = new Quaternion(arg0.Values[3].FloatValue, arg0.Values[4].FloatValue, arg0.Values[5].FloatValue, arg0.Values[6].FloatValue);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
