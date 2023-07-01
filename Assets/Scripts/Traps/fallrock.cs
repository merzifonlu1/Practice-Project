using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fallrock : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private Rigidbody2D rb;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            anim.SetTrigger("fall");
        }
    }

    private void Fall()
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }
}
