using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(Health))]
public class Enemy : MonoBehaviour
{
    private const float AGGRO_RADIUS = 6f;

    [SerializeField] float attackRange = 5f;
    [SerializeField] float timeBetweenAttacks = 2f;
    [SerializeField] float attackDamage = 5f;
    private float lastAttackTime = -100f;

    protected NavMeshAgent myNavMeshAgent;
    protected Transform target;
    private Health myHealth;

    private Tower towerTarget;

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

        if (towerTarget != null)
        {
            float dist = Vector3.Distance(transform.position, towerTarget.transform.position);
            if (dist <= attackRange)
            {
                myNavMeshAgent.SetDestination(transform.position);
                TryAttack();
            }
            return;
        }

        // Check for towers in range
        Tower[] towers = nearbyTowers();
        if (towers.Length != 0)
        {
            towerTarget = towers[0];
            myNavMeshAgent.SetDestination(towerTarget.transform.position);
            return;
        }

        float distToTarget = Vector3.Distance(transform.position, target.transform.position);
        if (distToTarget < 3.5f)
        {
            Debug.Log("HIT YOUR CORE!"); // TODO lose condition.
            Die();
            return;
        }

        Move();
    }

    void TryAttack() {
        if (Time.time - lastAttackTime < timeBetweenAttacks) return;

        towerTarget.health -= attackDamage;
        lastAttackTime = Time.time;
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

    private Tower[] nearbyTowers()
    {
        return Physics.OverlapSphere(transform.position, AGGRO_RADIUS)
            .Select(hit => hit.GetComponent<Tower>())
            .Where(tower => tower != null)
            .OrderBy(tower => Vector3.Distance(tower.transform.position, transform.position))
            .ToArray();
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
