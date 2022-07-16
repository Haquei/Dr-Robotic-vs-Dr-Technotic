using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MGTower : MonoBehaviour
{
    private Transform Target;

    [Header("Attributes")]

    public float Range;
    public float FireRate = 1f;
    private float FireCountDown = 0f;




    [Header("UnityField")]

    public string EnemyTag = "Enemy";
    public GameObject BulletPrefab;
    public Transform FirePoint;
    void Start()
    {
        InvokeRepeating("UpdateTarget", 0,0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Target == null)
        {
            return;
        }
        if (FireCountDown <= 0f)
        {
            Shoot();
            FireCountDown = 1f / FireRate;
        }
        FireCountDown -= Time.deltaTime;
    }
    void UpdateTarget()
    {
        GameObject[] Enemies = GameObject.FindGameObjectsWithTag(EnemyTag);
        float ShortestDistance = Mathf.Infinity;
        GameObject NearestEnemy = null;

        foreach (GameObject Enemy in Enemies)
        {
            float DistanceToEnemy = Vector3.Distance(transform.position, Enemy.transform.position);
            if (DistanceToEnemy < ShortestDistance)
            {
                ShortestDistance = DistanceToEnemy;
                NearestEnemy = Enemy;

            }


        }
        if (NearestEnemy != null && ShortestDistance <= Range)
        {
            Target = NearestEnemy.transform;

        }
        else
        {
            Target = null;
        }


    }
    void Shoot()
    {


        if (Target != null)
        {
            GameObject BulletGO = (GameObject)Instantiate(BulletPrefab, FirePoint.position, FirePoint.rotation);
            MGTowerbullet bullet = BulletGO.GetComponent<MGTowerbullet>();
            if (bullet != null)
            {
                bullet.Chase(Target);
            }
        }



    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Range);
    }
}
