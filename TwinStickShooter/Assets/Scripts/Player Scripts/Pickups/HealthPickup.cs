using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();

        if (player != null)
        {
            if (player.currentHealth < player.maxHealth)
            {
                player.currentHealth += player.maxHealth / 2;
                if (player.currentHealth > player.maxHealth)
                    player.currentHealth = player.maxHealth;

                Destroy(gameObject);
            }
        }
    }
}
