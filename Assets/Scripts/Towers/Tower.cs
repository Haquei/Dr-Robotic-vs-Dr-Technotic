using UnityEngine;

public abstract class Tower : MonoBehaviour
{
    public AudioSource PlacementAudio;
    public AudioSource ShootingFBAudio;

    public float health = 15;
    public Transform firepoint;
    public Transform topBit;

    [Header("Attributes")]
    public float Range = 7f;
    public float ReloadTime;
    protected float lastFiredTime;
    protected bool IsReloading => Time.time - lastFiredTime < ReloadTime;

    protected Enemy Target;

    void Start()
    {
        PlacementAudio.Play();
        lastFiredTime = -ReloadTime;
    }

    void Update()
    {
        if (health <= 0f) Die();
        if (Target == null) Target = FindTarget();
        if (Target == null) return;

        transform.rotation = Quaternion.LookRotation(transform.position - Target.transform.position);

        if (CanShoot())
        {
            ShootingFBAudio.Play();
            lastFiredTime = Time.time;
            Shoot();
        }
    }

    private void Die()
    {
        Building bc = GetComponent<Building>();
        FindObjectOfType<Grid>().ClearGrid(bc.GridX, bc.GridZ, bc.WidthInBlocks, bc.HeightInBlocks);
        Destroy(gameObject);
    }

    protected virtual bool CanShoot()
    {
        return !IsReloading;
    }

    protected abstract void Shoot();

    private Enemy FindTarget()
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
            return NearestEnemy.GetComponent<Enemy>();
        }

        return null;

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Range);
    }
}
