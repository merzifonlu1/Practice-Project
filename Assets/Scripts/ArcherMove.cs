using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ArcherMove : MonoBehaviour
{
    [SerializeField] private Transform player;
    private Animator anim;
    private Rigidbody2D rb;
    private bool FacingRight;
    private bool runRangebool;
    private bool attackRangebool;
    private bool isEscaping;
    public bool arrived;
    [SerializeField] private Transform escapepoint;
    [SerializeField] private float attackRange = 10f;
    [SerializeField] private float runRange = 3f;
    [SerializeField] private float runspeed = 5f;

    void Start()
    {        
        rb = GetComponent<Rigidbody2D>();
        FacingRight = false;
        anim = GetComponent<Animator>();
        runRangebool = false;
        attackRangebool = false;
        arrived = false;
        isEscaping = false;
    }


    void Update()
    {
        if (Vector2.Distance(player.position, rb.position) <= runRange)
        {
            runRangebool = true;
        }
        else
        {
            runRangebool = false;
        }
        if (Vector2.Distance(player.position, rb.position) <= attackRange)
        {
            attackRangebool = true;
        }
        else
        {
            attackRangebool = false;
        }

    }

    private void FixedUpdate()
    {
        if (runRangebool && !arrived)
        {
            Escape();
        }

        if ((!runRangebool && attackRangebool) || (arrived && !isEscaping))
        {
            Attack();
        }
        else
        {
            anim.SetBool("shoot", false);
        }
    }

    private void flip()
    {
        transform.Rotate(0f, 180f, 0f);
        FacingRight = !FacingRight;
    }

    private void Attack()
    {
        anim.SetBool("running", false);
        LookAtPlayer();
        anim.SetBool("shoot", true);
    }

    private void Escape()
    {
        isEscaping = true;
        anim.SetBool("shoot", false);

        if (!FacingRight)
        {
            flip();
        }

        anim.SetBool("running", true);
        Vector2 target = new Vector2(escapepoint.position.x, rb.position.y);
        Vector2 newPos = Vector2.MoveTowards(rb.position, target, runspeed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);

        if (Vector2.Distance(newPos, target) < 0.1f)
        {

            arrived = true;
            isEscaping = false;
        }

    }

    public void LookAtPlayer()
    {
        Vector3 flipped = transform.localScale;
        flipped.z = -1f;
        if (transform.position.x > player.position.x && FacingRight)           
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            FacingRight = false;
        }
        else if (transform.position.x < player.position.x && !FacingRight)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            FacingRight = true;
        }
    }
}
