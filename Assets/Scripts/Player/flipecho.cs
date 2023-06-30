using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flipecho : MonoBehaviour
{
    public SpriteRenderer sr;
    public PlayerMovement player;
   

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.direcX != 0)
        {
            if (player.facingright)
            {
                sr.flipX = false;
            }
            else
            {
                sr.flipX = true;
            }
        }
    }
}