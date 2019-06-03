using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private Transform Target;
    [SerializeField] private Transform Projectile;
    [SerializeField] private Transform ShootOffset;

    [SerializeField] private Transform[] WPoints;
    [SerializeField] private int DPoint;
    private Vector3 CPoint;

    [SerializeField] private float MaximumLookDistance = 30;
    [SerializeField] private float MaximumAttackDistance = 10;
    [SerializeField] private float MinimumDistanceFromPlayer = 4;

    [SerializeField] private float rotationDamping = 2;
    [SerializeField] private float ShotInterval = 0.5f;
    [SerializeField] private float shotTime = 0f;


    [SerializeField] public int Health = 40;
    [SerializeField] private float Speed = 1.5f;
    [SerializeField] private int Counter = 0;
    [SerializeField] private int Points;

    [SerializeField] private float Distance;

    [SerializeField] private NavMeshAgent agent;


    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false;
    }
    private void Update()
    {
        Vector3 targetDir = Target.position - transform.position;
        float angleToPlayer = (Vector3.Angle(targetDir, transform.forward));

        if(angleToPlayer >= -90 && angleToPlayer <= 90 && Vector3.Distance(transform.position, Target.position) <= Distance)
        {
            Debug.Log("Player in sight");
            CPoint = Target.position;
            print(Distance);
            print(CPoint);
            GetComponent<NavMeshAgent>().destination = CPoint;
            var distance = Vector3.Distance(Target.position, transform.position);
            if (distance <= MaximumLookDistance)
            {
                LookAtTarget();
                if (distance <= MaximumAttackDistance && (Time.time - shotTime) > ShotInterval)
                {
                    Shoot();
                }
            }
        }          
        else
        {
            if(!agent.pathPending && agent.remainingDistance < 0.5f)
            Movement();
        }


        Die();
    }

    void Shoot()
    {
        shotTime = Time.time;
        Instantiate(Projectile, ShootOffset.position, Quaternion.LookRotation(Target.position - transform.position));
    }

    void LookAtTarget()
    {
        var dir = Target.position - transform.position;
        dir.y = 0;
        var rotation = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationDamping);
    }
    void Movement()
    {
        if (WPoints.Length == 0)
            return;
        agent.destination = WPoints[DPoint].position;
        DPoint = (DPoint + 1) % WPoints.Length;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "Bullet")
        {
            Health -= 10;
        }
    }

    void Die()
    {
        if(Health <= 0)
        {
            //overall score += Score;
            Destroy(gameObject);
        }
    }


}
