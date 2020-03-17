using UnityEngine;
using UnityEngine.UI;


public class KeyboardKeysManager : MonoBehaviour
{
    public float spaceX = 0.1f;
    public float spaceY = 0.1f;

    public float keySX = 0.1f;
    public float keySY = 0.1f;
    public float keySZ = 0.1f;

    public const int rows = 3;
    public const int columns = 5;

    public GameObject keyPrefab;

    public PULSO_Key[,] keys;

    public Transform keysRoot;

    public AudioSource clickSnd;
    public float[] pitches = new float[4] {0.7f, 0.8f, 0.9f, 1.0f};

    bool inited = false;

    public float zonesOffset = 0f;

    public Material defaultKey;
    public Material hightlightedKey;
    public Material pointerdKeyMat;
    

    public string[,] abc;

    public Transform fingerPointer;

    public enum AvalibleControlHand
    {
        RIGHT,
        LEFT,
        BOTH
    }

    public PULSO_Key[] nearKeys = new PULSO_Key[4];

    public Text helperViewText;

    public Transform helperViewTransform;

    public PULSO_HandpadNew pulso;

    public Text testInput;

    public int clickTreshhold = 10;

    public int currentPattern = -1;

    public Transform testHand;

    public AvalibleControlHand avalibleHand = AvalibleControlHand.RIGHT;

    void Awake()
    {
        if (avalibleHand == AvalibleControlHand.LEFT)
        { 
            abc = new string[rows, columns]
            {
                {"Q", "W", "E", "R", "T"},
                {"A", "S", "D", "F", "G"},
                {"Z", "X", "C", "V", "B"}
                //{ "", "", "", "", "", "", "", "", "", ""},
                //{ "", "", "", "", "", "", "", "", "", ""}
            };
        }

        if (avalibleHand == AvalibleControlHand.RIGHT)
        {
            abc = new string[rows, columns]
            {
                {"Y", "U", "I", "O", "P"},
                { "H", "J", "K", "L", "<<"},
                {"N", "M", "*", "^", "_"}
                //{ "", "", "", "", "", "", "", "", "", ""},
                //{ "", "", "", "", "", "", "", "", "", ""}
            };
        }

        /*
        if (avalibleHand == AvalibleControlHand.BOTH)
        {
            abc = new string[rows, columns]
            {
                {"Q", "W", "E", "R", "T", "Y", "U", "I", "O", "P"},
                {"A", "S", "D", "F", "G", "H", "J", "K", "L", "<<"},
                {"Z", "X", "C", "V", "B", "N", "M", "_", "_", "*"},
                //{ "", "", "", "", "", "", "", "", "", ""},
                //{ "", "", "", "", "", "", "", "", "", ""}
            };
        }
        */
    }

    public float buttonPressedOffset;



    void Start()
    { 
        CreateKeys();
    }


    void Update()
    {
        CalculateSelectedKeyzones();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            pulso.transform.position = testHand.transform.position;
            pulso.transform.rotation = testHand.transform.rotation;
        }
    }


    public void ResetBuffers()
    {
        pulso.ResetBuffers();
    }


    /// <summary>
    /// /delete this
    /// </summary>
    /// <returns></returns>
    public int FindPattern()
    {
        if (pulso.figers[0].fingerInt < pulso.figers[1].fingerInt)
        {
            if (pulso.figers[3].fingerInt > pulso.figers[2].fingerInt)//pulso.fingersInt[2] > pulso.fingersInt[1] && 
            {
                //Debug.Log("pATTERN 1");
                return 0;
            }
        }

        if (pulso.figers[1].fingerInt < pulso.figers[0].fingerInt)
        {
            if (pulso.figers[2].fingerInt > pulso.figers[1].fingerInt && pulso.figers[3].fingerInt > pulso.figers[2].fingerInt)
            {
                //Debug.Log("pATTERN 2");
                return 1;
            }
        }

        if (pulso.figers[2].fingerInt < pulso.figers[3].fingerInt)
        {
            if (pulso.figers[1].fingerInt > pulso.figers[2].fingerInt && pulso.figers[0].fingerInt > pulso.figers[1].fingerInt)
            {
                //Debug.Log("pATTERN 3");
                return 2;
            }
        }

        if (pulso.figers[3].fingerInt < pulso.figers[2].fingerInt)
        {
            if (pulso.figers[0].fingerInt > pulso.figers[1].fingerInt) //pulso.fingersInt[1] > pulso.fingersInt[2] && 
            {
                // Debug.Log("pATTERN 4");
                return 3;
            }
        }

        return -1;
    }




    public void ClickOnKey(int nearKeyID)
    {
        if (nearKeys[nearKeyID] == null)
        {
            for (int i = nearKeyID; i > 0; i--)
            {
                //Debug.Log("Try find near " + i);
                if (nearKeys[i] != null)
                {
                    //Debug.Log("not null" + i);
                    nearKeyID = i;
                }
            }

            if (nearKeys[nearKeyID] == null)
            {
                return;
            }
        }

        //clickSnd.pitch = UnityEngine.Random.Range(pitches[0], pitches[1]);
        clickSnd.pitch = pitches[nearKeyID];


        clickSnd.Play();

        if (nearKeys[nearKeyID].label.text == "_")
        {
            testInput.text += " ";
        }
        else if (nearKeys[nearKeyID].label.text == "<<")
        {
            testInput.text = testInput.text.Remove(testInput.text.Length - 1, 1);
        }
        else if (nearKeys[nearKeyID].label.text == "*")
        {
            testInput.text = "";
        }
        else if (nearKeys[nearKeyID].label.text == "^")
        {
            testInput.text += "\n";
        }
        else
        {
            testInput.text += nearKeys[nearKeyID].label.text;
        }
    }


    void CalculateSelectedKeyzones()
    {
        PULSO_Key lastNearPulsoKey = nearKeys[0];
        float distMin = 1000f;

        //переделать на нормальные геометрические коллизии

        //float distCurrent = Vector3.Distance(lastNearPulsoKey.transform.position, fingerPointer.transform.position);

        for (int yy = 0; yy < rows; yy++)
        {
            for (int xx = 0; xx < columns; xx++)
            {
              if (keys[yy, xx] == null) return;

                float dist = Vector3.Distance(keys[yy, xx].transform.position, fingerPointer.transform.position);

                if (distMin > dist)
                {
                    nearKeys[0] = keys[yy, xx];
                    distMin = dist;
                }
            }
        }

        //if (distMin)

        //////////////////////////////////////--if han stay in one selected zone

        if (nearKeys[0] == lastNearPulsoKey)
        {
            for (int i = 0; i < nearKeys.Length; i++)
            {
                if (nearKeys[i] != null)
                {
                    //keys animation
                    nearKeys[i].body.transform.localPosition = new Vector3(nearKeys[i].startPos.x,
                        nearKeys[i].startPos.y - (pulso.figers[i].rootNodeAngle_01) * buttonPressedOffset,
                        nearKeys[i].startPos.z);
                }
            }

            //CalcMotionInZones();
            //FindClickPeakInBuffer();

            return;
        }
        //////////////////////////////////////////////////

        ResetBuffers();

        if (avalibleHand == AvalibleControlHand.RIGHT)
        {
            /////////////////////////////////////////////////--------CHECK END OF GRID XX
            if (nearKeys[0].xx + 1 <= columns - 1)
            {
                nearKeys[1] = keys[nearKeys[0].yy, nearKeys[0].xx + 1];
                //nearKeys[1].transform.localPosition = nearKeys[1].startPos + new Vector3(0f, offsetsSelected[1]);
            }
            else
            {
                nearKeys[1] = null;
            }

            if (nearKeys[0].xx + 2 <= columns - 1)
            {
                nearKeys[2] = keys[nearKeys[0].yy, nearKeys[0].xx + 2];
                //nearKeys[2].transform.localPosition = nearKeys[2].startPos + new Vector3(0f, offsetsSelected[2]);
            }
            else
            {
                nearKeys[2] = null;
            }

            if (nearKeys[0].xx + 3 <= columns - 1)
            {
                nearKeys[3] = keys[nearKeys[0].yy, nearKeys[0].xx + 3];
                //nearKeys[3].transform.localPosition = nearKeys[3].startPos + new Vector3(0f, offsetsSelected[3]);
            }
            else
            {
                nearKeys[3] = null;
            }
        }
        else if (avalibleHand == AvalibleControlHand.LEFT)
        {
            /////////////////////////////////////////////////--------CHECK END OF GRID XX
            if (nearKeys[0].xx - 1 >= 0)
            {
                nearKeys[1] = keys[nearKeys[0].yy, nearKeys[0].xx - 1];
                //nearKeys[1].transform.localPosition = nearKeys[1].startPos + new Vector3(0f, offsetsSelected[1]);
            }
            else
            {
                nearKeys[1] = null;
            }

            if (nearKeys[0].xx - 2 >= 0)
            {
                nearKeys[2] = keys[nearKeys[0].yy, nearKeys[0].xx - 2];
                //nearKeys[2].transform.localPosition = nearKeys[2].startPos + new Vector3(0f, offsetsSelected[2]);
            }
            else
            {
                nearKeys[2] = null;
            }

            if (nearKeys[0].xx - 3 >= 0)
            {
                nearKeys[3] = keys[nearKeys[0].yy, nearKeys[0].xx - 3];
                //nearKeys[3].transform.localPosition = nearKeys[3].startPos + new Vector3(0f, offsetsSelected[3]);
            }
            else
            {
                nearKeys[3] = null;
            }
        }

        //////////// HELPER DIGITS
        helperViewTransform.position = new Vector3(nearKeys[0].transform.position.x, helperViewTransform.position.y, helperViewTransform.position.z);
        string newHelper = "";


        //////////////////////////////////////////-------------REPAINT
        if (avalibleHand == AvalibleControlHand.RIGHT)
        {
            for (int yy = 0; yy < rows; yy++)
            {
                for (int xx = 0; xx < columns; xx++)
                {
                    if (keys[yy, xx].yy == nearKeys[0].yy)
                    {
                        if (keys[yy, xx].xx >= nearKeys[0].xx && keys[yy, xx].xx < nearKeys[0].xx + 4)
                        {
                            //4 keys main
                            newHelper += keys[yy, xx].label.text + "   ";
                            keys[yy, xx].body.GetComponent<Renderer>().material = pointerdKeyMat;
                        }
                        else
                        {
                            //other
                            keys[yy, xx].body.GetComponent<Renderer>().material = defaultKey;
                            keys[yy, xx].body.transform.localPosition = keys[yy, xx].startPos;
                        }
                    }
                    else if (keys[yy, xx].xx >= nearKeys[0].xx && keys[yy, xx].xx < nearKeys[0].xx + 4)
                    {
                        //4 keys up down
                        keys[yy, xx].body.GetComponent<Renderer>().material = hightlightedKey;
                        keys[yy, xx].body.transform.localPosition = keys[yy, xx].startPos;

                    }
                    else
                    {
                        //other
                        keys[yy, xx].body.GetComponent<Renderer>().material = defaultKey;
                        keys[yy, xx].body.transform.localPosition = keys[yy, xx].startPos;
                    }
                }
            }
        }

        if (avalibleHand == AvalibleControlHand.LEFT)
        {
            for (int yy = 0; yy < rows; yy++)
            {
                for (int xx = 0; xx < columns; xx++)
                {
                    if (keys[yy, xx].yy == nearKeys[0].yy)
                    {
                        if (keys[yy, xx].xx <= nearKeys[0].xx && keys[yy, xx].xx > nearKeys[0].xx - 4)
                        {
                            //4 keys main
                            newHelper += keys[yy, xx].label.text + "   ";
                            keys[yy, xx].body.GetComponent<Renderer>().material = pointerdKeyMat;
                        }
                        else
                        {
                            //other
                            keys[yy, xx].body.GetComponent<Renderer>().material = defaultKey;
                            keys[yy, xx].body.transform.localPosition = keys[yy, xx].startPos;
                        }
                    }
                    else if (keys[yy, xx].xx <= nearKeys[0].xx && keys[yy, xx].xx > nearKeys[0].xx - 4)
                    {
                        //4 keys up down
                        keys[yy, xx].body.GetComponent<Renderer>().material = hightlightedKey;
                        keys[yy, xx].body.transform.localPosition = keys[yy, xx].startPos;

                    }
                    else
                    {
                        //other
                        keys[yy, xx].body.GetComponent<Renderer>().material = defaultKey;
                        keys[yy, xx].body.transform.localPosition = keys[yy, xx].startPos;
                    }
                }
            }
        }

        helperViewText.text = newHelper;
    }


    void UpdatePoses()
    {
        if (inited)
        {
            for (int yy = 0; yy < rows; yy++)
            {
                for (int xx = 0; xx < columns; xx++)
                {
                    keys[yy, xx].transform.localPosition = new Vector3(yy * spaceY, 0f, -xx * spaceX);
                    keys[yy, xx].transform.localScale = new Vector3(keySX, keySY, keySZ);
                }
            }
        }
    }

    public void CreateKeys()
    {
        keys = new PULSO_Key[rows, columns];

        for (int yy = 0; yy < rows; yy++)
        {
            for (int xx = 0; xx < columns; xx++)
            {
                keys[yy, xx] = Instantiate(keyPrefab).GetComponent<PULSO_Key>();
                keys[yy, xx].transform.SetParent(keysRoot);
                keys[yy, xx].transform.localPosition = new Vector3(yy * spaceY, 0f, -xx * spaceX);
                keys[yy, xx].transform.localScale = new Vector3(keySX, keySY, keySZ);
                //Debug.Log("XX "+ xx + " " + "YY " + yy + "   " + keys[yy, xx].label);
                keys[yy, xx].label.text = abc[rows - 1 - yy, xx];
                keys[yy, xx].xx = xx;
                keys[yy, xx].yy = yy;
            }
        }

        keyPrefab.SetActive(false);

        inited = true;
    }
}
