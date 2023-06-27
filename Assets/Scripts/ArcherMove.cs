using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherMove : MonoBehaviour
{
    [SerializeField] private Transform player;
    private Animator anim;
    private Rigidbody2D rb;
    private bool FacingRight = false;
    [SerializeField] private Transform escapepoint;
    [SerializeField] private float attackRange = 10f;
    [SerializeField] private float runRange = 3f;
    [SerializeField] private float runspeed = 5f;

    void Start()
    {        
        rb = GetComponent<Rigidbody2D>();
        FacingRight = false;
        anim = GetComponent<Animator>();
    }


    void Update()
    {
        if (Vector2.Distance(player.position, rb.position) <= attackRange && Vector2.Distance(player.position, rb.position) > runRange)
        {
            anim.SetBool("running", false);
            Attack();
        }

        if (Vector2.Distance(player.position, rb.position) <= runRange)
        {
            Escape();
        }

    }

    private void flip()
    {
        transform.Rotate(0f, 180f, 0f);
        FacingRight = !FacingRight;
    }

    private void Attack()
    {
        anim.SetBool("shoot", true);
    }

    private void Escape()
    {
        anim.SetBool("shoot", false);

        if (!FacingRight)
        {
            flip();
        }

        anim.SetBool("running", true);
        Vector2 target = new Vector2(escapepoint.position.x, rb.position.y);
        Vector2 newPos = Vector2.MoveTowards(rb.position, target, runspeed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);

    }
}
