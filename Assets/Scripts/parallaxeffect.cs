using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class parallaxeffect : MonoBehaviour
{
    private float lenght;
    private float startpos;
    public GameObject kamera;
    public float parallax;

    void Start()
    {
        startpos = transform.position.x;
        lenght = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void FixedUpdate()
    {
        float temp = (kamera.transform.position.x * (1 - parallax));
        float distance = (kamera.transform.position.x * parallax);

        transform.position = new Vector3(startpos + distance, transform.position.y, transform.position.z);

        if (temp > startpos + lenght)
        {
            startpos += lenght;
        }
        else if (temp < startpos - lenght)
        {
            startpos -= lenght;
        }
    }
}
