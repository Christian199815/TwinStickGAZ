using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MutantAI : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Transform[] Wpoints;
    [SerializeField] private int Dpoint;
    private Vector3 Rpoint;


    [SerializeField] private float MaxLookDis = 10;
    [SerializeField] private float MaxAttDis = 10;
    [SerializeField] private float MinDisfromPlayer = 0;

    [SerializeField] private float HitInterval = 0.5f;
    [SerializeField] private float HitTime = 0f;
    [SerializeField] public int Health = 50;
    [SerializeField] private float speed = 1.5f;
    [SerializeField] private int Counter = 0;
    [SerializeField] private int points;
    [SerializeField] private float RotDamp = 2;

    [SerializeField] private float Distance;
    [SerializeField] private NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false;
    }

    private void Update()
    {
        Vector3 targetDir = target.position - transform.position;
        float angleToPlayer = (Vector3.Angle(targetDir, transform.forward));

        if (angleToPlayer >= -90 && angleToPlayer <= 90 && Vector3.Distance(transform.position, target.position) <= Distance)
        {
            Debug.Log("Player in sight");
            Rpoint = target.position;

            GetComponent<NavMeshAgent>().destination = Rpoint;
            var distance = Vector3.Distance(target.position, transform.position);
            if(distance <= MaxLookDis)
            {
                LookatTarget();
                if(distance <= MaxAttDis && (Time.time - HitTime) > HitInterval)
                {
                    Attack();
                }
            }

        }
        else
        {
            if (!agent.pathPending && agent.remainingDistance < 0.5f)
                Movement();
        }

        Die();
    }

    void Attack()
    {
        HitTime = Time.time;
        
    }

    void LookatTarget()
    {
        var dir = target.position - transform.position;
        dir.y = 0;
        var rotation = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * RotDamp);
    }
    void Movement()
    {
        if (Wpoints.Length == 0)
            return;
        agent.destination = Wpoints[Dpoint].position;
        Dpoint = (Dpoint + 1) % Wpoints.Length;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "Player")
        {
            PlayerController.Instance.currentHealth -= 10;
        }

        if(collision.transform.tag == "Bullet")
        {
            Health -= 10;
        }
    }

    

    void Die()
    {
        if(Health <= 0)
        {
            PlayerController.statistics.score += 25;
            Destroy(gameObject);
        }
    }

}
