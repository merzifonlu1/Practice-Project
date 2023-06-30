using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crossbpw : MonoBehaviour
{
    [SerializeField] private Transform player;
    private Animator anim;
    private Rigidbody2D rb;
    [SerializeField] private float attackRange = 10f;
    private float rotationSpeed = 5f;
    public Transform basePosition;
    public Transform firepoint;
    public GameObject ArrowCrosPrefab;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        if (Vector2.Distance(player.position, rb.position) <= attackRange)
        {
            anim.SetBool("shoot", true);
            Vector3 targetDir = player.position - transform.position;
            targetDir.z = 0;
            float angle = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg;
            Quaternion desiredRotation = Quaternion.Euler(0, 0, angle + 90);
            transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            anim.SetBool("shoot", false);
            ReturnToBasePosition();
        }

    }
    void ReturnToBasePosition()
    {
        anim.SetBool("shoot", false);
        Vector3 targetDir = basePosition.position - transform.position;
        targetDir.z = 0;
        float angle = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg;
        Quaternion desiredRotation = Quaternion.Euler(0, 0, angle);
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, rotationSpeed * Time.deltaTime);
    }

    public void shoot()
    {
        Instantiate(ArrowCrosPrefab, firepoint.position, firepoint.rotation);
    }
}
