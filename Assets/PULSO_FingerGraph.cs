using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PULSO_FingerGraph : MonoBehaviour
{

    public Texture2D[] tex;
    public PULSO_HandpadNew pulso;

    [HideInInspector()]
    public Color[] backgroundColor;

    public RawImage[] render = new RawImage[4];

    public int[] middlePics = new int[4];

    public int[] filterLine;

    public int[] lastClickPower = new int[4];

    public int[] clickPower;

    public int[] clickPause;

    public KeyboardKeysManager keyboard;

    public int clickPauseFrames = 4;

    public PULSO_RollBehaviour[] minClickPowerSliders;
    public PULSO_RollBehaviour[] minClicLineSliders;

    public Text[] clickForceText;

    // Start is called before the first frame update
    void Start()
    {
        clickPower = new int[4] {100, 100, 100, 100};
        filterLine = new int[4] {252, 252, 252, 252};
        clickPause = new int[4];
        tex = new Texture2D[4];
        middlePics = new int[4];
        lastClickPower = new int[4];


        for (int i = 0; i < 4; i++)
        {
            tex[i] = new Texture2D(pulso.fingerBufferLenght, 512, TextureFormat.RGB24, false);
            tex[i].filterMode = FilterMode.Point;
            render[i].texture = tex[i];

            backgroundColor = tex[i].GetPixels();
            for (int k = 0; k < backgroundColor.Length - 1; k++)
            {
                backgroundColor[k] = Color.black;
            }
        }


        if (minClickPowerSliders.Length != 0)
        {
            for (int i = 0; i < 4; i++)
            {
                minClickPowerSliders[i].Init(Map(clickPower[i], 100, 1200, 0f, 1f));
                minClicLineSliders[i].Init(Map(filterLine[i], 50, 400, 0f, 1f));
            }
        }
    }


    float Map(int x, int in_min, int in_max, float out_min, float out_max)
    {
        return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
    }


    public int GetDownFinger()
    {
        int id = 0;
        int min = 511;
        for (int i = 0; i < pulso.figers.Length; i++)
        {
            if (pulso.figers[i].fingerInt < min)
            {
                min = pulso.figers[i].fingerInt;
                id = i;
            }
        }

        return id;
    }


    // Update is called once per frame
    void Update()
    {
        if (minClickPowerSliders.Length != 0)
        {
            for (int i = 1; i < pulso.figers.Length; i++)
            {
                clickPower[i - 1] = (int)Mathf.Lerp(100, 1200, minClickPowerSliders[i - 1].currentAngle);
                filterLine[i - 1] = (int)Mathf.Lerp(50, 400, minClicLineSliders[i - 1].currentAngle);
            }

            for (int i = 0; i < minClickPowerSliders.Length; i++)
            {
                clickForceText[i].text = lastClickPower[i] + " / " + clickPower[i];
            }
        }



        for (int fingIndex = 0; fingIndex < 4; fingIndex++)
        {
            if (clickPause[fingIndex] > 0)
            {
                clickPause[fingIndex]--;
                return;
            }

            tex[fingIndex].SetPixels(backgroundColor);


            for (int i = 0; i < pulso.fingerBufferLenght; i++)
            {
                /*
                if (t2 == i && t2 != -1)
                {
                    for (int y = pulso.figers[fingIndex].fingerBuffer[i]; y < tex[fingIndex].height; y++)
                    {
                        tex[fingIndex].SetPixel(i, y, Color.blue);
                    }
                }
                */

                /*
                if (t == p && i == t)
                {
                    for (int y = pulso.figers[finfetID].fingerBuffer[i]; y < tex.height; y++)
                    {
                        tex.SetPixel(i, y, Color.magenta);
                    }
                }
                else
                {
                */

              
                    for (int y = pulso.figers[fingIndex].fingerBuffer[i]; y < tex[fingIndex].height; y++)
                    {
                        tex[fingIndex].SetPixel(i, y, Color.red);
                    }
                
                /*
                else if (i == p)
                {
                    for (int y = pulso.figers[finfetID].fingerBuffer[i]; y < tex.height; y++)
                    {
                        tex.SetPixel(i, y, Color.green);
                    }
                }
                */
                //}

                tex[fingIndex].SetPixel(i, pulso.figers[fingIndex].fingerBuffer[i], Color.white);
                tex[fingIndex].SetPixel(i, pulso.figers[fingIndex].fingerBuffer[i] + 1, Color.white);

                if (i < pulso.fingerBufferLenght - 1)
                {
                    if (pulso.figers[fingIndex].fingerBuffer[i] - pulso.figers[fingIndex].fingerBuffer[i + 1] > 40)
                    {
                        tex[fingIndex].SetPixel(i, pulso.figers[fingIndex].fingerBuffer[i], Color.red);
                    }

                }

                if (i > 0)
                {
                    if (pulso.figers[fingIndex].fingerBuffer[i] - pulso.figers[fingIndex].fingerBuffer[i - 1] > 40)
                    {
                        tex[fingIndex].SetPixel(i, pulso.figers[fingIndex].fingerBuffer[i], Color.cyan);
                    }

                }


                tex[fingIndex].SetPixel(i, middlePics[fingIndex], Color.blue);
                tex[fingIndex].SetPixel(i, middlePics[fingIndex] - 1, Color.blue);

                tex[fingIndex].SetPixel(i, filterLine[fingIndex], Color.cyan);
                tex[fingIndex].SetPixel(i, filterLine[fingIndex] - 1, Color.cyan);
            }

            tex[fingIndex].Apply();
        }
    }

    public int FindPeakElement(int[] num)
    {
        List<int> deleteCopy = new List<int>();
        for (int i = 0; i < num.Length - 1; i++)
        {
            if (num[i] != num[i + 1])
            {
                deleteCopy.Add(num[i]);
            }
        }

        num = deleteCopy.ToArray();

        int low = 0, high = num.Length - 1;

        while (low <= high)
        {
            var mid = (high - low) / 2 + low;
            var before = (mid == 0 ? int.MinValue : num[mid - 1]);
            var after = (mid == num.Length - 1 ? int.MinValue : num[mid + 1]);
            if (num[mid] < before && num[mid] < after)
            {
                return mid;
            }
            else if (num[mid] > before && num[mid] > after)
            {
                high = mid - 1;
            }
            else if (num[mid] > before && num[mid] < after)
            {
                high = mid - 1;
            }
            else
            {
                low = mid + 1;
            }
        }

        return -1;
    }

    public int FindPeakElement2(int[] num)
    {
        int low = 0, high = num.Length - 1;

        while (low <= high)
        {
            var mid = (high - low) / 2 + low;
            var before = (mid == 0 ? int.MinValue : num[mid - 1]);
            var after = (mid == num.Length - 1 ? int.MinValue : num[mid + 1]);
            if (num[mid] > before && num[mid] > after)
            {
                return mid;
            }
            else if (num[mid] < before && num[mid] < after)
            {
                high = mid - 1;
            }
            else if (num[mid] < before && num[mid] > after)
            {
                high = mid - 1;
            }
            else
            {
                low = mid + 1;
            }
        }

        return -1;
    }

    int GetPeak()
    {
        for (int i = 1; i < pulso.fingerBufferLenght - 1; i++)
        {
            if (pulso.figers[0].fingerBuffer[i] < pulso.figers[0].fingerBuffer[i - 1] &&
                pulso.figers[0].fingerBuffer[i] < pulso.figers[0].fingerBuffer[i + 1])
            {
                return i;
            }
        }

        return -1;
    }
}
