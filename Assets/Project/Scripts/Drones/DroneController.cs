using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class DroneController : MonoBehaviour
{
    private NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        Vector3 randomTarget = transform.position + new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
        MooveTo(randomTarget);
    }

    private void MooveTo(Vector3 target)
    {
        agent.SetDestination(target);
    }
}
