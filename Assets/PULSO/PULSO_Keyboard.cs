using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PULSO_Keyboard : MonoBehaviour
{

	public Transform point;

	public GameObject zone1;
	public GameObject zone2;
	public GameObject zone3;

	public GameObject[] all1;
	public GameObject[] all2;
	public GameObject[] all3;

	public Material selected;
	public Material normal;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        RaycastHit hit;

        if (Physics.Raycast(point.position, point.forward, out hit))
        {
            //print("Found an object - distance: " + hit.distance);
            if (hit.transform.gameObject == zone1)
            {
				print("zone 1");
				for (int i = 0; i < 5; i++)
				{
					all1[i].GetComponent<Renderer>().material = selected;
					all2[i].GetComponent<Renderer>().material = normal;
					all3[i].GetComponent<Renderer>().material = normal;
				}
            }

            if (hit.transform.gameObject == zone2)
            {
				print("zone 2");
				for (int i = 0; i < 5; i++)
				{
					all1[i].GetComponent<Renderer>().material = normal;
					all2[i].GetComponent<Renderer>().material = selected;
					all3[i].GetComponent<Renderer>().material = normal;
				}
            }

            if (hit.transform.gameObject == zone3)
            {
				print("zone 3");
				for (int i = 0; i < 5; i++)
				{
					all1[i].GetComponent<Renderer>().material = normal;
					all2[i].GetComponent<Renderer>().material = normal;
					all3[i].GetComponent<Renderer>().material = selected;
				}
            }
        }

    }
}
