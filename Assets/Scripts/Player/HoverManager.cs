using UnityEngine;
using UnityEngine.UI;

public class HoverManager : MonoBehaviour
{

    private Rigidbody2D rb;
    private bool isHovering = false;
    private float gravity;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb != null) gravity = rb.gravityScale;

    }

    void Update()
    {
        // Hover Input
    }

    void FixedUpdate()
    {
        // Hovering
    }
}
