using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class xrayrotate : MonoBehaviour
{
    [SerializeField] private Transform player;
    private float rotationSpeed = 15f;

    void Update()
    {
        Vector3 targetDir = player.position - transform.position;
        targetDir.z = 0;
        float angle = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg;
        Quaternion desiredRotation = Quaternion.Euler(0, 0, angle + 180);
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, rotationSpeed * Time.deltaTime);
    }


}
