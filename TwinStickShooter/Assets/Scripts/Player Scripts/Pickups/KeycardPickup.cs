using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeycardPickup : MonoBehaviour
{
    public int keycard;
    public Color keycardColor;

    private void Awake()
    {
        KeycardDoor.keycardColors.Add(keycard, keycardColor);
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();

        if (player != null)
        {
            if(!player.keycards.Contains(keycard))
            {
                player.keycards.Add(keycard);
                Destroy(gameObject);
            }
        }
    }
}
