using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    // Component calling section
    public Rigidbody2D rb;
    private BoxCollider2D bcoll;
    private SpriteRenderer sprite;
    private Animator anim;
    [SerializeField] private LayerMask JumpableGround;
    [SerializeField] private LayerMask WallLayer;
    [SerializeField] private Transform WallCheck;
    [SerializeField] private AudioClip dashSoundClip;
    [SerializeField] private AudioClip attackSoundClip;
    [SerializeField] private AudioClip jumpSoundClip;

    // Slide Ability Codes
    private bool isSliding;
    public bool canSlide = true;
    public float slidetime = 1f;
    [SerializeField] private float slidespeed = 14f;
    public float slidingCooldown = 5f;
    

    // Dash Ability Codes
    public bool canDash = true;
    public bool isDashing;
    public float dashingTime = 0.15f;
    [SerializeField] private float dashingPower = 56f;
    public float dashingCooldown = 1f;


    // Wall Jump Ability Codes
    int Jumpcount = 0;
    private float walljumpingdirection;
    [SerializeField] private Vector2 wallJumpingPower = new Vector2(8f, 16f);
    [SerializeField] private float walljumpingTime = 2f;
    private float walljumpingCounter;
    private float walljumpingDuration;
    private bool isWallJumping;
   

    // Wallslide Ability Codes
    private bool isWallSliding;
    [SerializeField] private float wallslidespeed = 2f;


    // Player Horizantal and vertical Move Codes
    public float direcX;
    [SerializeField] private float hýz = 7f;
    [SerializeField] private float zýpla = 14f;
    public bool facingright = true;
    private int DoubleJump;


    //Attack Ability Codes
    private GameObject originalGameObject;
    private GameObject swordCollider;
    private BoxCollider2D sword;
    public bool Canattack = true;
    public float attackcooldown = 3f;

    public PlayerHealth ph;

    private enum AnimState {idle, run, jump, fall, wallslide, doublejump}
    AnimState state;


    private void Start()
    {
        Time.timeScale = 1f;
        originalGameObject = GameObject.Find("Player");
        swordCollider = originalGameObject.transform.GetChild(1).gameObject;
        sword = swordCollider.GetComponent<BoxCollider2D>();
        sword.enabled = false;

        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        bcoll = GetComponent<BoxCollider2D>();
    }

    
    private void Update()
    {
        if (PauseMenu.isPaused)
        {
            return;
        }
        if (ph.PlayerAlive == false) 
        {
            return;
        }
        if (isDashing)
        {
            return;
        }
        if (isSliding)
        {
            return;
        }

        // Attack Ability
        if (Input.GetKeyDown(KeyCode.Q) && Canattack && GroundCheck())
        {
            StartCoroutine(Attack());
            SoundFXManager.Instance.PlaySoundFXClip(attackSoundClip, transform, 1f);
        }

        // Horizantal Move Code
        if (!isWallJumping)
        {
            direcX = Input.GetAxisRaw("Horizontal");
            rb.velocity = new Vector2(direcX * hýz, rb.velocity.y);
        }

        // Flip Code
        if (direcX > 0f && !facingright)
        {
            Flip();
        }
        else if (direcX < 0f && facingright)
        {
            Flip();
        }

        // Jumping Code
        Jumpcount = Mathf.Clamp(Jumpcount, 0, 3);
        if (GroundCheck())
        {
            DoubleJump = 2;
        }
        if (Input.GetButtonDown("Jump"))
        {
            if (GroundCheck() || DoubleJump > 1)
            {
                rb.velocity = new Vector2(rb.velocity.x, zýpla);
                DoubleJump--;
                SoundFXManager.Instance.PlaySoundFXClip(jumpSoundClip, transform, 1f);
            } 
        }

        // Dash Code
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash && !isDashing && direcX != 0 && !IsWalled())
        {
            StartCoroutine(Dash());
            SoundFXManager.Instance.PlaySoundFXClip(dashSoundClip, transform, 1f);
        }
        
        if (Input.GetKeyDown(KeyCode.LeftControl) && GroundCheck() && canSlide && !isSliding && direcX != 0 && !IsWalled())
        {
            StartCoroutine(Slide());
        }
   
        
        Animations();
        Wallslide();
        WallJump();
    }


    private void WallJump() 
    {
        if (IsWalled())
        {
            Jumpcount++;         // integer for double jump
        }
        else
        {
            Jumpcount = 0;         // integer for double jump
        }
        if (isWallSliding)
        {
            isWallJumping = false;   
            walljumpingdirection = -transform.localScale.x;
            walljumpingCounter = walljumpingTime;
            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            walljumpingCounter -= Time.deltaTime;
        }
        if (Input.GetButtonDown("Jump") && walljumpingCounter > 0f && Jumpcount < 2)
        {
            isWallJumping = true;
            rb.velocity = new Vector2(walljumpingdirection * wallJumpingPower.x, wallJumpingPower.y);
            walljumpingCounter = 0f;

            if (transform.localScale.x != walljumpingdirection)
            {
                facingright = !facingright;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }

            Invoke(nameof(StopWallJumping), walljumpingDuration);
        }
    }
    private void StopWallJumping() 
    {
        isWallJumping = false;
    }

    private void Wallslide()
    {
        if (IsWalled() && !GroundCheck() && direcX != 0f)
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallslidespeed, float.MaxValue));
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void Animations()
    {
        // Run & Idle Animations
        if (GroundCheck() && direcX > 0f)
        {
            state = AnimState.run;

        }
        else if (GroundCheck() && direcX < 0f)
        {
            state = AnimState.run;
        }
        else if (GroundCheck() && direcX == 0f)
        {
            state = AnimState.idle;
        }

        // Vertical Move Animations
        if (rb.velocity.y > .01f && DoubleJump == 2)
        {
            state = AnimState.jump;
        }
        else if (rb.velocity.y > .01f && DoubleJump == 1)
        {
            state = AnimState.doublejump;
        }
        else if (rb.velocity.y < -.01f)
        {
            state = AnimState.fall;
        }

        // Wallslide Animations
        if (isWallSliding)
        {
            state = AnimState.wallslide;
        }

        anim.SetInteger("state", (int)state);
    }

    private void Flip()
    {
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        facingright = !facingright;
    }

    private IEnumerator Dash()
    { 
        canDash = false;    // Dash cooldown = dashingcooldown
        isDashing = true;       // Dashing at the moment is on
        anim.SetTrigger("dash");        // animation
        float originalGravity = rb.gravityScale;         // Gravity setting to 0 for if we dash at air
        rb.gravityScale = 0f;                           // Gravity setting to 0 for if we dash at air
        rb.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);  // applying the dash speed(power)
        yield return new WaitForSeconds(dashingTime);     // waiting until dash completes
        rb.gravityScale = originalGravity;         //Gravity setting back to original
        isDashing = false;                 // Dashing at the moment is off
        yield return new WaitForSeconds(dashingCooldown);         // Dash cooldown
        canDash = true;          // Dash cooldown = 0
    }

    private IEnumerator Slide()
    {
        canSlide = false;
        isSliding = true;                 // Sliding at the moment
        bcoll.offset = new Vector2(-0.0682888f, -0.3575193f);
        bcoll.size = new Vector2(1.637926f, 0.6921828f);
        anim.SetBool("slide", true);      // Animation is ON     
        rb.velocity = new Vector2(transform.localScale.x * slidespeed, rb.velocity.y);    // applying slide slow
        yield return new WaitForSeconds(slidetime);
        anim.SetBool("slide", false);
        isSliding = false;
        bcoll.offset = new Vector2(0.008831739f, -0.00955677f);
        bcoll.size = new Vector2(0.598455f, 1.306816f);
        yield return new WaitForSeconds(slidingCooldown);
        canSlide = true;       
    }

    private IEnumerator Attack()
    {
        Canattack = false;
        anim.SetTrigger("attack");
        yield return new WaitForSeconds(attackcooldown);
        Canattack = true;
       
    }

    private bool GroundCheck()
    {
        return Physics2D.BoxCast(bcoll.bounds.center, bcoll.bounds.size, 0f, Vector2.down, .1f, JumpableGround);     
    }

    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(WallCheck.position, 0.2f, WallLayer);     // Checking towards for nearby walls with wallcheck object in game
    }

    void activateCollider()
    {
        sword.enabled = true;
    }

    void deactivateCollider()
    {
        sword.enabled = false;
    }
}

