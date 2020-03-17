using extOSC;
using UnityEngine;
using UnityEngine.UI;


public class PULSO_OculusOSC : MonoBehaviour
{
    public string Address = "/pulso/1";
    public OSCTransmitter Transmitter;
    public OSCReceiver Receiver;

    public bool isReciever = true;

    public Transform rigthtController;
    public Transform leftController;

    public Transform rightListener;
    public Transform leftListener;

    bool sendUpd = false;

    public Text iptext;

    public int selectedIP = 0;


    protected virtual void Start()
    {
        isReciever = !(Application.platform == RuntimePlatform.Android);
        Debug.Log("Your IP: " + GetLocalIPAddress());

        if (isReciever)
        {
            Debug.Log(System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable());

            Receiver.enabled = true;
            Receiver.Bind(Address, ReceivedMessage);
        }
        else
        {
            Transmitter.enabled = true;
            iptext.text = "Your IP: " + GetLocalIPAddress();
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


    private void Update()
    {
        if (!isReciever)
        {
            if (OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick).y > 0f)
            {
                selectedIP++;

                if (selectedIP >= 254)
                {
                    selectedIP = 0;
                }

                iptext.text = "Your IP: " + GetLocalIPAddress() + " HostIP:" + selectedIP;
            }

            if (OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick).y < 0f)
            {
                selectedIP--;

                if (selectedIP <= 0)
                {
                    selectedIP = 254;
                }


                iptext.text = "Your IP: " + GetLocalIPAddress() + " HostIP:" + selectedIP;
            }

            if (OVRInput.GetUp(OVRInput.RawButton.A))
            {
                Debug.Log("A");

                string myip = GetLocalIPAddress();
                string[] newIP = myip.Split('.');

                Transmitter.RemoteHost = newIP[0] + "." + newIP[1] + "." + newIP[2] + "." + selectedIP;

                Debug.Log(Transmitter.RemoteHost);
                Transmitter.Connect();
            }

            if (OVRInput.GetUp(OVRInput.RawButton.B))
            {
                Debug.Log("B");

                sendUpd = !sendUpd;
            }

            if (sendUpd)
            {
                Send();
            }
        }
    }


    public void Send()
    {
        if (!isReciever)
        {
            var message = new OSCMessage(Address);
            message.AddValue(OSCValue.Float(rigthtController.transform.position.x));
            message.AddValue(OSCValue.Float(rigthtController.transform.position.y));
            message.AddValue(OSCValue.Float(rigthtController.transform.position.z));

            message.AddValue(OSCValue.Float(rigthtController.transform.rotation.x));
            message.AddValue(OSCValue.Float(rigthtController.transform.rotation.y));
            message.AddValue(OSCValue.Float(rigthtController.transform.rotation.z));
            message.AddValue(OSCValue.Float(rigthtController.transform.rotation.w));

            message.AddValue(OSCValue.Float(leftController.transform.position.x));
            message.AddValue(OSCValue.Float(leftController.transform.position.y));
            message.AddValue(OSCValue.Float(leftController.transform.position.z));

            message.AddValue(OSCValue.Float(leftController.transform.rotation.x));
            message.AddValue(OSCValue.Float(leftController.transform.rotation.y));
            message.AddValue(OSCValue.Float(leftController.transform.rotation.z));
            message.AddValue(OSCValue.Float(leftController.transform.rotation.w));

            Transmitter.Send(message);
        }
    }


    private void ReceivedMessage(OSCMessage message)
    {
        rightListener.transform.position = new Vector3(message.Values[0].FloatValue, message.Values[1].FloatValue, message.Values[2].FloatValue);
        rightListener.transform.rotation = new Quaternion(message.Values[3].FloatValue, message.Values[4].FloatValue, message.Values[5].FloatValue, message.Values[6].FloatValue);

        leftListener.transform.position = new Vector3(message.Values[7].FloatValue, message.Values[8].FloatValue, message.Values[9].FloatValue);
        leftListener.transform.rotation = new Quaternion(message.Values[10].FloatValue, message.Values[11].FloatValue, message.Values[12].FloatValue, message.Values[13].FloatValue);
    }

}
