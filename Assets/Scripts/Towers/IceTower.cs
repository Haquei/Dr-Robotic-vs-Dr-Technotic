
public class IceTower : Tower
{
    public IceBall IceBallPrefab;

    protected override void Shoot()
    {
        IceBall iceBall = Instantiate(IceBallPrefab, firepoint.position, firepoint.rotation);
        iceBall.Launch(firepoint.position, Target.transform.position);
    }

}
