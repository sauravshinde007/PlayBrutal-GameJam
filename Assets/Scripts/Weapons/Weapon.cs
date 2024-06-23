
using System.Collections;
using UnityEngine;

public interface IWeapon {
    // Ammo
    public int CurrentAmmo {get; set;}
    public int MaxAmmo {get; set;}
    public int ShotsPerRound {get; set;}
    public int Rounds {get; set;}
    public int BulletsShotAtATime {get; set; }

    // Reloading
    public float ReloadTime {get; set;}
    public bool IsReloading {get; set;}

    public void Shoot(Vector2 shootDir);
    public IEnumerator Reload();
}
