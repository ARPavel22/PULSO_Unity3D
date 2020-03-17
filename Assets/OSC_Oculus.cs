using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using extOSC;

public class OSC_Oculus : MonoBehaviour
{
    public OSCTransmitter transmitter;
    public bool send = false;

    public Transform controllerRight;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        if (Application.platform != RuntimePlatform.Android)
        {
            //this.enabled = false;
            gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("Local IP: " + GetLocalIPAddress());
            yield return new WaitForSecondsRealtime(1f);
            transmitter.RemoteHost = "192.168.1.103";
            transmitter.Connect();
            Debug.Log("RemoteHost: " + transmitter.RemoteHost);
            yield return new WaitForSecondsRealtime(1f);
        }
    }

    public string GetLocalIPAddress()
    {
        var host = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            {
                return ip.ToString();
            }
        }

        throw new System.Exception("No network adapters with an IPv4 address in the system!");
    }

    // Update is called once per frame
    void Update()
    {
        if (send)
        {
            var message = new OSCMessage("/pulso/glove");

            // Populate values.
            message.AddValue(OSCValue.Float(controllerRight.transform.position.x));
            message.AddValue(OSCValue.Float(controllerRight.transform.position.y));
            message.AddValue(OSCValue.Float(controllerRight.transform.position.z));

            message.AddValue(OSCValue.Float(controllerRight.transform.rotation.x));
            message.AddValue(OSCValue.Float(controllerRight.transform.rotation.y));
            message.AddValue(OSCValue.Float(controllerRight.transform.rotation.z));
            message.AddValue(OSCValue.Float(controllerRight.transform.rotation.w));

            transmitter.Send(message);

            Debug.Log("send:" + message.ToString());
        }
    }
}
