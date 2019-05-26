using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    // Components
    private Rigidbody rb;

    // UI Elements
    private RectTransform healthbar;
    private RectTransform armorbar;
    private Text healthText;
    private Text armorText;
    private Text ammoText;
    private Text weaponText;

    // Player Variables
    [Header("Player Movement")]
    public float movementSpeed = 2.0f;
    public LayerMask rayMask;

    [Header("Camera Settings")]
    private Camera gameCamera;
    public float cameraSmooth = .2f;
    public Vector3 cameraOffset;

    [Header("Weapon Manager")]
    public Weapons weapons;
    public Weapons.Weapon currentWeapon;
    public Vector3 shootOffset;
    private Transform shootPoint;
    public int ammo;
    public int clips;
    public float shootTimer;
    public float reloadTimer;

    [Header("Player Information")]
    public int maxHealth;
    public int currentHealth;
    public int maxArmor;
    public int currentArmor;
    public bool healthCheat;
    public List<int> keycards;

    //Lerp Variables
    private float lerpHealth;
    private float lerpArmor;

    private void Awake()
    {
        weapons.SetInstance();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        gameCamera = Camera.main;

        currentWeapon = weapons.weapons[0];
        currentWeapon.curAmmo = currentWeapon.maxAmmo;
        currentWeapon.curClip = currentWeapon.startClip;

        shootPoint = new GameObject("Shooting Point").transform;
        shootPoint.position = transform.position + shootOffset;
        shootPoint.parent = transform;

        lerpHealth = currentHealth;
        lerpArmor = currentArmor;

        healthbar = GameObject.Find("Health Mask").GetComponent<RectTransform>();
        armorbar = GameObject.Find("Armor Mask").GetComponent<RectTransform>();

        healthText = GameObject.Find("Health Text").GetComponent<Text>();
        armorText = GameObject.Find("Armor Text").GetComponent<Text>();
        ammoText = GameObject.Find("Ammo Text").GetComponent<Text>();
        weaponText = GameObject.Find("Weapon Text").GetComponent<Text>();
    }

    void FixedUpdate()
    {
        #region Movement
        // Casting a ray to mouse position, then setting the rotation to the point of the hit and reseting the rotational values on the X and Z axis
        Ray mouseRay = gameCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Physics.Raycast(mouseRay, out hit, 100, rayMask);
        transform.LookAt(hit.point);
        transform.rotation = Quaternion.Euler(new Vector3(0, transform.rotation.eulerAngles.y, 0));

        // Setting the movement of the player using the rigidbody
        rb.velocity = new Vector3(movementSpeed * Input.GetAxisRaw("Horizontal"), 0, movementSpeed * Input.GetAxisRaw("Vertical"));

        // Camera movement by lerping between the camera position and the player position + the offset
        gameCamera.transform.position = Vector3.Lerp(gameCamera.transform.position, new Vector3(transform.position.x + cameraOffset.x, transform.position.y + cameraOffset.y, transform.position.z + cameraOffset.z), cameraSmooth);
        #endregion

        #region Shooting
        // Checking if the player tries to shoot and he is not reloading
        if (Input.GetMouseButton(0) && shootTimer > currentWeapon.shootDelay && reloadTimer > currentWeapon.reloadDelay)
        {
            // Instantiating the bullet at the position of the player

            if(currentWeapon.curAmmo > 0)
            {
                GameObject bulletObject = Instantiate(currentWeapon.bullet, shootPoint.position, transform.rotation);
                currentWeapon.curAmmo--;
            }

            if (currentWeapon.curAmmo <= 0 && currentWeapon.curClip >= currentWeapon.maxAmmo)
            {
                currentWeapon.curAmmo = currentWeapon.maxAmmo;
                currentWeapon.curClip -= currentWeapon.maxAmmo;
                reloadTimer = 0;
            }
            else if(currentWeapon.curAmmo <= 0 && currentWeapon.curClip <= currentWeapon.maxAmmo && currentWeapon.curClip > 0)
            {
                ammo = currentWeapon.curClip;
                currentWeapon.curClip = 0;
                reloadTimer = 0;
            }

            shootTimer = 0;
        }

        shootTimer += Time.deltaTime;
        reloadTimer += Time.deltaTime;
        #endregion

        #region Health
        if (healthCheat)
        {
            currentHealth = maxHealth;
            currentArmor = maxArmor;
        }

        lerpHealth = Mathf.Lerp(lerpHealth, currentHealth, .25f);
        lerpArmor = Mathf.Lerp(lerpArmor, currentArmor, .25f);

        healthbar.sizeDelta = new Vector2(lerpHealth * 48 ,healthbar.sizeDelta.y);
        armorbar.sizeDelta = new Vector2(lerpArmor * 45 ,healthbar.sizeDelta.y);

        healthText.text = Mathf.Round((100.0f / maxHealth) * lerpHealth) + "%";
        armorText.text = Mathf.Round((100.0f / maxArmor) * lerpArmor) + "%";

        ammoText.text = ((reloadTimer < currentWeapon.reloadDelay) ? 0 : currentWeapon.curAmmo) + " / " + currentWeapon.maxAmmo + "\n" + currentWeapon.curClip;
        weaponText.text = currentWeapon.weaponName;
        #endregion
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(255, 0, 0);

        if (Application.isEditor && !Application.isPlaying)
        {
            Gizmos.DrawSphere(transform.position + shootOffset, .05f);
        }
        else if(Application.isEditor && Application.isPlaying)
        {
            Gizmos.DrawSphere(shootPoint.position, .05f);
        }
    }

    // Functions
    public bool ChangeWeapon(Weapons.Weapon weapon)
    {
        print(weapon.weaponName);

        if(weapon.weaponName == currentWeapon.weaponName)
        {
            if (weapon.startClip + currentWeapon.startClip <= currentWeapon.maxClip)
            {
                currentWeapon.startClip += weapon.startClip;
                return true;
            }
            else if(currentWeapon.curClip <= currentWeapon.maxClip)
            {
                currentWeapon.startClip = currentWeapon.maxClip;
                return true;
            }
            else
            {
                print("wiwiwiwiwi");
                return false;
            }
        }
        else
        {
            if(currentWeapon.weaponName != "Nothing")
                weapons.RecreatePickup(currentWeapon, transform.position);

            print("burp");
            currentWeapon = weapon;
            return true;
        }
    }
}
