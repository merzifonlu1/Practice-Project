using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed = 20f;
    public int damage = 10;


    void Start()
    {
        rb.velocity = -transform.right * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerHealth player = collision.GetComponent<PlayerHealth>();       
        if (player != null && player.PlayerAlive)
        {
            player.TakeDamage(damage);
        }
        Destroy(gameObject);
    }
}
