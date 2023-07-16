using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMovement : MonoBehaviour
{
    [SerializeField] private Transform player;
    private Animator anim;
    private Rigidbody2D rb;
    private bool FacingRight;
    private bool Moving;
    private bool walking;
    private bool Attacking1;

    [SerializeField] private float attackrange = 5f;
    [SerializeField] private float runspeed = 5f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        FacingRight = false;
        anim = GetComponent<Animator>();
        Moving = false;
        walking = false;
        Attacking1 = false;
    }

    
    void Update()
    {
        if (Vector2.Distance(player.position, rb.position) > attackrange)
        {
            Moving = true;
        }
        else
        {
            Moving = false;
            walking = false;
        }
    }

    private void FixedUpdate()
    {
        if (Attacking1)
        {
            return;
        }
        LookAtPlayer();

        if (Moving)
        {
            Walk();
        }
        else
        {
            StartCoroutine(Attack1());
        }
    }

    private void Walk()
    {
        walking = true;
        anim.SetBool("Walking", true);
        Vector2 target = new Vector2(player.position.x, rb.position.y);
        Vector2 newPos = Vector2.MoveTowards(rb.position, target, runspeed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);
    }

    private IEnumerator Attack1()
    {
        Attacking1 = true;
        anim.SetBool("Walking", false);        
        anim.SetTrigger("Attack1");
        yield return new WaitForSeconds(0.8f);
        Attacking1 = false;
    }

    private void flip()
    {
        transform.Rotate(0f, 180f, 0f);
        FacingRight = !FacingRight;
    }

    public void LookAtPlayer()
    {
        Vector3 flipped = transform.localScale;
        flipped.z = -1f;
        if (transform.position.x > player.position.x && FacingRight && !Attacking1)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            FacingRight = false;
        }
        else if (transform.position.x < player.position.x && !FacingRight && !Attacking1)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            FacingRight = true;
        }
    }

}
