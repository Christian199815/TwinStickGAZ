using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    [SerializeField] private GameObject Pause;
    [SerializeField] private GameObject GameOver;
    [SerializeField] private Transform Player;

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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause.SetActive(true);
            Time.timeScale = 0;
        }
    }
}
