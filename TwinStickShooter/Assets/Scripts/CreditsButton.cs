using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditsButton : MonoBehaviour
{
    public GameObject Credits;
    public GameObject Home;

    
    public void OnClickEnter()
    {
        
        Credits.SetActive(true);
        Home.SetActive(false);
    }
}
