using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RRTower : MonoBehaviour
{
    private GameObject Target;
    public float Range;

    bool IsReloading = false;
    void Start()
    {
        InvokeRepeating("UpdateTarget", 0, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Target != null && IsReloading == false)
        {
            KillTarget();
        }
    }
    void UpdateTarget()
    {
        GameObject[] Enemies = GameObject.FindGameObjectsWithTag("Enemy");
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
            Target = NearestEnemy;

        }
        else
        {
            Target = null;
        }


    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Range);
    }
    void KillTarget()
    {
        Destroy(Target);
        
        IsReloading = true;
        StartCoroutine(Reload());
        return;
    }
    IEnumerator Reload()
    {
        yield return new WaitForSeconds(2);
        IsReloading = false;
       
       
    }
}
