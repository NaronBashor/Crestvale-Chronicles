using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitHomeFarm : MonoBehaviour
{
    public Vector2 townPosition;

    public GameObject townTileMap;
    public GameObject homeTileMap;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null) {
            if (collision.CompareTag("Player")) {
                homeTileMap.SetActive(false);
                townTileMap.SetActive(true);
                collision.GetComponent<Transform>().transform.position = townPosition;
            }
        }
    }
}
