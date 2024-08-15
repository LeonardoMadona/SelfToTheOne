using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientLightLerp : MonoBehaviour
{
    public float lerp;

    [ColorUsage(true, true)]
    public Color color1, color2;    

    // Update is called once per frame
    void Update()
    {
        RenderSettings.ambientLight = Color.Lerp(color1, color2, lerp);
    }
}
