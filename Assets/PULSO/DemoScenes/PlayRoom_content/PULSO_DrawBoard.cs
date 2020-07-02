using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class PULSO_DrawBoard : MonoBehaviour
{
    public Texture2D tex;

    public Renderer renderSurface;

    public int resH;
    public int resW;


    public Transform marker;

    public float drawDistance = 0.01f;
    public float lastDistance = 1f;

    // Start is called before the first frame update
    void Start()
    {
        tex = new Texture2D(resW,resH);

        renderSurface.material.mainTexture = tex;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(marker.position, marker.forward, out hit))
        {

            if (hit.distance < drawDistance)
            {
                Vector2 pixelUV = hit.textureCoord;
                pixelUV.x *= tex.width;
                pixelUV.y *= tex.height;

                //tex.SetPixel((int)pixelUV.x, (int)pixelUV.y, Color.black);

                for (int yy = (int) pixelUV.y - 3; yy < (int) pixelUV.y + 3; yy++)
                {
                    for (int xx = (int) pixelUV.x - 3; xx < (int) pixelUV.x + 3; xx++)
                    {
                        tex.SetPixel(xx, yy, Color.black);
                    }
                }
                tex.Apply();
            }

            lastDistance = hit.distance;
        }

       
        /*
        else
        {

            tex.SetPixel(Random.Range(0, resW), Random.Range(0, resH), Color.green);
            tex.Apply();
        }
        */


    }
}
