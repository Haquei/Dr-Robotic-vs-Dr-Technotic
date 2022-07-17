
// Ray Ray tower
public class RayTower : Tower
{

    protected override void Shoot()
    {
        Target.TakeDamage(Target.GetComponent<Health>().currentHealth);
    }
   
}
