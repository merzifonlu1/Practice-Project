using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    public int maxHealth = 100;
    public int currentHealth;
    public bool PlayerAlive = true;

    public HealthBar healthbar;

    void Start()
    {
        currentHealth = maxHealth;
        healthbar.MaxHealth(maxHealth);


        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!PlayerAlive)
        {
            return;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            healthbar.Sethealth(currentHealth);
            Die();
        }

        if (PlayerAlive)
        {
            anim.SetTrigger("takedamage");
            healthbar.Sethealth(currentHealth);
        }

    }

    private void Die()
    {
        anim.SetTrigger("death");
        rb.bodyType = RigidbodyType2D.Static;
        PlayerAlive = false;        
    }

    public void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
