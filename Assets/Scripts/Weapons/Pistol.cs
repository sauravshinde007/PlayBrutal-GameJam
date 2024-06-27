
using UnityEngine;

public class Pistol : BaseWeapon {


    [SerializeField] private AudioSource audioSource; // The AudioSource component
    [SerializeField] private AudioClip shootSound;    // The shooting sound clip


    public override void Shoot(Vector2 shootDir)
    {
        if(CurrentAmmo <= 0){
            CurrentAmmo = 0;
            Debug.Log("Reload now");
            return;
        }
        CurrentAmmo --;

        // Play the shooting sound
        if (audioSource != null && shootSound != null)
        {
            audioSource.PlayOneShot(shootSound);
        }

        var bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity).GetComponent<Bullet>();
        bullet.SetDirection(shootDir);
    }
}
