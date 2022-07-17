using UnityEngine;

public class MachineGunTower : Tower
{
    [Header("Machine Gun Attributes")]
    public GameObject BulletPrefab;
    public float longReloadTime = 5f;
    public int maxAmmo = 10;
    
    private float ammo;

    protected override bool CanShoot()
    {
        if (ammo > 0) return base.CanShoot();

        // Out of ammo. LongReload.
        if (Time.time - lastFiredTime > longReloadTime)
        {
            ammo = maxAmmo;
            return true;
        }
        
        return false;
    }

    protected override void Shoot()
    {
        ammo--;

        GameObject BulletGO = Instantiate(BulletPrefab, firepoint.position, firepoint.rotation);
        MGTowerbullet bullet = BulletGO.GetComponent<MGTowerbullet>();
        if (bullet != null)
        {
            bullet.Chase(Target.transform);
        }
    }
   
}
