using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FBTTower : MonoBehaviour
{
    //Vector3 FBTargetPos;
    //private Transform FBTargetFirstPos;
    public float Health = 15;
    public Transform Target;
    public Transform FirePoint;
    public float Range = 7f;
    public float FireRate = 1f;
    public float ReloadTime;
    bool IsReloading = false;
    public GameObject FBPrefab;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("UpdateTarget", 0, 5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Target != null && IsReloading == false)
        {
            ShootFireBall();
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
            Target = NearestEnemy.transform;

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
    void ShootFireBall()
    {
       GameObject FireBall =  (GameObject)Instantiate(FBPrefab, FirePoint.position, FirePoint.rotation);
        //FBTargetPos = FBTargetFirstPos.transform.position;
        //FireBall.transform.position = Vector3.MoveTowards(FirePoint.transform.position,Target.transform.position,100f * Time.deltaTime);
        IsReloading = true;
        StartCoroutine(Reload());
    }
    IEnumerator Reload()
    {
        yield return new WaitForSeconds(5);
        IsReloading = false;
    }
    IEnumerator Death ()
    {
        yield return new WaitForSeconds(Health);
        Destroy(gameObject);
    }

}
