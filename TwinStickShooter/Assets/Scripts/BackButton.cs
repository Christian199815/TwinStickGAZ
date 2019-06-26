using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackButton : MonoBehaviour
{
    public GameObject Home;
    public GameObject Credits;
    public GameObject LevelSelect;

    public void OnClickEnter()
    {
        Home.SetActive(true);
        Credits.SetActive(false);
        
    }
}
