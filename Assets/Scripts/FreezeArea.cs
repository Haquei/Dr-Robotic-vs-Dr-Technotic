using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeArea : MonoBehaviour
{
    public float RecoveryTime = 5f;
    bool IsRecovering = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Freeze(collision.collider);
            

        }
        
    }

    public void Freeze(Collider other)
    {
        other.gameObject.GetComponent<Enemy>().Freeze(5);
        IsRecovering = true;
      
        
        
    }
    IEnumerator Recovery()
    {
        yield return new WaitForSeconds(RecoveryTime);
        IsRecovering = false;

    }
}
