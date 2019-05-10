using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiCharacterScript : MonoBehaviour
{
    public Transform[] points;
    private int DestPoint = 0;
    private NavMeshAgent agent;
    private float distance = 3;
    private Transform player;
    
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false;
        GotoNextPoint();

        
    }



    void GotoNextPoint()
    {
        if(points.Length == 0)
            return;

        agent.destination = points[DestPoint].position;
        DestPoint = (DestPoint + 1) % points.Length;
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, player.transform.position) <= distance)
        {
            GetComponent<NavMeshAgent>().destination = player.transform.position;
        }

        else
        {
            if (!agent.pathPending && agent.remainingDistance < 0.5f)
                GotoNextPoint();
        }
    }

    



}
