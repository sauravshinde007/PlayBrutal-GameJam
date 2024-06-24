using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private int damageAmt = 10;
    [SerializeField] private float destroyTime = 4f;
    [SerializeField] private float speed = 40f;

    private Vector2 direction;


    public void SetDirection(Vector2 dir){
        direction = dir;
    }

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, destroyTime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }
    
    void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Player")){
            var hm = other.GetComponent<HealthManager>();
            hm.Damage(damageAmt);
        }
    }
}
