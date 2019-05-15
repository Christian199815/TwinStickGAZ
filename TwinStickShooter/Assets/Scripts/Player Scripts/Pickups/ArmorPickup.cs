using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorPickup : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();

        if (player != null)
        {
            if (player.currentArmor < player.maxArmor)
            {
                player.currentArmor += player.maxArmor / 2;
                if (player.currentArmor > player.maxArmor)
                    player.currentArmor = player.maxArmor;

                Destroy(gameObject);
            }
        }
    }
}
