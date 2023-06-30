using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ec : MonoBehaviour
{
    private float timeBtwSpawns;
    public float startTimeBtwSpawns;


    public GameObject echo;
    public PlayerMovement player;


    // Update is called once per frame
    void Update()
    {
        if (player.isDashing)
        {

            if (timeBtwSpawns <= 0)
            {
                GameObject instance = (GameObject)Instantiate(echo, transform.position, Quaternion.identity);
                Destroy(instance, player.dashingTime);
                timeBtwSpawns = startTimeBtwSpawns;
            }
            else
            {
                timeBtwSpawns -= Time.deltaTime;
            }
        }
    }
}

