using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HoverManager : MonoBehaviour
{
    [Header("Hover Controls")]
    public float hoverForce = 5f;
    public float maxFuel = 5f;
    public float fuelConsumptionRate = 1f;
    public float fuelRestoreRate = 0.5f;
    private Slider fuelSlider;

    public float currentFuel
    {
        get
        {
            return _currentFuel;
        }
        set
        {
            _currentFuel = value;
            if (value < 0)
            {
                _currentFuel = 0;
            }
            if (value > maxFuel)
            {
                _currentFuel = maxFuel;
            }

            if (fuelSlider != null) fuelSlider.value = _currentFuel;
            else fuelSlider = GameObject.FindGameObjectWithTag("FuelSlider").GetComponent<Slider>();
        }
    }
    private float _currentFuel;

    private Rigidbody2D rb;
    private bool isHovering = false;
    private float gravity;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb != null) gravity = rb.gravityScale;

        fuelSlider = GameObject.FindGameObjectWithTag("FuelSlider").GetComponent<Slider>();
        if (fuelSlider != null) fuelSlider.maxValue = maxFuel;
        currentFuel = maxFuel;
    }

    void Update()
    {
        // Hover Input
        if (Input.GetKey(KeyCode.E) && currentFuel > 0)
        {
            isHovering = true;
        }
        else
        {
            isHovering = false;
        }
    }

    void FixedUpdate()
    {
        // Hovering
        if (isHovering)
        {
            rb.gravityScale = 0; // Disable gravity while hovering
            rb.AddForce(Vector2.up * hoverForce, ForceMode2D.Force);
            currentFuel -= fuelConsumptionRate * Time.fixedDeltaTime;
        }
        else
        {
            rb.gravityScale = gravity; // Restore gravity
            if (currentFuel < maxFuel)
            {
                currentFuel += fuelRestoreRate * Time.fixedDeltaTime;
            }
        }
    }
}
