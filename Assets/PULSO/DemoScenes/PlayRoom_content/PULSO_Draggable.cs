using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

//[Serializable]
public class ItemInteractiveProfie
{
    public JsonSign sign;
    public Vector3 localPos;
    public Quaternion localRot;
}

public class PULSO_Draggable : PULSO_InteractiveObject
{
    public bool canDrag = true;
    private Vector3 startLocalPos;
    private Quaternion startRot;
    public Transform parent;

    public string grabSign;
    public string profileName;


    public PULSO_HandpadNew lastHand;
    //[SerializeField]
    public ItemInteractiveProfie profile;
    public bool profileLoaded = false;

    public string _filesPath;

    // Start is called before the first frame update
    void Start()
    {
        profile = null;

        startLocalPos = transform.localPosition;
        startRot = transform.localRotation;
        parent = transform.parent;

        if (Application.platform == RuntimePlatform.Android)
        {
            _filesPath = Application.persistentDataPath;
        }
        else
        {
            _filesPath = Application.streamingAssetsPath;
        }

        OpenJsonProfile();
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (OVRInput.GetUp(OVRInput.RawButton.B))
        {
            OpenJsonProfile();
        }
        */
    }

    public void Get(PULSO_HandpadNew pulso)
    {
        Vibrate(1f, 1f, 0.1f, pulso.handSide);
        transform.SetParent(pulso._handRoot);

        if (profile != null && profileLoaded)
        {
            transform.localPosition = profile.localPos;
            transform.localRotation = profile.localRot;
        }
        else
        {
            ///transform.localPosition = Vector3.zero;
        }

        lastHand = pulso;
    }

    public void Touch()
    {

    }

    public void Put()
    {
        transform.SetParent(parent);
        transform.localPosition = startLocalPos;
        transform.localRotation = startRot;

        Vibrate(0.5f, 0.5f, 0.1f, lastHand.handSide);
    }

    public void SetGrabPose(string name)
    {
        transform.SetParent(lastHand._handRoot);

        JsonSign grabSignFile = new JsonSign();
        grabSignFile._name = name;
        lastHand.SetJsonGoalsFromCurrentHand(grabSignFile);
        profile = new ItemInteractiveProfie();
        profileName = name;
        profile.localPos = transform.localPosition;
        profile.localRot = transform.localRotation;

        profile.sign = grabSignFile;

        transform.SetParent(parent);
        transform.localPosition = startLocalPos;
        transform.localRotation = startRot;

        string _json = JsonUtility.ToJson(profile);
        Debug.Log(_json);
        File.WriteAllText(_filesPath + "/" + gameObject.name + ".json", _json);
    }

    public void OpenJsonProfile()
    {
        string _fileName = profileName;
        Debug.Log(File.Exists(_filesPath + "/" + _fileName + ".json"));
       

        if (File.Exists(_filesPath + "/" + _fileName + ".json"))
        {
            string _t = File.ReadAllText(_filesPath + "/" + _fileName + ".json");
            profile = JsonUtility.FromJson<ItemInteractiveProfie>(_t);

            Debug.Log(_filesPath + "/" + _fileName + ".json \n"  + _t);


            profileLoaded = true;
        }
        else
        {
            Debug.Log("DONT FIND: "+ _filesPath + "/" + _fileName + ".json \n");
        }
    }
}
