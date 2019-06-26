using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }
    // Components
    private Rigidbody rb;

    // UI Elements
    private RectTransform healthbar;
    private RectTransform armorbar;
    private Text healthText;
    private Text armorText;
    private Text ammoText;
    private Text weaponText;
    private Text statText;

    // Statistics
    public static PlayerStats statistics;

    // Player Variables
    [Header("Player Movement")]
    public float movementSpeed = 2.0f;
    public float rotationDamp = .75f;
    private Vector3 rotation;

    [Header("Camera Settings")]
    private Transform gameCamera;
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

    [Header("Lighting")]
    public float radius = 10;
    public int rays = 180;
    private Vector2[] meshVerts;
    private MeshFilter lightLayer;

    //Lerp Variables
    private float lerpHealth;
    private float lerpArmor;

    private void Awake()
    {
        Instance = this;
        weapons.SetInstance();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        gameCamera = GameObject.Find("Cameras").transform;

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
        statText = GameObject.Find("Stats Text").GetComponent<Text>();

        statistics = new PlayerStats();

        meshVerts = new Vector2[rays];
        lightLayer = GameObject.Find("LightLayer").GetComponent<MeshFilter>();
    }

    void FixedUpdate()
    {
        #region Movement
        
        if(Input.GetAxis("RHorizontal") != 0.0f || Input.GetAxis("RVertical") != 0.0f)
        {
            rotation = new Vector3(Input.GetAxis("RHorizontal"), transform.forward.y, -Input.GetAxisRaw("RVertical"));
        }

        transform.forward = Vector3.Lerp(transform.forward, rotation, rotationDamp);
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);

        // Setting the movement of the player using the rigidbody
        rb.velocity = new Vector3(movementSpeed * Input.GetAxisRaw("Horizontal"), 0, movementSpeed * Input.GetAxisRaw("Vertical"));

        // Camera movement by lerping between the camera position and the player position + the offset
        gameCamera.transform.position = Vector3.Lerp(gameCamera.transform.position, new Vector3(transform.position.x + cameraOffset.x, transform.position.y + cameraOffset.y, transform.position.z + cameraOffset.z), cameraSmooth);
        #endregion

        #region Shooting
        // Checking if the player tries to shoot and he is not reloading
        if (Input.GetButton("Fire") && shootTimer > currentWeapon.shootDelay && reloadTimer > currentWeapon.reloadDelay)
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

        #region Lighting
        for (int i = 0; i < rays; i++)
        {
            float stepSize = 360f / rays;

            Vector3 dir = Quaternion.Euler(0, 360 - (i * stepSize), 0) * Vector3.right * radius;

            float angle = (i * stepSize * Mathf.PI / 180f);
            float x = Mathf.Cos(angle) * radius;
            float y = Mathf.Sin(angle) * radius;

            Ray ray = new Ray(transform.position, dir);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit, radius))
            {
                meshVerts[i] = hit.point.Convert2D();
            }
            else
            {
                meshVerts[i] = new Vector2(x, y) + transform.position.Convert2D();
            }

            //Debug.DrawRay(transform.position, Quaternion.Euler(0, i * 360 / rays, 0) * Vector3.forward * radius);
        }

        Triangulator tr = new Triangulator(meshVerts);
        int[] indices = tr.Triangulate();

        Vector3[] vertices = new Vector3[meshVerts.Length];
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = new Vector3(meshVerts[i].x, meshVerts[i].y, 0);
        }

        Mesh msh = new Mesh();
        msh.vertices = vertices;
        msh.triangles = indices;
        msh.RecalculateNormals();
        msh.RecalculateBounds();

        lightLayer.mesh = msh;

        #endregion

        statistics.timePlayed += Time.deltaTime;

        statText.text = "Score: " + statistics.score + "\n" + "Times Player: " + statistics.timesDied + "\n" + "Play Time: " + ((int)(statistics.timePlayed / 60.0f / 60.0f) + ":" + (int)((statistics.timePlayed / 60.0f) % 60) + ":" + (int)(statistics.timePlayed % 60));

        //if(Time.)

        //statistics.SaveToPrefs();
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

        //for (int i = 0; i < meshVerts.Length; i++)
        //{
        //    Gizmos.color = Color.red;
        //    Gizmos.DrawSphere(meshVerts[i].Convert3D(2), .2f);
        //    Gizmos.color = Color.white;
        //    Gizmos.DrawLine(transform.position,meshVerts[i].Convert3D(2));
        //}

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

    public struct PlayerStats
    {
        public int enemiesKilled;
        public int timesDied;
        public int weaponPickedup;
        public int healthPickedup;
        public int ammoPickedup;
        public int armorPickedup;
        public float timePlayed;
        public int score;

        public PlayerStats(bool loadFromPrefs = true)
        {
            enemiesKilled = PlayerPrefs.GetInt("EnemiesKilled", 0);
            timesDied = PlayerPrefs.GetInt("TimesDied", 0);
            weaponPickedup = PlayerPrefs.GetInt("WeaponPickedup", 0);
            healthPickedup = PlayerPrefs.GetInt("HealthPickedup", 0);
            ammoPickedup = PlayerPrefs.GetInt("AmmoPickedup", 0);
            armorPickedup = PlayerPrefs.GetInt("ArmorPickedup", 0);
            timePlayed = PlayerPrefs.GetInt("TimePlayed", 0);

            score = 0;
        }

        public void SaveToPrefs()
        {
            PlayerPrefs.SetInt("EnemiesKilled", enemiesKilled);
            PlayerPrefs.SetInt("TimesDied", enemiesKilled);
            PlayerPrefs.SetInt("WeaponPickedup", enemiesKilled);
            PlayerPrefs.SetInt("HealthPickedup", enemiesKilled);
            PlayerPrefs.SetInt("AmmoPickedup", enemiesKilled);
            PlayerPrefs.SetInt("ArmorPickedup", enemiesKilled);

            print("save");
        }
    }
}
