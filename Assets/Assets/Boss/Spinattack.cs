using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinattack : MonoBehaviour
{
    public int damage = 42;

    public BossMovement Boss;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerHealth player = collision.GetComponent<PlayerHealth>();
        if (player != null && player.PlayerAlive)
        {
            player.TakeDamage(damage);
        }
    }
}
