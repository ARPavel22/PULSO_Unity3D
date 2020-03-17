using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.Events;


public class PULSO_HandpadNew : MonoBehaviour
{
	[Serializable]
	public class Finger
	{
		public enum Names
		{
			Big,
			Pointer,
			Middle,
			Ring,
			Pinky
		}

		public Names name;

		public bool invertMagnet;

		public Transform rootNode;
		public float rootNode_fromAngle;
		public float rootNode_toAngle;
		[Range(0f, 1f)]
		public float rootNodeAngle_01 = 1.0f;

		public Transform middleNode;
		public float middleNode_fromAngle;
		public float middleNode_toAngle;
		[Range(0f, 1f)]
		public float middleNodeAngle_01 = 1.0f;

		public Transform pointNode;
		public float pointNode_fromAngle;
		public float pointNode_toAngle;
		[Range(0f, 1f)]
		public float pointNodeAngle_01 = 1.0f;

		public Vector3 axis = Vector3.forward;
		float prev_rootNodeAngle_01;
		float prev_middleNodeAngle_01;

	    public PULSO_HandpadNew pulsoGlove;

	    public int fingerInt = 0;

        public int callibratedSteps = 511;

	    public int[] fingerBuffer;

	    public UnityEvent OnStartMotion;

	    public void Init()
	    {
	        fingerBuffer = new int[pulsoGlove.fingerBufferLenght];
        }

	    public void ResetBuffer()
	    {
	        SetBufferLineTo(0);

	    }

	    public void SetBufferLineTo(int setTo)
	    {
	        for (int i = 0; i < fingerBuffer.Length; i++)
	        {
	            fingerBuffer[i] = setTo;
	        }
        }

	    public void UpdateBuffer()
	    {
	        for (int i = fingerBuffer.Length - 1; i >= 1; i--)
	        {
                ////////////////////////////////////////////////////////////???????????????????  если сдвигать постоянно то пропускается пик
	             fingerBuffer[i] = fingerBuffer[i - 1];

                /*
	            if (fingerBuffer[0] != fingerInt)
	            {
	             fingerBuffer[i] = fingerBuffer[i - 1];
	            //OnStartMotion.Invoke();
	            }
                */
            }

            fingerBuffer[0] = fingerInt;

	        if (Math.Abs(fingerBuffer[0] - fingerBuffer[1]) < callibratedSteps)
	        {
	            callibratedSteps = Math.Abs(fingerBuffer[0] - fingerBuffer[1]);
	        }
        }

        public virtual void UpdateAngles()
		{
            if (pulsoGlove.gloveType == PulsoType.PulsoPRO)
            {
                rootNode.localRotation = Quaternion.Euler(0f, 0f, Mathf.Lerp(rootNode_fromAngle, rootNode_toAngle, Mathf.Lerp(rootNodeAngle_01, prev_rootNodeAngle_01, pulsoGlove.fourSmooth)));
                middleNode.localRotation = Quaternion.Euler(0f, 0f, Mathf.Lerp(middleNode_fromAngle, middleNode_toAngle, Mathf.Lerp(middleNodeAngle_01, prev_middleNodeAngle_01, pulsoGlove.fourSmooth)));
                pointNode.localRotation = Quaternion.Euler(0f, 0f, Mathf.Lerp(pointNode_fromAngle, pointNode_toAngle, Mathf.Lerp(middleNodeAngle_01, prev_middleNodeAngle_01, pulsoGlove.fourSmooth)));

                prev_rootNodeAngle_01 = rootNodeAngle_01;
                prev_middleNodeAngle_01 = middleNodeAngle_01;
            }
            else if (pulsoGlove.gloveType == PulsoType.PulsoLite)
            {
                rootNode.localRotation = Quaternion.Euler(0f, 0f, Mathf.Lerp(rootNode_fromAngle, rootNode_toAngle, Mathf.Lerp(rootNodeAngle_01, prev_rootNodeAngle_01, pulsoGlove.fourSmooth)));
                middleNode.localRotation = Quaternion.Euler(0f, 0f, Mathf.Lerp(middleNode_fromAngle, middleNode_toAngle, Mathf.Lerp(rootNodeAngle_01, prev_rootNodeAngle_01, pulsoGlove.fourSmooth)));
                pointNode.localRotation = Quaternion.Euler(0f, 0f, Mathf.Lerp(pointNode_fromAngle, pointNode_toAngle, Mathf.Lerp(rootNodeAngle_01, prev_rootNodeAngle_01, pulsoGlove.fourSmooth)));

                prev_rootNodeAngle_01 = rootNodeAngle_01;
            }
		}
	}

	[Serializable]
	public class BigFinger : Finger
	{
		public bool useAnimator;
		public Animator animator;

		public string animationClipName = "default";

		public override void UpdateAngles ()
		{
			if (useAnimator)
			{
				float pos = 1f - rootNodeAngle_01;

				if (pos < 0f) { pos = 0f; }
				if (pos >= 0.99f) { pos = 0.99f; }

				animator.Play(animationClipName, -1, pos);

                if (pulsoGlove.gloveType == PulsoType.PulsoPRO)
                {
                    pointNode.localRotation = Quaternion.Euler(0f, 0f, Mathf.Lerp(pointNode_fromAngle, pointNode_toAngle, middleNodeAngle_01));
                    middleNode.localRotation = Quaternion.Euler(0f, 0f, Mathf.Lerp(middleNode_fromAngle, middleNode_toAngle, middleNodeAngle_01));
                }
                else if (pulsoGlove.gloveType == PulsoType.PulsoLite)
                {
                    pointNode.localRotation = Quaternion.Euler(0f, 0f, Mathf.Lerp(pointNode_fromAngle, pointNode_toAngle, rootNodeAngle_01));
                    middleNode.localRotation = Quaternion.Euler(0f, 0f, Mathf.Lerp(middleNode_fromAngle, middleNode_toAngle, rootNodeAngle_01));
                }
            }
			else
			{
				base.UpdateAngles ();
			}
		}
	}

    public enum PulsoType
    {
        PulsoLite,
        PulsoPRO
    }

    public PulsoType gloveType = PulsoType.PulsoLite;

    public Transform _handRoot;

	public int _mainButton = 0;
	public int _batteryPower = 0;

	[Range(0f, 1f)]
	public float bigSmooth = 1f;
	[Range(0f, 1f)]
	public float fourSmooth = 1f;

	[Header("Big finger:")]
	[SerializeField]
	public BigFinger bigFinger;
	[Header("Other fingers:")]
	[SerializeField]
	public Finger[] figers;

    public bool updateForce = false;

    public enum HandSide
    {
        LEFT,
        RIGHT
    }

    public HandSide handSide = HandSide.RIGHT;


    public int fingerBufferLenght = 5;


    private float lastRecieve;
    public float receiveRate;

    public PULSO_Bluetooth PulsoBT;
    public SerialController PulsoSerial;

    public Transform pointerRoot;

    public void Awake()
	{
	    bigFinger.pulsoGlove = this;
        bigFinger.Init();

	    foreach (var f in figers)
	    {
	        f.pulsoGlove = this;
            f.Init();
	    }

	    if (pointerRoot == null)
	    {
            Debug.LogError("Pointer root is NULL " + gameObject.name + " set to Hand root");
	        pointerRoot = transform;
	    }
	}

    void Start()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            PulsoBT.enabled = true;
        }
        else
        {
            PulsoSerial.enabled = true;   
        }
    }

    void Update()
    {
        if (updateForce)
        {
            bigFinger.UpdateAngles();
            for (int i = 0; i < figers.Length; i++)
            {
                figers[i].UpdateAngles();
            }
        }
    }


    public void ResetBuffers()
    {
        bigFinger.ResetBuffer();
        for (int i = 0; i < figers.Length; i++)
        {
            figers[i].ResetBuffer();
        }
    }

	public void OnMessageArrived(string msg)
	{
		if (!string.IsNullOrEmpty (msg))
		{
			//Debug.Log(msg);
			Parse (msg);
		}
	}

	public void Parse(string package)
	{
	    receiveRate = Mathf.Lerp(receiveRate, Time.time - lastRecieve, 0.1f);
	    lastRecieve = Time.time;

        string[] _int = package.Split (' ');

		try
		{
			_mainButton = int.Parse (_int[5]);
			_batteryPower = int.Parse (_int[6]);

			int raw_0_254 = 0;
			float angle_0_1 = 0f;

			for(int k = 0; k < 5; k++)
			{
				raw_0_254 = int.Parse(_int[k]);
				angle_0_1 = Map (raw_0_254, 0, 511, 0f, 1f);

				switch(k)
				{
					/*
					case 0: bigFinger.rootNodeAngle_01 = angle_0_1; break;
					case 1: bigFinger.middleNodeAngle_01 = angle_0_1; break;
					case 2: figers[0].rootNodeAngle_01 = angle_0_1; break;
					case 3: figers[0].middleNodeAngle_01 = angle_0_1; break;
					case 4: figers[1].rootNodeAngle_01 = angle_0_1; break;
					case 5: figers[1].middleNodeAngle_01 = angle_0_1; break;
					case 6: figers[2].rootNodeAngle_01 = angle_0_1; break;
					case 7: figers[2].middleNodeAngle_01 = angle_0_1; break;
					case 8: figers[3].rootNodeAngle_01 = angle_0_1; break;
					case 9: figers[3].middleNodeAngle_01 = angle_0_1; break;
					*/

					case 0: bigFinger.fingerInt = 511 - raw_0_254; bigFinger.rootNodeAngle_01 = bigFinger.invertMagnet ? angle_0_1 : 1f - angle_0_1; break;
					case 1: figers[0].fingerInt = 511 - raw_0_254; figers[0].rootNodeAngle_01 = figers[0].invertMagnet ? angle_0_1 : 1f - angle_0_1; break;
					case 2: figers[1].fingerInt = 511 - raw_0_254; figers[1].rootNodeAngle_01 = figers[1].invertMagnet ? angle_0_1 : 1f - angle_0_1; break;
					case 3: figers[2].fingerInt = 511 - raw_0_254; figers[2].rootNodeAngle_01 = figers[2].invertMagnet ? angle_0_1 : 1f - angle_0_1; break;
					case 4: figers[3].fingerInt = 511 - raw_0_254; figers[3].rootNodeAngle_01 = figers[3].invertMagnet ? angle_0_1 : 1f - angle_0_1; break;
						
					default: break;
				}
			}

            bigFinger.UpdateBuffer();
			bigFinger.UpdateAngles();

			for (int i = 0; i < figers.Length; i++)
			{
                figers[i].UpdateBuffer();
				figers[i].UpdateAngles();
            }
		}
		catch(System.Exception e)
		{
			Debug.LogError ("Oops.. Break raw data, can't parse! " + e.Message);
		}
	}

	float Map(int x, int in_min, int in_max, float out_min, float out_max)
	{
		return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
	}
}