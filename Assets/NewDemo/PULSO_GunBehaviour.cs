using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PULSO_GunBehaviour : PULSO_Event
{

	public static PULSO_GunBehaviour _instance;

    
	public delegate void OnShot();
	public OnShot InvokeShot;

	public delegate void OnFireStay();
	public static OnFireStay FireStay;

	public delegate void OnFireStop();
	public static OnFireStop StopFire;

	public delegate void OnGunSelect(int _id);
	public static OnGunSelect SelectGun;
    


    public PULSO_HandpadNew pulso;

    public enum ShotState
	{
		None,
		WaitDown,
		WaitUp
	}

	public int _selectedGun = 0;

	public ShotState _shotState = ShotState.None;

	public float _shotStatrAtPos = 0f;


	public void StartShot()
	{
		if (InvokeShot != null)
		{
			InvokeShot ();
		}
	}

	public void ShotStay()
	{
		if (FireStay != null)
		{
			FireStay ();
		}
	}

	public void EndFire()
	{
		if (StopFire != null)
		{
			StopFire ();
		}
	}



	void Update()
	{
		if (_selectedGun == 0 || _selectedGun == 1)
		{
			if (_shotState == ShotState.WaitDown)
			{
				if (_shotStatrAtPos - pulso.figers[1].rootNodeAngle_01 > 0.05f)
				{
					if (InvokeShot != null)
					{
						InvokeShot ();
					}

					_shotStatrAtPos = pulso.figers[1].rootNodeAngle_01;
					_shotState = ShotState.WaitUp;
				}
			}
			else if (_shotState == ShotState.WaitUp)
			{
				if (FireStay != null)
				{
					FireStay ();
				}

				if (pulso.figers[1].rootNodeAngle_01 >= _shotStatrAtPos + 0.05f)
				{
					if (StopFire != null)
					{
						StopFire ();
					}

					_shotState = ShotState.None;
				}
			}
		}
	}
}
