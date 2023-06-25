using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamControls : MonoBehaviour
{
    [SerializeField] private GameObject waypoint;
    [SerializeField] float speed = 2f;
    
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, waypoint.transform.position, Time.deltaTime * speed);
   
    }
}
