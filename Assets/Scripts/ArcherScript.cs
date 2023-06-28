using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherScript : MonoBehaviour
{
    private Animator anim;
    public bool isDead = false;

    void Start()
    {
        anim = GetComponent<Animator>();
    }


    private void Die()
    {
        anim.SetTrigger("death");
        Invoke(nameof(Delete), 1f);
        isDead = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("sword"))
        {
            Die();
        }
    }

    private void Delete()
    {
        Destroy(gameObject);
    }

}
