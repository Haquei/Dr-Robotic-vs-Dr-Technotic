using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{

    private NavMeshAgent myNavMeshAgent;
    private Transform target;

    void Awake()
    {
        myNavMeshAgent = GetComponent<NavMeshAgent>();    
    }

    void Start()
    {
        target = FindObjectOfType<Builder>().transform;
    }

    void Update()
    {
        myNavMeshAgent.SetDestination(target.position);    
    }
}
