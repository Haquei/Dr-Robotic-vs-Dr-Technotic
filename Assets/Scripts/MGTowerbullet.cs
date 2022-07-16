using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MGTowerbullet : MonoBehaviour
{
    private Transform BulletTarget;
    public float Speed = 10f;

    public void Chase(Transform _BulletTarget)
    {
        BulletTarget = _BulletTarget;
    }
    void Update()
    {
        if (BulletTarget == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 dir = BulletTarget.position - transform.position;
        float DistanceThisFrame = Speed * Time.deltaTime;


        if (dir.magnitude <= DistanceThisFrame)
        {
            HitTarget();
            return;
        }

        transform.Translate(dir.normalized * DistanceThisFrame, Space.World);

    }

    void HitTarget()
    {
        Destroy(gameObject);
    }
}
