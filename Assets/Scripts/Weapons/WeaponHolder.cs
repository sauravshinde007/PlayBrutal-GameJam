using UnityEngine;

public class WeaponHolder : MonoBehaviour {
    [SerializeField] private Camera cam;
    
    [SerializeField] private Pistol pistol;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        Vector2 mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 lookDir = ((Vector2)transform.position - mousePosition).normalized;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        if(Input.GetMouseButtonDown(0)){
            pistol.Shoot(-lookDir);
        }

        if(Input.GetButtonDown("Reload")){
            StartCoroutine(pistol.Reload());
        }
    }
}
