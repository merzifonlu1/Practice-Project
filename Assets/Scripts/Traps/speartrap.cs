using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class speartrap : MonoBehaviour
{
    private Animator anim;
    private bool intrap = false;
    public PlayerHealth ph;
    public int damageAmount = 10;  // Amount of damage to inflict on collision

    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the colliding object is the player
        if (other.CompareTag("Player"))
        {
            intrap = true;
        }

    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            intrap = false;
        }

    }
    public void givedamage()
    {
        if (intrap)
        {
            ph.TakeDamage(damageAmount);
        }
    }
}
