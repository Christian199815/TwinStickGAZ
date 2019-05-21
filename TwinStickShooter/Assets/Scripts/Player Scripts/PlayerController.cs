using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    // Components
    private Rigidbody rb;

    // UI Elements
    private Transform healthbar;
    private Transform armorbar;
    private Text ammoText;

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

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        gameCamera = Camera.main;

        currentWeapon = weapons.weapons[0];
        currentWeapon.curAmmo = currentWeapon.ammo;
        currentWeapon.curClip = currentWeapon.clip;

        shootPoint = new GameObject("Shooting Point").transform;
        shootPoint.position = transform.position + shootOffset;
        shootPoint.parent = transform;

        lerpHealth = currentHealth;
        lerpArmor = currentArmor;

        healthbar = GameObject.Find("Healthbar").transform;
        armorbar = GameObject.Find("Armorbar").transform;
        ammoText = GameObject.Find("Ammo UI").GetComponent<Text>();
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

            if (currentWeapon.curAmmo <= 0 && currentWeapon.curClip >= currentWeapon.ammo)
            {
                currentWeapon.curAmmo = currentWeapon.ammo;
                currentWeapon.curClip -= currentWeapon.ammo;
                reloadTimer = 0;
            }
            else if(currentWeapon.curAmmo <= 0 && currentWeapon.curClip <= currentWeapon.ammo && currentWeapon.curClip > 0)
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

        healthbar.localScale = new Vector3((1.0f / maxHealth) * lerpHealth, 1, 1);
        armorbar.localScale = new Vector3((1.0f / maxArmor) * lerpArmor, 1, 1);

        ammoText.text = "Ammo: " + ((reloadTimer < currentWeapon.reloadDelay) ? 0 : currentWeapon.curAmmo) + "/" + currentWeapon.ammo + "  " + currentWeapon.curClip;
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
}
