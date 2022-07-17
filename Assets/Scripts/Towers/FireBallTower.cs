
public class FireBallTower : Tower
{
    public FireBall FireBallPrefab;

    protected override void Shoot()
    {
        FireBall fireBall = Instantiate(FireBallPrefab, firepoint.position, firepoint.rotation);
        fireBall.Launch(firepoint.position, Target.transform.position);
    }

}
