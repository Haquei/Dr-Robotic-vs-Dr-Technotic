using System.Linq;
using UnityEngine;

public class IceBall : Projectile
{
    [SerializeField] float blastRadius = 5f;
    [SerializeField] float damage = 5f;
    [SerializeField][Range(0, 100)] float slowPercentage = 50f;
    [SerializeField] float slowDuration = 3;

    float slowFactor01 => slowPercentage / 100f;

    protected override void OnHitDestination()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, blastRadius);
        hits.Select(hit => hit.GetComponent<Enemy>())
            .Where(enemy => enemy != null)
            .ToList()
            .ForEach(enemy => {
                enemy.TakeDamage(damage);
                enemy.Slow(slowDuration, slowFactor01);
            });

        Destroy(gameObject);
    }
}
