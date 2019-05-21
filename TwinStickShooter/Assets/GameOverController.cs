using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverController : MonoBehaviour
{
    [SerializeField] private Transform[] Select;
    [SerializeField] private Transform Lift;
    [SerializeField] private float position = 0;
    [SerializeField] private KeyCode up;
    [SerializeField] private KeyCode down;
    [SerializeField] private KeyCode Enter;

    [SerializeField] private GameObject Home;
    [SerializeField] private float speed;

    private void Update()
    {
        LiftSelect();
        FindPosition();
        BeloworAbove();
        ContinueSelection();
    }

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

    void FindPosition()
    {
        if(position == 0)
        {
            Lift.position = Vector3.MoveTowards(Lift.position, Select[0].position, speed * Time.deltaTime);
        }
        if (position == 1)
        {
            Lift.position = Vector3.MoveTowards(Lift.position, Select[1].position, speed * Time.deltaTime);
        }
        if (position == 2)
        {
            Lift.position = Vector3.MoveTowards(Lift.position, Select[2].position, speed * Time.deltaTime);
        }
    }

    void BeloworAbove()
    {
        if(position <= 0)
        {
            position = 2;
        }
        if(position >= 3)
        {
            position = 1;
        }
    }

    void ContinueSelection()
    {
        if(Lift.position == Select[1].position && Input.GetKeyDown(Enter))
        {
            //SceneManager.LoadScene();
        }
        if(Lift.position == Select[2].position && Input.GetKeyDown(Enter))
        {
           //SceneManager.LoadScene();
        }
    }
}
