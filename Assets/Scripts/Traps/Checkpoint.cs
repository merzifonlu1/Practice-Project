using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private Animator anim;
    public PlayerHealth ph;

    public Transform checkpoint;

    private void Start()
    {
        anim = GetComponent<Animator>();

        ph = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            anim.SetTrigger("phase1");
            ph.UpdateSpawnPoint(checkpoint.position);
        }
    }

    public void checkpointactiveanim()
    {
        anim.SetTrigger("phase2");
    }
}
