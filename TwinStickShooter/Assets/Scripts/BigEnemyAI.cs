using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BigEnemyAI : MonoBehaviour
{
    private NavMeshAgent agent;
    private PlayerController playerScript;
    private Transform player;
    private SphereCollider col;

    public float radius;
    private Vector3 destination;
    private Ray ray;

    public List<Vector3> idlePoints;

    [Header("Enemy Sight")]
    public float fovAngle = 110f;
    public bool playerInSight = false;

    public Vector3 closestPoi;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        playerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        col = GetComponent<SphereCollider>();

        player = playerScript.transform;
    }

    void Update()
    {
        Vector3 curPoi = GetClosestPoI(player);

        if(new Vector3(Mathf.Round(transform.position.x), 0, Mathf.Round(transform.position.z)) == agent.destination.Round() || curPoi != closestPoi)
        {
            Start:

            closestPoi = curPoi;
            destination = closestPoi + new Vector3(Random.Range(-radius, radius), 2, Random.Range(-radius, radius));

            ray = new Ray(transform.position, destination - transform.position);
            //if(Physics.Raycast(ray))
            //    goto Start;
        }

        if(playerInSight)
            agent.destination = player.position;
        else
            agent.destination = destination;
    }

    private void OnDrawGizmosSelected()
    {
        foreach (Vector3 pos in idlePoints)
        {
            Gizmos.color = Color.yellow;

            if(pos == closestPoi)
            {
                Gizmos.color = Color.green;
            }

            Gizmos.DrawSphere(pos, 1);
        }

        Gizmos.DrawLine(transform.position, new Vector3(destination.x, 0, destination.z));
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.name == "Player")
        {
            playerInSight = false;

            Vector3 direction = player.position - transform.position;
            float angle = Vector3.Angle(direction, transform.forward);

            if (angle < fovAngle * .5f)
            {
                RaycastHit hit;

                if (Physics.Raycast(transform.position + transform.up, direction.normalized, out hit, col.radius))
                {
                    if(hit.collider.name == "Player")
                    {
                        playerInSight = true;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Gets all points of interests and looks at which is the closest to the object
    /// </summary>
    /// <returns></returns>
    private Vector3 GetClosestPoI(Transform obj)
    {
        float closestDistance = 999;
        Vector3 closestPoint = new Vector3();

        foreach(Vector3 pos in idlePoints)
        {
            float distance = Mathf.Abs(Vector3.Distance(obj.position, pos));

            if(distance < closestDistance)
            {
                closestDistance = distance;
                closestPoint = pos;
            }
        }

        return closestPoint;
    }
}

public static class VectorMath
{
    public static Vector3 Round(this Vector3 vec)
    {
        return new Vector3(Mathf.Round(vec.x), Mathf.Round(vec.y), Mathf.Round(vec.z));
    }
}
