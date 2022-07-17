using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    [SerializeField] float timeToTarget = 1f;

    protected Vector3 destination;
    private bool isInitialized;
    private Vector3 startingPosition;
    private float currentTimeFiring;


    void Awake()
    {
        this.enabled = false;
    }

    void Update()
    {
        if (!isInitialized) return;

        currentTimeFiring += Time.deltaTime;
        float progress = currentTimeFiring / timeToTarget;

        transform.position = ParabolaHelper.LerpParabola(
                startingPosition, destination, 3, progress);

        if (progress >= 1) OnHitDestination();
    }

    protected abstract void OnHitDestination();

    public void Launch(Vector3 from, Vector3 to)
    {
        this.enabled = true;
        transform.position = from;
        
        this.startingPosition = from;
        this.destination = to;

        this.currentTimeFiring = 0f;

        isInitialized = true;
    }
}
