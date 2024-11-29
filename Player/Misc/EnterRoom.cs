using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterRoom : MonoBehaviour
{
    [SerializeField] private Vector3 camPosition;
    public Camera cam;
    public GameObject player;

    public int roomIndex;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null) {
            if (collision.CompareTag("Player")) {
                collision.GetComponent<PlayerController>().canEnterRoom = true;
                collision.GetComponent<PlayerController>().buildingEntrancePosition = this.transform.position;
                collision.GetComponent<PlayerController>().camPosition = new Vector3(camPosition.x, 288.51f, -10f);
                collision.GetComponent<PlayerController>().roomIndex = roomIndex;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision != null) {
            if (collision.CompareTag("Player")) {
                collision.GetComponent<PlayerController>().canEnterRoom = false;
            }
        }
    }
}
