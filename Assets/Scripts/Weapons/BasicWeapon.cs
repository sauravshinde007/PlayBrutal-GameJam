using System.Collections;
using TMPro;
using UnityEngine;

public class BaseWeapon : MonoBehaviour, IWeapon {
    public GameObject bulletPrefab;
    public Transform shootPoint;
    public float reloadTime;
    
    private TMP_Text roundBulletsLeft;
    private TMP_Text totalAmmoLeft;

    public void Start(){
        MaxAmmo = 128;
        ShotsPerRound = 16;
        Rounds = MaxAmmo / ShotsPerRound;
        BulletsShotAtATime = 1;
        CurrentAmmo = ShotsPerRound;

        IsReloading = false;
        ReloadTime = reloadTime;

        roundBulletsLeft = GameObject.FindGameObjectWithTag("BulletsLeft").GetComponent<TMP_Text>();
        totalAmmoLeft = GameObject.FindGameObjectWithTag("TotalAmmoLeft").GetComponent<TMP_Text>();
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

    public void UpdateUI(){
        if(roundBulletsLeft != null) roundBulletsLeft.text = _currentAmmo.ToString();
        if(totalAmmoLeft != null) totalAmmoLeft.text = (_rounds * ShotsPerRound).ToString();
    }

    private int _currentAmmo;
    private int _rounds;

    public virtual void Shoot(Vector2 shootDir){ }

    public virtual void Reload(){
        if(CurrentAmmo == ShotsPerRound){
            Debug.Log("Already Full");
            return;
        }
        if(Rounds <= 0){
            Debug.Log("No Ammo Left");
            return;
        }
        CurrentAmmo = ShotsPerRound;
        Rounds --;
    }

    #endregion
}