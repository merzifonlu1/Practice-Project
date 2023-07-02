using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTeleport : MonoBehaviour
{
    private GameObject currentTeleporter;
    private float teleportcooldown = 0.5f;
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
        transform.position = currentTeleporter.GetComponent<Teleporter>().GetDestination().position;
        yield return new WaitForSeconds(teleportcooldown);
        CanTeleport = true;
    }
}
