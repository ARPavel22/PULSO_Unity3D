using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Reconnector : MonoBehaviour
{
#if UNITY_STANDALONE_WIN
    public SerialController serial;

    public int currentPort = 0;
    public GameObject input;
    private string prefsKey_port = "COMPort";
    public InputField inputField;


    public void OpenInput()
    {
        inputField.text = currentPort.ToString();
        input.SetActive(true);
    }

    public void ReconnectWithCOM()
    {
        PlayerPrefs.SetInt(prefsKey_port, int.Parse(inputField.text));
        currentPort = PlayerPrefs.GetInt(prefsKey_port);
        StartCoroutine(Reconnection(inputField.text));
    }

    IEnumerator Reconnection(string com)
    {
        serial.enabled = false;
        yield return new WaitForSecondsRealtime(2f);

        serial.portName = "COM" + currentPort.ToString();

        serial.enabled = true;
    }
        
    void Start()
    {
        input.SetActive(false);
        
        if (PlayerPrefs.HasKey(prefsKey_port))
        {
            currentPort = PlayerPrefs.GetInt(prefsKey_port);
            serial.portName = "COM" + currentPort.ToString();
        }

        serial.enabled = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OpenInput();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
#endif
}
