using System.Collections;
using TMPro;
using UnityEngine;

public class Pistol : MonoBehaviour, IWeapon{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform shootPoint;
    [SerializeField] float reloadTime;
    
    [Header("UI Stuff")]
    [SerializeField] private TMP_Text roundBulletsLeft;
    [SerializeField] private TMP_Text totalAmmoLeft;

    public void Start(){
        MaxAmmo = 128;
        ShotsPerRound = 16;
        Rounds = MaxAmmo / ShotsPerRound;
        BulletsShotAtATime = 1;
        CurrentAmmo = ShotsPerRound;

        IsReloading = false;
        ReloadTime = reloadTime;
    }

    #region IWeapon
    
    // Reloading
    public float ReloadTime {get; set;}
    public bool IsReloading {get; set;}
    
    // Ammo
    public int MaxAmmo {get; set;}
    public int ShotsPerRound {get; set;}
    public int BulletsShotAtATime {get; set; }

    public int CurrentAmmo {
        get{
            return _currentAmmo;
        }
        set{
            _currentAmmo = value;
            if(roundBulletsLeft != null) roundBulletsLeft.text = _currentAmmo.ToString();
        }
    }
    public int Rounds {
        get{
            return _rounds;
        }
        set{
            _rounds = value;
            if(totalAmmoLeft != null) totalAmmoLeft.text = (_rounds * ShotsPerRound).ToString();
        }
    }

    private int _currentAmmo;
    private int _rounds;

    public virtual void Shoot(Vector2 shootDir){
        if(CurrentAmmo <= 0){
            CurrentAmmo = 0;
            Debug.Log("Reload now");
            return;
        }
        CurrentAmmo --;

        var bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity).GetComponent<Bullet>();
        bullet.SetDirection(shootDir);
    }

    public virtual IEnumerator Reload(){
        if(CurrentAmmo == ShotsPerRound){
            Debug.Log("Already Full");
            yield return null;
        }
        if(Rounds <= 0){
            Debug.Log("No Ammo Left");
            yield return null;
        }

        Debug.Log("Reloading");
        IsReloading = true;
        yield return new WaitForSeconds(ReloadTime);

        CurrentAmmo = ShotsPerRound;
        Rounds --;
        IsReloading = false;
    }

    #endregion
}