using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapons", menuName = "Game/Weapons")]
public class Weapons : ScriptableObject {
    [System.Serializable]
    public struct Weapon
    {
        public string weaponName;

        public float damage;
        public int ammo;
        public int curAmmo;
        public int clip;
        public int curClip;

        public float shootDelay;
        public float reloadDelay;

        public GameObject bullet;
    }

    public List<Weapon> weapons;
}
