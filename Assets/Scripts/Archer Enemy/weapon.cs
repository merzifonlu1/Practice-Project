using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weapon : MonoBehaviour
{

    public Transform firepoint;
    public GameObject arrowPrefab;



    public void shoot()
    {
        Instantiate(arrowPrefab, firepoint.position, firepoint.rotation);
    }
}
