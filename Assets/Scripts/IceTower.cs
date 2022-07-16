using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceTower : MonoBehaviour
{
    public float ReloadTime;
    public float Range;
    public GameObject Target;
    bool IsReloading = false;
    void Start()
    {
        InvokeRepeating("UpdateTarget", 0, 5);
    }

    // Update is called once per frame
    void Update()
    {
        if (Target != null)
        {
           
           
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Range);
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

    IEnumerator Freeze()
    {
        Target.GetComponent<Enemy>().enabled = false;

        Debug.Log("Dj");
        yield return new WaitForSeconds(ReloadTime);
        Target.GetComponent<Enemy>().enabled = true;


    }


}


