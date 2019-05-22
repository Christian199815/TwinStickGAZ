using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public int weaponID;
    public bool inspectorSpawned = false;

    [Header("Weapon Info")]
    public Weapons.Weapon weapon;

    void Start()
    {
        if(inspectorSpawned)
        {
            weapon = Weapons.Instance.weapons[weaponID];
            weapon.curAmmo = weapon.maxAmmo;
            weapon.curClip = weapon.startClip;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();

        if (player != null && Input.GetKey(KeyCode.F))
        {
            bool c = player.ChangeWeapon(weapon);

            if(c)
            {
                Destroy(transform.parent.gameObject);
            }
        }
    }
}