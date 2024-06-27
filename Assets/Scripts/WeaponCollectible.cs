using Photon.Pun;
using UnityEngine;

public class WeaponCollectible : MonoBehaviour
{
    [SerializeField] private bool pistol = true;

    void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Player")){
            Debug.Log("Player in range");
            other.GetComponent<PlayerMovement>().AddWeapon(pistol);
            other.GetComponent<PlayerMovement>().weaponHolder.GetComponent<WeaponHolder>().RefreshWeapons();
            PhotonNetwork.Destroy(this.gameObject);
        }
    }
}
