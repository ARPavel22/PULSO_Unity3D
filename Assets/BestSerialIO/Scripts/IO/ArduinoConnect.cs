using UnityEngine;
using System;
using System.Collections;
using System.IO.Ports;

public class ArduinoConnect : MonoBehaviour
{
	private SerialPort stream;

    public GameObject messageListener;
    public ScanPorts scanPorts;
    public float readPause = 0.002f;

    public Queue input = Queue.Synchronized(new Queue());

    private void Start()
    {
        scanPorts.StartCoroutine(scanPorts.OpenPortByName("/dev/tty.PULSO_L-ESP32SPP"));
    }

    private void Update()
    {
        ReadQueueMessage();
    }

    public void ReadQueueMessage()
    {
        if (input.Count == 0)
        {
            return;
        }

        DebugCallback(input.Dequeue().ToString());
    }

    public void Open (string portName)
	{
        Debug.Log("Open port...");
		stream = new SerialPort (portName, PortData.Current.baudrate, Parity.None, 8, StopBits.One);
		stream.ReadTimeout = 50;
		stream.Open ();
	}

	public void WriteToArduino (string message)
	{
		// Send the request
		try {
			stream.WriteLine (message);
			stream.BaseStream.Flush ();
		} catch (TimeoutException) {
			
		}
	}


    public void StartRead()
    {
        //StartCoroutine(ReadPulsoCoroutine());
        StartCoroutine(ReadPulsoAsynchronous());
    }

    /*
    IEnumerator ReadPulsoCoroutine()
    {
        while(true)
        {
            DebugCallback(ReadFromArduino(1));
            
            yield return new WaitForSecondsRealtime(0.1f);
        }
    }
    */

    IEnumerator ReadPulsoAsynchronous()
    {
        while (true)
        {
            StartCoroutine
            (
                AsynchronousReadFromArduino
                ((string s) => DebugCallback(s),     // Callback
                    0.1f                         // Timeout (seconds)
                )
            );

            stream.DiscardInBuffer();
            //yield return new WaitForEndOfFrame();
            yield return new WaitForSecondsRealtime(readPause);
        }

        yield return null;
    }
    


    void DebugCallback(string str)
    {
        //Debug.Log(str
        messageListener.SendMessage("OnMessageArrived", str);
    }


    public string ReadFromArduino (int timeout = 0)
	{
		stream.ReadTimeout = timeout;
		try {
			return stream.ReadLine ();
		} catch (TimeoutException) {
			return null;
		}
	}

	public IEnumerator AsynchronousReadFromArduino (Action<string> callback, float timeout = float.PositiveInfinity)
	{
        //Debug.Log("AsynchronousReadFromArduino");
        /*
		DateTime initialTime = DateTime.Now;
		DateTime nowTime;
		TimeSpan diff = default(TimeSpan);
        */

		string dataString = null;

		//do {
			// A single read attempt
			try {
				dataString = stream.ReadLine ();
			} catch (TimeoutException) {
				dataString = null;
			}

            if (dataString != null)
            {
                if (input.Count < 1)
                {
                    input.Enqueue((object)dataString);
                }
                else
                {
                    Debug.LogWarning("Queue is full. Dropping message: " + dataString);
                    //stream.DiscardInBuffer();
                }


                yield return null;
            }
            /*
            else
            {
                Debug.Log("dataString is NULL");
                yield return new WaitForSeconds(0.05f);
            }
            */

            /*
			nowTime = DateTime.Now;
			diff = nowTime - initialTime;

		} while (diff.Milliseconds < timeout);
        */
		
		yield return null;
	}

	void OnApplicationQuit ()
	{
		stream.Close ();
        StopAllCoroutines();
	}
}