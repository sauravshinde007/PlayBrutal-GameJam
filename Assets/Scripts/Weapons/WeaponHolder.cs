using System.Collections.Generic;
using UnityEngine;

public class WeaponHolder : MonoBehaviour {
    [SerializeField] private Camera cam;
    
    private BaseWeapon[] weapons;
    private BaseWeapon currentWeapon;

    // Start is called before the first frame update
    void Start() {
        List<BaseWeapon> ws = new List<BaseWeapon>();
        for(int i = 0; i < transform.childCount; i++){
            var w = transform.GetChild(i).GetComponent<BaseWeapon>();
            if(w != null) ws.Add(w);
        }
        weapons = ws.ToArray();

        SelectWeapon(0);
    }

    void SelectWeapon(int index){
        for(int i = 0; i < weapons.Length; i++){
            if(i == index) currentWeapon = weapons[i];
            else weapons[i].gameObject.SetActive(false);
        }

        currentWeapon.gameObject.SetActive(true);
        currentWeapon.UpdateUI();
    }

    // Update is called once per frame
    void Update() {
        if(Input.GetKeyDown(KeyCode.Alpha1)){
            SelectWeapon(0);
        }
        if(Input.GetKeyDown(KeyCode.Alpha2)){
            SelectWeapon(1);
        }
        if(Input.GetKeyDown(KeyCode.Alpha3)){
            SelectWeapon(2);
        }

        Vector2 mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 lookDir = ((Vector2)transform.position - mousePosition).normalized;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        if(Input.GetMouseButtonDown(0)){
            currentWeapon.Shoot(-lookDir);
        }

        if(Input.GetButtonDown("Reload")){
            StartCoroutine(currentWeapon.Reload());
        }
    }
}
