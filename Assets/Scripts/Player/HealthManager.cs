using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour {

    public int maxHealth = 100;
    private Slider healthSlider;

    public int currentHealth{
        get{
            return _currentHealth;
        }
        set{
            _currentHealth = value;
            if(value < 0) {
                _currentHealth = 0;
                Debug.Log("Dead");
                Destroy(gameObject);
            }
            if(value > maxHealth){
                _currentHealth = maxHealth;
                Debug.Log("Full Health");
            }

            if(healthSlider != null) healthSlider.value = _currentHealth;
            else healthSlider = GameObject.FindGameObjectWithTag("HealthSlider").GetComponent<Slider>();
        }
    }
    private int _currentHealth;

    void Start(){
        healthSlider = GameObject.FindGameObjectWithTag("HealthSlider").GetComponent<Slider>();
        if(healthSlider != null) healthSlider.maxValue = maxHealth;
        currentHealth = maxHealth;
    }

    public void Damage(int amt){
        currentHealth -= amt;
    }

    public void Heal(int amt){
        currentHealth += amt;
    }
}