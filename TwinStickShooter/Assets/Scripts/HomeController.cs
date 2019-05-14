﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeController : MonoBehaviour
{
    [SerializeField] private Transform[] Select;
    [SerializeField] private Transform Lift;
    [SerializeField] private float position = 0;
    [SerializeField] private KeyCode up;
    [SerializeField] private KeyCode down;
    [SerializeField] private KeyCode Enter;

    [SerializeField] private GameObject Home;
    [SerializeField] private GameObject Credits;
    [SerializeField] private GameObject LevelSelect;

 


    

    
    void Update()
    {
        LiftSelect();
        FindPosition();
        BeloworAbove();
        ContinueSelection();
    }

    //input
    void LiftSelect()
    {
        if (Input.GetKeyDown(up))
        {
            position--;
        }
        else if (Input.GetKeyDown(down))
        {
            position++;
        }
    }
    //verplaatsen
    void FindPosition()
    {
        if(position == 0)
        {
            Lift.position = Vector3.MoveTowards(Lift.position, Select[0].position, 1.5f * Time.deltaTime);
        }
        if (position == 1)
        {
            Lift.position = Vector3.MoveTowards(Lift.position, Select[1].position, 1.5f * Time.deltaTime);
        }
        if (position == 2)
        {
            Lift.position = Vector3.MoveTowards(Lift.position, Select[2].position, 1.5f * Time.deltaTime);
        }
        if (position == 3)
        {
            Lift.position = Vector3.MoveTowards(Lift.position, Select[3].position, 1.5f * Time.deltaTime);
        }
        if (position == 4)
        {
            Lift.position = Vector3.MoveTowards(Lift.position, Select[4].position, 1.5f * Time.deltaTime);
        }
    }
    //veiligheid
    void BeloworAbove()
    {
        if(position <= 0)
        {
            position = 0;
        }
        if(position >= 4)
        {
            position = 4;
        }
    }
    //selectie
    void ContinueSelection()
    {
        if(Lift.position == Select[1].position && Input.GetKeyDown(Enter))
        {
            //SceneManager.LoadScene();
        }
        if (Lift.position == Select[2].position && Input.GetKeyDown(Enter))
        {
            print("level");
            LevelSelect.SetActive(true);
            Home.SetActive(false);
        }
        if (Lift.position == Select[3].position && Input.GetKeyDown(Enter))
        {
            print("options");
            Credits.SetActive(true);
            Home.SetActive(false);
        }
        if (Lift.position == Select[4].position && Input.GetKeyDown(Enter))
        {
            print("quit");
            Application.Quit();
        }
    }
}