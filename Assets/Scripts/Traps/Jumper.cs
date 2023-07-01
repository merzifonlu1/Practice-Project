using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Jumper : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private float launchpower = 19f;
    [SerializeField] private Rigidbody2D rb;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            anim.SetBool("launch", true);
        }
    }
    private void launch()
    {
        Vector3 launchDirection = Vector3.up;
        rb.GetComponent<Rigidbody2D>().AddForce(Vector2.up * launchpower, ForceMode2D.Impulse);
    }
    private void launchfalse()
    {
        anim.SetBool("launch", false);
    }
}
