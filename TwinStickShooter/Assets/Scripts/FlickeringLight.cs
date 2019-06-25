using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickeringLight : MonoBehaviour
{

    private Light light;
    private Material mat;
    
    private bool isOn = true;

    public int rangeRate;

    void Start()
    {
        light = GetComponent<Light>();
        mat = GetComponent<Renderer>().material;
    }

    void Update()
    {
        if(Random.Range(0, rangeRate) == 0)
        {
            isOn = !isOn;
            
            if(isOn)
            {
                light.range = 30;
                mat.EnableKeyword("_EMISSION");
            }
            else
            {
                light.range = 0;
                mat.DisableKeyword("_EMISSION");
            }
        }
    }
}
