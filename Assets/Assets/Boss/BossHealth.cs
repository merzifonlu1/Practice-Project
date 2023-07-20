using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BossHealth : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private BoxCollider2D bcoll;
    private SpriteRenderer SR;
    public int bossmaxHealth = 214;
    public int bosscurrentHealth;
    public bool BossAlive = true;
    [SerializeField] private Transform Boss;
    [SerializeField] private Color newColor = Color.red;


    void Start()
    {
        bosscurrentHealth = bossmaxHealth;

        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        bcoll = GetComponent<BoxCollider2D>();
        SR = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (!BossAlive)
        {
            return;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("sword"))
        {
            BossTakeDamage(18);
        }
    }

    public void BossTakeDamage(int damage)
    {
        bosscurrentHealth -= damage;

        if (bosscurrentHealth <= 0)
        {
            StartCoroutine(ColorShift());
            Die();
        }

        if (BossAlive)
        {
            StartCoroutine(ColorShift());
        }
    }

    private IEnumerator ColorShift()
    {
        SR.color = newColor;
        yield return new WaitForSeconds(0.3f);
        SR.color = Color.white;
    }

    private void Die()
    {
        anim.SetTrigger("death");
        bcoll.enabled = false;
        rb.bodyType = RigidbodyType2D.Static;
        BossAlive = false;
    }
}
