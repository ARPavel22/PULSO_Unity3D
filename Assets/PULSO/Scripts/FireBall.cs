using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
	public GameObject ballPref;
	public PULSO_HandpadNew _pulso;

	float timeToCast = 1f;
	float castTimer = 0f;

	public enum States
	{
		Opened,
		Closed,
		FireReady,
		FireShoted
	}

	public States currentState = States.Opened;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    	if (currentState == States.Opened)
    	{
    		if (IsClosed())
    		{
    			currentState = States.Closed;
    			castTimer = timeToCast;
    		}
    	}
    	else if(currentState == States.Closed)
    	{
    		if (castTimer > 0f)
    		{
    			castTimer -= Time.deltaTime;
    		}
    		else
    		{
    			currentState = States.FireReady;
    			Cast();
    		}
    	}
    	else if (currentState == States.FireReady)
    	{
    		if (IsOpened())
    		{
    			//Shot();
    			Cancel();
    			currentState = States.Opened;
    		}
    	}
    }

    void Cast()
    {
    	Debug.Log("Cast");
    	ballPref.SetActive(true);
    }

    void Cancel()
    {
    	Debug.Log("Cancel");
    	ballPref.SetActive(false);
    }

    void Shot()
    {
    	Debug.Log("Shot");
    	ballPref.SetActive(false);
    }

    bool IsOpened()
    {
        int openedFingers = 0;

        for (int i = 0; i < 5; i++)
        {
        	if (_pulso.fingers[i].rootNodeAngle_01 > 0.65f)
        	{
        		openedFingers++;
        	}
        }

        if (openedFingers == 5)
        {
        	return true;
        }

        return false;
    }

    bool IsClosed()
    {
        int closedFingers = 0;

        for (int i = 0; i < 5; i++)
        {
        	if (_pulso.fingers[i].rootNodeAngle_01 < 0.4f)
        	{
        		closedFingers++;
        	}
        }

        if (closedFingers == 5)
        {
        	return true;
        }

        return false;
    }
}
