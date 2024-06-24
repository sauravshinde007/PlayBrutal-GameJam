
using UnityEngine;

public class Shotgun : BaseWeapon
{
    [SerializeField] private float spreadAngle = 45f;
    [Range(2, 20)]
    [SerializeField] private int bulletCount = 3;

    public override void Shoot(Vector2 shootDir)
    {
        if(CurrentAmmo < bulletCount){
            Debug.Log("Reload now");
            return;
        }
        CurrentAmmo -= bulletCount;

        // Calculate the starting angle of the spread
        float startAngle = -spreadAngle / 2f;
        float angleIncrement = spreadAngle / (bulletCount - 1);

        for (int i = 0; i < bulletCount; i++)
        {
            // Calculate the angle for this bullet
            float angle = startAngle + (angleIncrement * i);

            // Rotate the direction by the angle
            Vector2 bulletDirection = Quaternion.Euler(0, 0, angle) * shootDir;

            // Instantiate the bullet at the shooter's position
            var bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity).GetComponent<Bullet>();
            bullet.SetDirection(bulletDirection);

        }

    }
}
