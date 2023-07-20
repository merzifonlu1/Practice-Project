using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class xraybullet : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed = 40f;
    public int damage = 10;
    public GameObject gameobject;

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
            Destroy(gameobject);
        }
        if (collision.gameObject.CompareTag("sword"))
        {
            Destroy(gameobject);
        }
        if (collision.gameObject.CompareTag("zemin"))
        {
            Destroy(gameobject);
        }
    }
}
