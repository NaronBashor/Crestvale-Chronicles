using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitRoom : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null) {
            if (collision.CompareTag("Player")) {
                collision.GetComponent<PlayerController>().canExitRoom = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision != null) {
            if (collision.CompareTag("Player")) {
                collision.GetComponent<PlayerController>().canExitRoom = false;
            }
        }
    }
}
