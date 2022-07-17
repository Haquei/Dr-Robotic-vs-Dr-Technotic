using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RRTower : MonoBehaviour
{
    private GameObject Target;
    public float Range;
    public float HealthTime = 10;
    bool IsReloading = false;
    public AudioSource PlacementAudio;
    public AudioSource ShootingAudio;
    void Start()
    {
        PlacementAudio.Play();
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
        ShootingAudio.Play();
        StartCoroutine(Killing());
        
        
        IsReloading = true;
        StartCoroutine(Reload());
        return;
    }
    IEnumerator Reload()
    {
        yield return new WaitForSeconds(2);
        IsReloading = false;
       
       
    }
    IEnumerator Health()
    {
        yield return new WaitForSeconds(HealthTime);
        Destroy(gameObject);
    }
    IEnumerator Killing
        ()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(Target);
    }
}
