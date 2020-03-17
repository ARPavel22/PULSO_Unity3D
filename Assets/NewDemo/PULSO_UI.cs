using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PULSO_UI : MonoBehaviour
{
    // Start is called before the first frame update
    public PULSO_HandpadNew pulso;
    //public Slider controlSlider;
    public Text lastSign;
    public PULSO_SignRecognizer signs;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        lastSign.text = signs._lastRecoSign.Name + "\n" + signs._lastRecoSign.RecoPercent.ToString("F1") + "%";
    }

    public void Upd(Slider s)
    {
        
        pulso.multiplyMul[s.transform.GetSiblingIndex()] = s.value;
    }
}
