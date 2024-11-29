using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsHomeChecker : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null) {
            if (collision.CompareTag("Player")) {
                collision.GetComponent<PlayerController>().isHomeFarm = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision != null) {
            if (collision.CompareTag("Player")) {
                collision.GetComponent<PlayerController>().isHomeFarm = false;
            }
        }
    }
}
