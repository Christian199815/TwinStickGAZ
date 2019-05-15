using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();

        if (player != null)
        {
            player.currentWeapon.curClip += player.currentWeapon.ammo * 3;
            Destroy(gameObject);
        }
    }
}
