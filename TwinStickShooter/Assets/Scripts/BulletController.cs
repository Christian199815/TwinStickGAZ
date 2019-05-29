using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {

    private Rigidbody rb;

    [Header("Bullet settings")]
    public float bulletSpeed = 8.0f;
    public AiCharacterScript Ai;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update () {
		rb.velocity = transform.forward * bulletSpeed;
	}

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.tag == "Wall" || other.transform.tag == "Furniture")
        {
            Destroy(gameObject);
        }
        if(other.transform.tag == "Enemy")
        {

            other.gameObject.GetComponent<EnemyAI>().Health--;
            Destroy(gameObject);
        }
        if(other.transform.tag == "Player")
        {
            if (other.gameObject.GetComponent<PlayerController>().currentArmor >= 1)
            {
                other.gameObject.GetComponent<PlayerController>().currentArmor--;
                Destroy(gameObject);
            }
            else
            {
                other.gameObject.GetComponent<PlayerController>().currentHealth--;
                Destroy(gameObject);
            }
        }
    }
}
