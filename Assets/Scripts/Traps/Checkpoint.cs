using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private Animator anim;
    public int scenenumber;
    private void Start()
    {
        scenenumber = 0;
        anim = GetComponent<Animator>();


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            anim.SetTrigger("phase1");
            scenenumber++;
        }
    }

    public void checkpointactiveanim()
    {
        anim.SetTrigger("phase2");
    }
}
