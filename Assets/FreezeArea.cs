using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeArea : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
       if(other.gameObject.tag == "Enemy")
        {
            Freeze(other);
        }
    }
    
    public void Freeze(Collider other)
    {
    
        //everything you do to this function will happen to the Enemies(as I tested).
    }
}
