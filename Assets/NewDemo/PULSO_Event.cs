using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;


public class PULSO_Event : MonoBehaviour{

	public delegate void OnStart();
	public static OnStart EventStart;

	public delegate void OnEnd();
	public static OnEnd EventEnd;

    public PULSO_SignRecognizer _signHand;

    public enum HandSide
	{
		LEFT,
		RIGHT
	}

	public virtual void Awake()
	{
		
	}

	public virtual void Start()
	{
		
	}


	public virtual bool OnEventStart ()
	{
		if (EventStart != null) {
			EventStart.Invoke ();
		}

		return _signHand.EventTryStart(this);
	}

	public virtual void OnEventEnd ()
	{
		if (EventEnd != null) {
			EventEnd.Invoke ();
		}

		_signHand.EventStop(this);
	}

	public virtual void UPD () {
		
	}

	public virtual void SignStart(UnitySign _s)
	{
		
	}

	public virtual void SignStay(UnitySign _s)
	{
		
	}

	public virtual void SignStop(UnitySign _s)
	{

	}
}
