using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class WeaponHolder : MonoBehaviour {
    [SerializeField] private PhotonView view;
    
    private Camera cam;
    private List<BaseWeapon> weapons = new List<BaseWeapon>();
    private BaseWeapon currentWeapon;
    private int currentWeaponIndex;

    // Start is called before the first frame update
    void Start() {
        cam = Camera.main;

        for(int i = 0; i < transform.childCount; i++){
            var w = transform.GetChild(i).GetComponent<BaseWeapon>();
            if(w != null) weapons.Add(w);
        }

        view.RPC("SelectWeapon", RpcTarget.All, 0);
    }

    [PunRPC]
    void SelectWeapon(int index){
        for(int i = 0; i < weapons.Count; i++){
            if(i == index) currentWeapon = weapons[i];
            else weapons[i].gameObject.SetActive(false);
        }

        if(currentWeapon != null){
            currentWeapon.gameObject.SetActive(true);
            currentWeapon.UpdateUI();
        }
    }

    void SwitchWeapon(){
        float scrollWheel = Input.GetAxisRaw("Mouse ScrollWheel");
        if(scrollWheel > 0f){
            currentWeaponIndex = (currentWeaponIndex + 1) % weapons.Count;
        }
        else if(scrollWheel < 0f){
            currentWeaponIndex --;
            if(currentWeaponIndex < 0) currentWeaponIndex  = weapons.Count - 1;
        }
        view.RPC("SelectWeapon", RpcTarget.All, currentWeaponIndex);
    }

    [PunRPC]
    void Shoot(Vector2 lookDir){
        currentWeapon.Shoot(-lookDir);
    }

    // Update is called once per frame
    void Update() {
        if(view != null && !view.IsMine) return;

        SwitchWeapon();

        // Get Angle and direction
        Vector2 mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 lookDir = ((Vector2)transform.position - mousePosition).normalized;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        // Shoot
        if(Input.GetMouseButtonDown(0)){
            view.RPC("Shoot", RpcTarget.All, lookDir);
        }

        // Reloading
        if(Input.GetButtonDown("Reload") || Input.GetMouseButtonDown(2)){
            currentWeapon.Reload();
        }
    }
}
