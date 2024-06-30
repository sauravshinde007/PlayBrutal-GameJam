using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using System;

public class PlayerMovement : MonoBehaviour {

    private bool isDead = false;

    [Header("Multiplayer")]
    [SerializeField] private PhotonView view;
    [SerializeField] public GameObject weaponHolder;

    [Header("Movement Controls")]
    [SerializeField] float speed = 450f;
    [SerializeField] Transform feetPos; // feet position to check if grounded
    [SerializeField] Vector3 groundCheckSize = new Vector3(1, 0.05f, 1);
    [SerializeField] float gravity = 10; // gravity to apply when in air
    [SerializeField] LayerMask whatIsGround; // can only jump if on ground layer

    [Header("Hover Controls")]
    [SerializeField] private float hoverForce = 50f;
    [SerializeField] private float maxFuel = 5f;
    [SerializeField] private float fuelConsumptionRate = 1f;
    [SerializeField] private float fuelRestoreRate = 0.5f;
    [SerializeField] private GameObject hoverEffect;

    [Header("Sounds")]
    [SerializeField] private AudioSource walkAudioSource;
    [SerializeField] private AudioSource hoverAudioSource;
    [SerializeField] private AudioClip walkSound;
    [SerializeField] private AudioClip hoverSound;

    [Header("Animations")]
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer sr;
    private static readonly int Idle = Animator.StringToHash("Idle");
    private static readonly int Run = Animator.StringToHash("Run");
    private static readonly int Hover = Animator.StringToHash("Hover");
    private static readonly int DieAnim = Animator.StringToHash("Die");

    private Slider fuelSlider;

    // Private variables
    private Rigidbody2D rb;
    private float movement; // movement input
    private bool isGrounded = false;
    private bool isHovering = false;

    public float currentFuel {
        get {
            return _currentFuel;
        }
        set {
            _currentFuel = value;
            if (value < 0) {
                _currentFuel = 0;
                isHovering = false;
            }
            if (value > maxFuel) {
                _currentFuel = maxFuel;
            }

            if (fuelSlider != null) fuelSlider.value = _currentFuel;
            else fuelSlider = GameObject.FindGameObjectWithTag("FuelSlider").GetComponent<Slider>();
        }
    }
    private float _currentFuel;

    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody2D>();

        fuelSlider = GameObject.FindGameObjectWithTag("FuelSlider").GetComponent<Slider>();
        if (fuelSlider != null) fuelSlider.maxValue = maxFuel;
        currentFuel = maxFuel;
        hoverEffect.SetActive(false);

        // Assign audio clips to audio sources
        if (walkAudioSource != null) walkAudioSource.clip = walkSound;
        if (hoverAudioSource != null) hoverAudioSource.clip = hoverSound;
    }

    void GetInput(){
        movement = Input.GetAxisRaw("Horizontal");
        
        // Check if grounded by a physics check
        isGrounded = Physics2D.OverlapBoxAll(feetPos.position, groundCheckSize, 0f, whatIsGround).Length > 0;
        // Set gravity to 0 if on ground
        rb.gravityScale = isGrounded ? 0 : gravity;

        // Hovering
        if (Input.GetButtonDown("Hover") && currentFuel > 0)
        {
            isHovering = true;
            hoverEffect.SetActive(true);

            if (hoverAudioSource != null && !hoverAudioSource.isPlaying)
            {
                hoverAudioSource.Play();
            }

        }
        if(Input.GetButtonUp("Hover"))
        {
            isHovering = false;
            hoverEffect.SetActive(false);

            if (hoverAudioSource != null && hoverAudioSource.isPlaying)
            {
                hoverAudioSource.Stop();
            }
        }
    }

    int ManageAnimations(){
        
        if(isHovering) return Hover;
        return movement == 0 ? Idle : Run;
    }

    // Update is called once per frame
    void Update() {
        if(isDead) return;
        if(!view.IsMine) return;
        GetInput();
        var state = ManageAnimations();
        animator.CrossFade(state, 0, 0);

        // Flipping
        if(movement < 0) sr.flipX = true;
        if(movement > 0) sr.flipX = false;

        // Play walk sound
        if (movement != 0 && isGrounded && !walkAudioSource.isPlaying)
        {
            walkAudioSource.Play();
        }
        else if (movement == 0 || !isGrounded)
        {
            walkAudioSource.Stop();
        }

        if (Input.GetKeyDown(KeyCode.K)){
            Die();
        }
    }

    void FixedUpdate(){
        if(isDead) return;
        if(!view.IsMine) return;
        // Movement
        var vel = rb.velocity;
        // Changing the velocity directly
        vel.x = movement * speed * Time.fixedDeltaTime;

        // Hovering
        if (isHovering)
        {
            rb.gravityScale = 0; // Disable gravity while hovering
            rb.AddForce(Vector2.up * hoverForce, ForceMode2D.Force);
            currentFuel -= fuelConsumptionRate * Time.fixedDeltaTime;

        }
        else rb.gravityScale = gravity;
        if(isGrounded)
        {
            // rb.gravityScale = gravity; // Restore gravity
            if (currentFuel < maxFuel)
            {
                currentFuel += fuelRestoreRate * Time.fixedDeltaTime;
            }
        }

        rb.velocity = vel;
    }

    // Debugging
    void OnDrawGizmos(){
        // Groundcheckbox
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(feetPos.position, groundCheckSize);
    }

    public void Die(){

        WinnerManager.Instance.PlayerDied(view.Owner); // Notify WinnerManager

        Debug.Log("Player Died");

        // view.RPC("Dest", RpcTarget.All);
        animator.CrossFade(DieAnim, 0, 0);
        isDead = true;
        Destroy(weaponHolder);

        
    }

    [PunRPC]
    public void Dest(){
        PhotonNetwork.Destroy(gameObject);
    }

    [SerializeField] private GameObject pistolPrefab;
    [SerializeField] private GameObject shotgunPrefab;

    [PunRPC]
    public void GetWeapon(bool pistol){
        if(pistol) Instantiate(pistolPrefab, weaponHolder.transform.position, weaponHolder.transform.rotation, weaponHolder.transform);
        else Instantiate(shotgunPrefab, weaponHolder.transform.position, weaponHolder.transform.rotation, weaponHolder.transform);
    }

    public void AddWeapon(bool pistol){
        view.RPC("GetWeapon", RpcTarget.All, pistol);
    }
}
