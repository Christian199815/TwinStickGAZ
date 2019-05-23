using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeycardDoor : MonoBehaviour
{
    public static Dictionary<int, Color> keycardColors = new Dictionary<int, Color>();
    private Rigidbody rb;

    public int neededKeycard;

    private bool isOpened = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        GetComponent<MeshRenderer>().material.color = keycardColors[neededKeycard];
        rb.isKinematic = true;
    }

    private void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.gameObject.GetComponent<PlayerController>();

        if (player != null)
        {
            if (player.keycards.Contains(neededKeycard))
            {
                isOpened = true;
                rb.isKinematic = false;
            }
        }
    }
}
