using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    private BoxCollider2D bcoll;
    private SpriteRenderer sprite;
    private Animator anim;

    public float direcX;

    private bool canDash = true;
    public bool isDashing;
    [SerializeField] private float dashingPower = 24f;
    public float dashingTime = 0.2f;
    [SerializeField] private float dashingCooldown = 1f;

    [SerializeField] private LayerMask JumpableGround;
    [SerializeField] private LayerMask WallLayer;
    [SerializeField] private Transform WallCheck;

    int Jumpcount = 0;
    private float walljumpingdirection;
    [SerializeField] private float hýz = 7f;
    [SerializeField] private float zýpla = 14f;
    [SerializeField] private float wallslidespeed = 2f;
    [SerializeField] private float walljumpingTime = 2f;
    private float walljumpingCounter;
    private float walljumpingDuration;
    [SerializeField] private Vector2 wallJumpingPower = new Vector2(8f, 16f);

    public bool facingright = true;
    private bool isWallSliding;
    private bool isWallJumping;

    private int DoubleJump;
    //public bool canMove;


    [SerializeField] private AudioSource jumpSoundEffect;

    private enum AnimState {idle, run, jump, fall, wallslide, doublejump}
    AnimState state;

    private void Start()
    {
        //canMove = true;
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        bcoll = GetComponent<BoxCollider2D>();
    }

    

    private void Update()
    {
        Jumpcount = Mathf.Clamp(Jumpcount, 0, 3);

        if (isDashing)
        {
            return;
        }

        if (!isWallJumping)
        {
            direcX = Input.GetAxisRaw("Horizontal");
            rb.velocity = new Vector2(direcX * hýz, rb.velocity.y);
        }
        if (direcX > 0f && !facingright)
        {
            Flip();
        }
        else if (direcX < 0f && facingright)
        {
            Flip();
        }

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
            } 
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash && direcX != 0)
        {
            anim.SetTrigger("dash");
            StartCoroutine(Dash());
        }

        Animations();
        Wallslide();
        WallJump();

    }

    private void WallJump()
    {
        if (IsWalled())
        {
            Jumpcount++;
        }
        else
        {
            Jumpcount = 0;
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
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        yield return new WaitForSeconds(dashingTime);
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }

    private bool GroundCheck()
    {
        return Physics2D.BoxCast(bcoll.bounds.center, bcoll.bounds.size, 0f, Vector2.down, .1f, JumpableGround);
    }

    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(WallCheck.position, 0.2f, WallLayer);
    }

}

