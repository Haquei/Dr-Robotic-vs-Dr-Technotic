using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : Enemy
{
    [SerializeField] float flySpeed;

    protected override void Start()
    {
        base.Start();
        myNavMeshAgent.enabled = false;
    }

    protected override void Move()
    {
        Vector3 toTarget = target.transform.position - transform.position;
        toTarget.Normalize();

        transform.position += toTarget * flySpeed * Time.deltaTime;
    }
}
