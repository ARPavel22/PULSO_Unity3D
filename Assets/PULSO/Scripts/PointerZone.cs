﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerZone : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.name);
        //KeyboardKeysManager.instance.OnZonesTouch(other);
    }
}