using System;
using System.Collections;
using UnityEngine;
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
        public bool invertMagnet2;
        public bool invertMagnet3;

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

	    [Range(0f, 1f)]
	    public float spokeNodeAngle_01 = 1.0f;

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
                ///если сдвигать постоянно то пропускается пик
                ///UPD
                ///пик по другой причине пропускался из-за двух одинаковых пиковых точек друг за другом

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
                    //middleNode.localRotation = Quaternion.Euler(0f, 0f, Mathf.Lerp(middleNode_fromAngle, middleNode_toAngle, middleNodeAngle_01));
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

	
	[Header("Other fingers:")]
	[SerializeField]
	public Finger[] fingers;

    [Header("Big finger:")]
    [SerializeField]
    public BigFinger bigFinger;

    public bool updateForce = false;

    public enum HandSide
    {
        LEFT,
        RIGHT
    }

    public HandSide handSide = HandSide.RIGHT;

    //OculusQuestControllers
    /*
    public OVRInput.Controller OVRSide
    {
        get
        {
            if (handSide == HandSide.LEFT)
            {
                return OVRInput.Controller.LTouch;
            }
            else
            {
                return OVRInput.Controller.RTouch;
            }
        }
    }
    */

    public int fingerBufferLenght = 5;


    private float lastRecieve;
    public float receiveRate;

    public PULSO_Bluetooth PulsoBT;
    
#if UNITY_EDITOR
    public SerialController PulsoSerial;
#endif


    public Transform pointerRoot;

    public bool showIncomingStringsData = false;

public float[] multiplyMul = new float[5] {1f,1f,1f,1f,1f};
    public bool multiplyFunction = false;


    public bool callibrationMode = false;
    public Coroutine callibrationFingerProcess = null;
    public float callibrationKoef = 0.01f;


    public void Awake()
    {
        fingers[4] = fingers[3];
        fingers[3] = fingers[2];
        fingers[2] = fingers[1];
        fingers[1] = fingers[0];
	    fingers[0] = bigFinger;

	    foreach (var f in fingers)
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

            if (PulsoBT != null)
            {
                PulsoBT.enabled = true;
            }
        }
        else
        {
#if UNITY_EDITOR
            if (PulsoSerial != null)
            {
                PulsoSerial.enabled = true;
            }
#endif

        }
        
    }


    public void StopCallibration()
    {
        if (callibrationFingerProcess != null)
        {
            StopCoroutine(callibrationFingerProcess);
        }

        callibrationMode = false;

        for (int i = 0; i < multiplyMul.Length; i++)
        {
            if (multiplyMul[i] < 0f)
            {
                multiplyMul[i] = 0f;
            }
        }
    }



    public void CallibrateFinger(int ID)
    {
        if (callibrationFingerProcess != null)
        {
            StopCallibration();
        }

        callibrationMode = true;
        callibrationFingerProcess = StartCoroutine(CalibrateFinger_Process(ID));
    }

    IEnumerator CalibrateFinger_Process(int ID)
    {
        Debug.Log("Callibrating finger " + ID);
        //
        while (true)
        {
            float dist = 0.4f - fingers[ID].rootNodeAngle_01;
            multiplyMul[ID] -= callibrationKoef * dist;

            if (ID == 1)
            {
                dist = 0.5f - fingers[0].rootNodeAngle_01;
                multiplyMul[0] -= callibrationKoef * dist;
            }

            yield return new WaitForEndOfFrame();
        }

        yield return null;
    }


    void Update()
    {
        if (updateForce)
        {
            for (int i = 0; i < fingers.Length; i++)
            {
                fingers[i].UpdateAngles();
            }
        }
    }


    public void ResetBuffers()
    {
        for (int i = 0; i < fingers.Length; i++)
        {
            fingers[i].ResetBuffer();
        }
    }

	public void OnMessageArrived(string msg)
	{
	    if (showIncomingStringsData)
	    {
	        Debug.Log(msg);
	    }

	    if (!string.IsNullOrEmpty (msg))
		{
			Parse (msg);
		}
	}


    public void SetJsonGoalsFromCurrentHand(JsonSign sign)
    {
        sign._goal0 = fingers[0].rootNodeAngle_01;
        sign._goal1 = fingers[1].rootNodeAngle_01;
        sign._goal2 = fingers[2].rootNodeAngle_01;
        sign._goal3 = fingers[3].rootNodeAngle_01;
        sign._goal4 = fingers[4].rootNodeAngle_01;
    }

	public void Parse(string package)
	{
	    receiveRate = Mathf.Lerp(receiveRate, Time.time - lastRecieve, 0.1f);
	    lastRecieve = Time.time;

        string[] _int = package.Split (' ');

		try
		{
			_mainButton = int.Parse (_int[5]);//5//10//15
			_batteryPower = int.Parse (_int[6]);//6//11//16

			int raw_0_254 = 0;
			float angle_0_1 = 0f;

			for(int k = 0; k < 5; k++)//5, 10, 15
			{
				raw_0_254 = int.Parse(_int[k]);
				angle_0_1 = Map (raw_0_254, 0, 511, 0f, 1f);

			    if (multiplyFunction)
			    {
			        angle_0_1 =  Mathf.Pow(angle_0_1, multiplyMul[k]);
			    }

			    // 0 1 2 3 4 
                if (k < 5)
			    {
			        fingers[k].fingerInt = 511 - raw_0_254;
			        fingers[k].rootNodeAngle_01 = fingers[k].invertMagnet ? angle_0_1 : 1f - angle_0_1;
                }

                // 5 6 7 8 9 
			    if (k > 4 && k < 10)
			    {
			        //fingers[k - 5].fingerInt = 511 - raw_0_254;
			        fingers[k - 5].middleNodeAngle_01 = fingers[k - 5].invertMagnet2 ? angle_0_1 : 1f - angle_0_1;
                }
                // 10 11 12 13 14 
			    if (k > 9 && k < 15)
			    {
                    //fingers[k - 10].fingerInt = 511 - raw_0_254;
			        fingers[k - 10].spokeNodeAngle_01 = fingers[k - 10].invertMagnet3 ? angle_0_1 : 1f - angle_0_1;
			    }

            }

			for (int i = 0; i < fingers.Length; i++)
			{
                fingers[i].UpdateBuffer();
				fingers[i].UpdateAngles();
            }
		}
		catch(System.Exception e)
		{
            if (showIncomingStringsData)
            {
                Debug.LogError("Oops.. Break raw data, can't parse! " + e.Message);
            }

		}
	}

	float Map(int x, int in_min, int in_max, float out_min, float out_max)
	{
		return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
	}
}