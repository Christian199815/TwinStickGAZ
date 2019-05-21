using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class AiCharacterScript : MonoBehaviour
{
    [SerializeField] private Transform Target;
    [SerializeField] private Transform Projectile;

    [SerializeField] private Transform[] WayPoints;
    [SerializeField] private int DestPoint;
   // [SerializeField] private Transform 
    

    [SerializeField] private float maximumLookDistance = 30;
    [SerializeField] private float maximumAttackDistance = 10;
    [SerializeField] private float minimumDistanceFromPlayer = 2;

    [SerializeField] private float rotationDamping = 2;
    [SerializeField] private float shotInterval = 0.5f;
    [SerializeField] private float shotTime = 0f;

    [SerializeField] private int Health = 40;
    [SerializeField] private float Speed = 1.5f;
    [SerializeField] private int Counter = 0;

    [SerializeField] private float Distance;

    [SerializeField] private NavMeshAgent agent;


    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false;

    }
    private void Update()
    {

        if(Vector3.Distance(transform.position, Target.position) <= Distance)
        {
            GetComponent<NavMeshAgent>().destination = Target.position;
            var distance = Vector3.Distance(Target.position, transform.position);
            if (distance <= maximumLookDistance)
            {
                LookAtTarget();
                if (distance <= maximumAttackDistance && (Time.time - shotTime) > shotInterval)
                {
                    Shoot();
                }
            }
        }
        else
        {
            if (!agent.pathPending && agent.remainingDistance < 0.5f)
                GotoNextPoint();
        }

        
       
       
        Die();
    }



    void LookAtTarget()
    {
        var dir = Target.position - transform.position;
        dir.y = 0;
        var rotation = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationDamping);


    }

    void Shoot()
    {
        shotTime = Time.time;
        Instantiate(Projectile, transform.position + (Target.position - transform.position).normalized, Quaternion.LookRotation(Target.position - transform.position));

    }

    private void OnCollisionEnter(Collision collision)
    {
         if (collision.transform.tag == "Bullet")
            {
            Health -= 10;
            }
    }

    void Die()
    {
        if(Health <= 0)
        {
            Destroy(gameObject);
        }
    }

   void GotoNextPoint()
    {
        if (WayPoints.Length == 0)
            return;
        agent.destination = WayPoints[DestPoint].position;
        DestPoint = (DestPoint + 1) % WayPoints.Length;
    }



}
