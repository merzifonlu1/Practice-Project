using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crossbpw : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private AudioClip shootcrossbow;
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
            bool hasObstacle = CheckForObstacles();

            if (!hasObstacle)
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

    bool CheckForObstacles()
    {
        Vector2 direction = player.position - transform.position;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, Vector2.Distance(player.position, rb.position));
        if (hit.collider != null && hit.collider.transform != player)
        {
            return true;
        }
        return false;
    }

    public void shoot()
    {
        Instantiate(ArrowCrosPrefab, firepoint.position, firepoint.rotation);
        SoundFXManager.Instance.PlaySoundFXClip(shootcrossbow, transform, 1f);
    }
}
