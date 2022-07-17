using System.Linq;
using UnityEngine;

public class FireBall : Projectile
{
    [SerializeField] float blastRadius = 5f;
    [SerializeField] float damage = 5f;

    protected override void OnHitDestination()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, blastRadius);
        hits.Select(hit => hit.GetComponent<Enemy>())
            .Where(enemy => enemy != null)
            .ToList()
            .ForEach(enemy => enemy.TakeDamage(damage));
                
        Destroy(gameObject);
    }
}
