using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(Health))]
public class Enemy : MonoBehaviour
{

    protected NavMeshAgent myNavMeshAgent;
    protected Transform target;
    private Health myHealth;

    private bool isFrozen;
    private bool isSlowed;

    void Awake()
    {
        myNavMeshAgent = GetComponent<NavMeshAgent>();
        myHealth = GetComponent<Health>();
        myHealth.onDied.AddListener(Die);
    }

    protected virtual void Start()
    {
        target = FindObjectOfType<Builder>().transform;
    }

    void Update()
    {
        if (isFrozen) return;

        Move();
    }

    public void TakeDamage(float amount)
    {
        myHealth.TakeDamage(amount);
    }

    void Die()
    {
        Destroy(gameObject);
    }

    protected virtual void Move()
    {
        myNavMeshAgent.SetDestination(target.position);
    }

    // Freeze the enemy for a given amount of time.
    public void Freeze(float seconds) {
        // Avoids chaining the effect TODO: Make towers not target same frozen
        //  target twice. (ez fix for this and will feel better).
        if (isFrozen) return;
        StartCoroutine(FreezeForSeconds(seconds));
    }

    IEnumerator FreezeForSeconds(float seconds)
    {
        // Can put animations/effects here
        isFrozen = true;
        myNavMeshAgent.isStopped = true;
        yield return new WaitForSeconds(seconds);
        myNavMeshAgent.isStopped = false;
        isFrozen = false;
        // And remove animations/effects here
    }

    // Slow and enemy for the given duration by the given percent. Percent is between 0 and 1.
    // 0 would be not moving.
    // 1 would be not slowed at all (same speed).
    public void Slow(float duration, float slowPercent01) {
        if (isSlowed) return; // TODO see comment for Freeze.
        StartCoroutine(SlowForSeconds(duration, slowPercent01));
    }

    IEnumerator SlowForSeconds(float duration, float slowPercent01)
    {
        // Can put animations/effects here
        isSlowed = true;
        float originalSpeed = myNavMeshAgent.speed;
        float adjustedSpeed = originalSpeed * slowPercent01;
        myNavMeshAgent.speed = adjustedSpeed;
        yield return new WaitForSeconds(duration);
        myNavMeshAgent.speed = originalSpeed;
        isSlowed = false;
        // Remove animations/effects here.
    }
}
