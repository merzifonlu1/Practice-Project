using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    public int maxHealth = 100;
    public int currentHealth;
    public bool PlayerAlive = true;
    public TextMeshProUGUI damagetext;
    [SerializeField] private Transform Player;

    public HealthBar healthbar;

    public Checkpoint checkpoint;

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
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetLevel();
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        damagetext.enabled = true;
        damagetext.text = damage.ToString();

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

        Invoke(nameof(textfade), 0.5f);
    }

    private void Die()
    {
        anim.SetTrigger("death");
        rb.bodyType = RigidbodyType2D.Static;
        PlayerAlive = false;
    }

    public void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().rootCount + checkpoint.scenenumber);
    }

    public void textfade()
    {
        damagetext.enabled = false;
    }
}
