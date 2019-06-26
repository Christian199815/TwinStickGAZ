using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{
    [SerializeField] private Transform[] select;
    [SerializeField] private Transform lift;
    [SerializeField] private float pos = 0;
    [SerializeField] private KeyCode up;
    [SerializeField] private KeyCode down;
    [SerializeField] private KeyCode enter;

    [SerializeField] private GameObject LvlSelect;
    [SerializeField] private GameObject Home;
    [SerializeField] private float speed;

    private void Start()
    {
        transform.position = select[0].position;
    }

    private void Update()
    {
        Liftselect();
        FindPos();
        BelowOrAbove();
        ContinueSelection();
    }

    void Liftselect()
    {
        if (Input.GetKeyDown(up))
             pos--;
        if (Input.GetKeyDown(down))
            pos++;
    }

    void FindPos()
    {
        if (pos == 0)
            lift.position = Vector3.MoveTowards(lift.position, select[0].position, speed * Time.deltaTime);
        if (pos == 1)
            lift.position = Vector3.MoveTowards(lift.position, select[1].position, speed * Time.deltaTime);
        if (pos == 2)
            lift.position = Vector3.MoveTowards(lift.position, select[2].position, speed * Time.deltaTime);
        if (pos == 3)
            lift.position = Vector3.MoveTowards(lift.position, select[3].position, speed * Time.deltaTime);
    }

    void BelowOrAbove()
    {
        if (pos <= 0)
            pos = 1;
        if (pos >= 4)
            pos = 3;
    }

    void ContinueSelection()
    {
        if (lift.position == select[1].position && Input.GetKeyDown(enter))
        {
            SceneManager.LoadScene(1);
        }
        if (lift.position == select[2].position && Input.GetKeyDown(enter))
        {
            SceneManager.LoadScene(2);
        }
        if (lift.position == select[3].position && Input.GetKeyDown(enter))
        {
            Home.SetActive(true);
            LvlSelect.SetActive(false);
        }
    }
}
