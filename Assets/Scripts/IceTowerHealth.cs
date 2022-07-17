using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceTowerHealth : MonoBehaviour
{
    public float Health = 5f;
    public AudioSource PlacementAudio;
    void Start()
    {
        PlacementAudio.Play();
        StartCoroutine(HealthTimer());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator HealthTimer()
    {
        yield return new WaitForSeconds(Health);
        Destroy(gameObject);
    }
}
