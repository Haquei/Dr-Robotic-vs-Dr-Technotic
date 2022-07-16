using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Movement : MonoBehaviour
{

    [SerializeField] float speed = 6f;
    [SerializeField] float acceleration = 32f;

    private NavMeshAgent myNavMeshAgent;
    private bool isMoving;

    public float Speed => myNavMeshAgent.velocity.magnitude;

    private void Awake()
    {
        UpdateNavAgent();
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving && myNavMeshAgent.IsAtDestination())
        {
            isMoving = false;
        }
    }

    public void Move(Vector3 position)
    {
        myNavMeshAgent.isStopped = false;
        isMoving = true;
        myNavMeshAgent.SetDestination(position);
    }

    public void Stop()
    {
        myNavMeshAgent.isStopped = true;
        isMoving = false;
    }

    void UpdateNavAgent()
    {
        myNavMeshAgent = GetComponent<NavMeshAgent>();
        myNavMeshAgent.speed = speed;
        myNavMeshAgent.acceleration = acceleration;
        myNavMeshAgent.angularSpeed = 100000f;
    }

    // Called when inspector values are changed.
    void OnValidate()
    {
        UpdateNavAgent();
    }
}
