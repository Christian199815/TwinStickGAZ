using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    [SerializeField] private GameObject Pause;
    [SerializeField] private GameObject GameOver;
    [SerializeField] private Transform Player;

    [SerializeField] private KeyCode start;
    bool StartPressed;
    bool C_start;

    // Start is called before the first frame update
    void Start()
    {

        //GameOver.SetActive(false);
        //Pause.SetActive(false);
    }



   
    void Update()
    {
        BecomePaused();
        PlayerDeath();

        StartPressed = Input.GetKey(KeyCode.JoystickButton7);
    }

    void PlayerDeath()
    {
        if(Player.GetComponent<PlayerController>().currentHealth <= 0)
        {
            PlayerController.statistics.timesDied++;

            GameOver.SetActive(true);
            
            
        }
    }


    void BecomePaused()
    {
        if (StartPressed == true)
        {
            C_start = true;
            Pause.SetActive(true);
            Time.timeScale = 0.5f;
        }
        if (StartPressed == true && C_start == true)
        {
            C_start = false;
            Pause.SetActive(false);
            Time.timeScale = 1;
        }

    }
}
