using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    [SerializeField] private Transform[] Select;
    [SerializeField] private Transform Lift;
    [SerializeField] private float position = 0;
    [SerializeField] private KeyCode up;
    [SerializeField] private KeyCode down;
    [SerializeField] private KeyCode Enter;
    [SerializeField] private int Speed;
    [SerializeField] private GameObject Level;

    
    [SerializeField] private GameObject Pause;







    void Update()
    {
        
        LiftSelect();
        FindPosition();
        BeloworAbove();
        ContinueSelection();
    }

    

    //input opvragen
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
    //verplaats naar de positie
    void FindPosition()
    {
        if (position == 0)
        {
            Lift.position = Vector3.MoveTowards(Lift.position, Select[0].position, Speed * Time.deltaTime);
        }
        if (position == 1)
        {
            Lift.position = Vector3.MoveTowards(Lift.position, Select[1].position, Speed * Time.deltaTime);
        }
        if (position == 2)
        {
            Lift.position = Vector3.MoveTowards(Lift.position, Select[2].position, Speed * Time.deltaTime);
        }
        if (position == 3)
        {
            Lift.position = Vector3.MoveTowards(Lift.position, Select[3].position, Speed * Time.deltaTime);
        }
       
    }
    //Zodat je niet boven of onder het getal kan gaan
    void BeloworAbove()
    {
        if (position <= 0)
        {
            position = 0;
        }
        if (position >= 3)
        {
            position = 3;
        }
    }
    //als je op spatie drukt dat hij dan een canvas of scene opent
    void ContinueSelection()
    {
        if (Lift.position == Select[1].position && Input.GetKeyDown(Enter))
        {
            print("Back");
            Pause.SetActive(false);
            Time.timeScale = 1;
        }
        if (Lift.position == Select[2].position && Input.GetKeyDown(Enter))
        {
            print("options");
            
            Pause.SetActive(false);
        }
        if (Lift.position == Select[3].position && Input.GetKeyDown(Enter))
        {
            print("home");
            SceneManager.LoadScene(0);
            
        }
       
    }
}
