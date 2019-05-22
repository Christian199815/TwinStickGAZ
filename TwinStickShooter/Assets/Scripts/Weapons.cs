using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapons", menuName = "Game/Weapons")]
public class Weapons : ScriptableObject {

    public GameObject weaponPickupPrefab;
    public static Weapons Instance;

    public void SetInstance()
    {
        Instance = this;
    }

    [System.Serializable]
    public struct Weapon
    {
        public string weaponName;

        public float damage;
        public int maxAmmo;
        public int curAmmo;
        public int startClip;
        public int maxClip;
        public int curClip;

        public float shootDelay;
        public float reloadDelay;

        public GameObject bullet;
    }

    public List<Weapon> weapons;

    // Functions
    public void CreatePickup(int weaponID, Vector3 position)
    {
        GameObject obj = Instantiate(weaponPickupPrefab, new Vector3(position.x, 0, position.z), Quaternion.identity);
        WeaponPickup pickup = obj.transform.GetChild(0).GetComponent<WeaponPickup>();

        pickup.weapon = weapons[weaponID];
        pickup.weapon.curAmmo = pickup.weapon.maxAmmo;
        pickup.weapon.curClip = pickup.weapon.startClip;
    }

    public void CreatePickup(Weapon weapon, Vector3 position)
    {
        GameObject obj = Instantiate(weaponPickupPrefab, new Vector3(position.x, 0, position.z), Quaternion.identity);
        WeaponPickup pickup = obj.transform.GetChild(0).GetComponent<WeaponPickup>();

        pickup.weapon = weapon;
    }
}
