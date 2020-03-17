using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PULSO_Key : MonoBehaviour
{
    public Text label;

    public int xx;
    public int yy;

    public GameObject body;

    //public float lastDistToFinger = 0f;
    //public int lastNearestFinger = 0;

    public Vector3 startPos;
   
    void Start()
    {
        startPos = body.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
