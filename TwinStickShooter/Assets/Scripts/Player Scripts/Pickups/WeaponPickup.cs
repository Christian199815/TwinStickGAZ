﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponPickup : MonoBehaviour
{
    private Text pickupText;

    public int weaponID;
    public bool inspectorSpawned = false;

    [Header("Weapon Info")]
    public Weapons.Weapon weapon;

    void Start()
    {
        if (inspectorSpawned)
        {
            weapon = Weapons.Instance.weapons[weaponID];
            weapon.curAmmo = weapon.maxAmmo;
            weapon.curClip = weapon.startClip;
        }

        pickupText = GameObject.Find("Weapon Pickup Text").GetComponent<Text>();
    }

    private void OnTriggerStay(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();

        if (player != null)
        {
            //pickupText.text = "Press F to pick up " + weapon.weaponName;

            if (Input.GetKeyUp(KeyCode.F))
            {
                bool c = player.ChangeWeapon(weapon);

                print(c);

                pickupText.text = "";

                if (c)
                {
                    Destroy(transform.parent.gameObject);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();

        if (player != null)
        {
            pickupText.text = "";
        }
    }
}