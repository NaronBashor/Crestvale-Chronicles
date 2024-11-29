using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterHomeFarm : MonoBehaviour
{
    public Vector2 homeFarmPosition;

    public GameObject townTileMap;
    public GameObject homeTileMap;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null) {
            if (collision.CompareTag("Player")) {
                homeTileMap.SetActive(true);
                townTileMap.SetActive(false);
                collision.GetComponent<Transform>().transform.position = homeFarmPosition;
            }
        }
    }
}
