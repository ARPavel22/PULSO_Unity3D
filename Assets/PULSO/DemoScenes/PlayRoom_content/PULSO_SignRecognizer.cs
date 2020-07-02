using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;




[System.Serializable]
public class SignsFile
{
    public JsonSign[] _waveSignsList;
}


[System.Serializable]
public class UnitySign
{
    public string name;
    public JsonSign _sign;
    public float _dist = 0f;
    public float _lastRecognizetTime = 0f;

    public PULSO_SignRecognizer _recognizer;

    public UnitySign(JsonSign jSign)
    {
        _sign = jSign;
    }

    public UnitySign()
    {

    }

    public string Name
    {
        get { return _sign._name; }
        set { Debug.Log(value); Debug.Log(_sign); _sign._name = value; name = value; }
    }

    public float RecoPercent
    {
        get { return (100f * _dist); }
    }

    public float RecoTime
    {
        get { return Time.time - _lastRecognizetTime; }
    }

    public void Calc()
    {
        float _dist0 = System.Math.Abs(_recognizer._hand.fingers[0].rootNodeAngle_01 - _sign._goal0);
        float _dist1 = System.Math.Abs(_recognizer._hand.fingers[1].rootNodeAngle_01 - _sign._goal1);
        float _dist2 = System.Math.Abs(_recognizer._hand.fingers[2].rootNodeAngle_01 - _sign._goal2);
        float _dist3 = System.Math.Abs(_recognizer._hand.fingers[3].rootNodeAngle_01 - _sign._goal3);
        float _dist4 = System.Math.Abs(_recognizer._hand.fingers[4].rootNodeAngle_01 - _sign._goal4);

        _dist = 1f - (_dist0 + _dist1 + _dist2 + _dist3 + _dist4) * 0.2f;  // = 1/5
    }
}


public class PULSO_SignRecognizer : MonoBehaviour
{

    public List<UnitySign> _signs;

    public UnitySign _lastRecoSign;

    public PULSO_Event[] _listenEvents;

    public PULSO_Event _currentEvent = null;

    public delegate void OnSignStart(UnitySign _s);
    public static OnSignStart SignStart;

    public delegate void OnSignStay(UnitySign _s);
    public static OnSignStay SignStay;

    public delegate void OnSignEnd(UnitySign _s);
    public static OnSignEnd SignEnd;

    public delegate void OnEventStart(PULSO_Event _e);
    public static OnEventStart EventStart;

    public delegate void OnEventEnd(PULSO_Event _e);
    public static OnEventEnd EventEnd;

    public UnitySign[] _sortedSigns;


    public string _filesPath;


    public bool _signsFileLoaded = false;

    public enum SignsLibrary
    {
        Playground,
        ABC,
        NONE
    }

    public SignsLibrary _signsLibrary = SignsLibrary.Playground;


    //public static PULSO_SignRecognizer _instance;

    public PULSO_HandpadNew _hand;

    /*
	public void Awake()
	{
		_instance = this;
	}
    */

    void Start()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            _filesPath = Application.persistentDataPath;
        }
        else
        {
            _filesPath = Application.streamingAssetsPath;
        }

        OpenJsonSigns();

        _sortedSigns = new UnitySign[_signs.Count()];
    }

    public float GetSignPercent(string name)
    {
        return _signs.Find(s => s.name == name).RecoPercent;
    }

    public bool EventTryStart(PULSO_Event _e)
    {
        if (EventStart != null)
        {
            EventStart.Invoke(_e);
        }

        return true;
    }

    public void EventStop(PULSO_Event _e)
    {
        if (EventEnd != null)
        {
            EventEnd.Invoke(_e);
        }
    }

    void StartRecognize(UnitySign _s)
    {
        if (SignStart != null)
        {
            SignStart.Invoke(_s);
        }

        if (_currentEvent == null)
        {
            foreach (PULSO_Event _e in _listenEvents)
            {
                _e.SignStart(_s);
            }
        }
        else
        {
            _currentEvent.SignStart(_s);
        }
    }


    void StayRecognized(UnitySign _s)
    {
        if (SignStay != null)
        {
            SignStay.Invoke(_s);
        }

        if (_currentEvent == null)
        {
            foreach (PULSO_Event _e in _listenEvents)
            {
                _e.SignStay(_s);
            }
        }
        else
        {
            _currentEvent.SignStay(_s);
        }
    }


    void EndRecognize(UnitySign _s)
    {
        if (SignEnd != null)
        {
            SignEnd.Invoke(_s);
        }

        if (_currentEvent == null)
        {
            foreach (PULSO_Event _e in _listenEvents)
            {
                _e.SignStop(_s);
            }
        }
        else
        {
            _currentEvent.SignStop(_s);
        }
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            OpenJsonSigns();
        }

        if (_signsFileLoaded)
        {
            foreach (UnitySign _s in _signs)
            {
                _s.Calc();
            }

            _sortedSigns = _signs.OrderBy(s => 1f - s._dist).ToArray();

            if (_lastRecoSign != _sortedSigns[0])
            {

                StartRecognize(_sortedSigns[0]);

                _lastRecoSign = _sortedSigns[0];
                _lastRecoSign._lastRecognizetTime = Time.time;
            }
            else
            {
                StayRecognized(_sortedSigns[0]);
            }

            if (_currentEvent != null)
            {
                _currentEvent.UPD();
            }
        }
    }


    public void AddCurrentSignToLibrary(string name)
    {
        JsonSign s = new JsonSign();

        UnitySign newSign = new UnitySign(s);
        newSign._recognizer = this;
        
        newSign.Name = name;
        

        newSign._sign._goal0 = _hand.fingers[0].rootNodeAngle_01;
        newSign._sign._goal1 = _hand.fingers[1].rootNodeAngle_01;
        newSign._sign._goal2 = _hand.fingers[2].rootNodeAngle_01;
        newSign._sign._goal3 = _hand.fingers[3].rootNodeAngle_01;
        newSign._sign._goal4 = _hand.fingers[4].rootNodeAngle_01;

        _signs.Add(newSign);
    }


    public void OpenJsonSigns()
    {
        string _fileName = "";

        switch (_signsLibrary)
        {
            case SignsLibrary.ABC:
                _fileName = "WaveSignsABC";
                break;

            case SignsLibrary.Playground:
                _fileName = "WaveSignsPlayground";
                break;

            default:
                break;
        }

        Debug.Log(_filesPath + "/" + _fileName + ".json");

        if (File.Exists(_filesPath + "/" + _fileName + ".json"))
        {

            string _t = File.ReadAllText(_filesPath + "/" + _fileName + ".json");
            SignsFile _file = JsonUtility.FromJson<SignsFile>(_t);

            _signs.Clear();

            foreach (JsonSign s in _file._waveSignsList)
            {
                UnitySign newS = new UnitySign();
                newS._sign = s;
                newS._recognizer = this;
                newS.name = newS._sign._name;
                _signs.Add(newS);
            }

            _signsFileLoaded = true;
        }
    }


    public void SaveJsonSigns()
    {
        string _fileName = "";

        switch (_signsLibrary)
        {
            case SignsLibrary.ABC:
                _fileName = "WaveSignsABC";
                break;

            case SignsLibrary.Playground:
                _fileName = "WaveSignsPlayground";
                break;

            default:
                break;
        }

        SignsFile _file = new SignsFile();
        List<JsonSign> _js = new List<JsonSign>();

        foreach (UnitySign s in _signs)
        {
            _js.Add(s._sign);
        }

        _file._waveSignsList = _js.ToArray();

        string _json = JsonUtility.ToJson(_file);
        Debug.Log(_json);
        File.WriteAllText(_filesPath + "/" + _fileName + ".json", _json);
    }
}
