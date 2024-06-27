using Photon.Pun;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private int damageAmt = 10;
    [SerializeField] private float destroyTime = 4f;
    [SerializeField] private float speed = 40f;

    [SerializeField] private GameObject bulletImpact;

    private Vector2 direction;
    private float destroyCtr = 0;

    public void SetDirection(Vector2 dir){
        // direction = dir;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);

        destroyCtr += Time.deltaTime;
        if(destroyCtr > destroyTime){
            Destroy(gameObject);
        }
    }
    
    void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Player")){
            var hm = other.GetComponent<HealthManager>();
            hm.Damage(damageAmt);
            var fx = Instantiate(bulletImpact, transform.position, Quaternion.identity);
            Destroy(fx, 1f);
        }
        Destroy(gameObject);
    }
}
