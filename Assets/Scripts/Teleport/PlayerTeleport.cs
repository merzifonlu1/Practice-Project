using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTeleport : MonoBehaviour
{
    private GameObject currentTeleporter;
    [SerializeField] private float teleportcooldown = 0.1f;
    private bool CanTeleport = true;
    void Update()
    {
      
        if (currentTeleporter != null && CanTeleport)
        {
              StartCoroutine(Teleport());
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Teleporter"))
        {
            currentTeleporter = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Teleporter"))
        {
            if (collision.gameObject == currentTeleporter)
            {
                currentTeleporter = null;
            }
        }
    }

    private IEnumerator Teleport()
    {
        CanTeleport = false;
        Vector3 destination = currentTeleporter.GetComponent<Teleporter>().GetDestination().position;
        Vector3 playerPosition = new Vector3(destination.x, transform.position.y, transform.position.z);
        transform.position = playerPosition;
        yield return new WaitForSeconds(teleportcooldown);
        CanTeleport = true;
    }
}
