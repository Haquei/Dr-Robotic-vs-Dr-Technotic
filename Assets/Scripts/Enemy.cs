using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{

    private NavMeshAgent myNavMeshAgent;
    private Transform target;

    private bool isFrozen;

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
        if (isFrozen) return;

        myNavMeshAgent.SetDestination(target.position);    
    }

    // Freeze the enemy for a given amount of time.
    public void Freeze(float seconds) => StartCoroutine(FreezeForSeconds(seconds));

    IEnumerator FreezeForSeconds(float seconds)
    {
        // Can put animations/effects here
        isFrozen = true;
        myNavMeshAgent.isStopped = true;
        yield return new WaitForSeconds(seconds);
        myNavMeshAgent.isStopped = false;
        isFrozen = false;
        // And remove animations/effects here
    }
}
