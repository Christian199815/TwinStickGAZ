using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenElevatorDoor : MonoBehaviour
{
    [SerializeField] private int MoveSpeed;
    [SerializeField] private Transform destination;

   
    private void OnTriggerEnter(Collider other)
    {
       if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("check");
            //transform.position = Vector3.MoveTowards(transform.position, destination.position, MoveSpeed * Time.deltaTime);
            transform.position = destination.position;
        }
    }
}
