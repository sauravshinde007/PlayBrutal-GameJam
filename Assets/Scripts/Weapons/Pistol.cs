
using UnityEngine;

public class Pistol : BaseWeapon
{
    public override void Shoot(Vector2 shootDir)
    {
        if(CurrentAmmo <= 0){
            CurrentAmmo = 0;
            Debug.Log("Reload now");
            return;
        }
        CurrentAmmo --;
        var bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity).GetComponent<Bullet>();
        bullet.SetDirection(shootDir);
    }
}
