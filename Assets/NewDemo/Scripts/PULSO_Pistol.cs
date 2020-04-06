using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PULSO_Pistol : MonoBehaviour
{
    public PULSO_GunBehaviour pulsoGunEvents;

    public GameObject sparks;

    public float shotPause = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        pulsoGunEvents.InvokeShot += Fire;
    }

    // Update is called once per frame
    void Update()
    {
        if (shotPause > 0f)
        {
            shotPause -= Time.deltaTime;
        }
        else
        {
            shotPause = 0f;
            pulsoGunEvents._shotState = PULSO_GunBehaviour.ShotState.WaitDown;
        }
    }

    public void Fire()
    {
        shotPause = 0.2f;
        Debug.Log("Shot!");

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.position + transform.forward * 10f, out hit))
        {

            Debug.Log(hit.collider.gameObject.name);
/*
            if (hit.distance < drawDistance)
            {
            }
            */
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position+transform.forward * 10f);
    }
}
