using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    // Components
    private Rigidbody rb;

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
    public float shootTimer;
    public float reloadTimer;

    [Header("Player Information")]
    public int Health;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        gameCamera = Camera.main;

        currentWeapon = weapons.weapons[0];
        ammo = currentWeapon.ammo;

        shootPoint = new GameObject("Shooting Point").transform;
        shootPoint.position = transform.position + shootOffset;
        shootPoint.parent = transform;
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
            GameObject bulletObject = Instantiate(currentWeapon.bullet, shootPoint.position, transform.rotation);

            ammo--;

            if (ammo <= 0)
            {
                ammo = currentWeapon.ammo;
                reloadTimer = 0;
            }

            shootTimer = 0;
        }

        shootTimer += Time.deltaTime;
        reloadTimer += Time.deltaTime;
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
