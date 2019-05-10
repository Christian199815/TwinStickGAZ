using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackButton : MonoBehaviour
{
    public GameObject Home;
    public GameObject Credits;

    public void OnClickEnter()
    {
        Home.SetActive(true);
        Credits.SetActive(false);
    }
}
