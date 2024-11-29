using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrePrefab : MonoBehaviour
{
    public Ores oreSO;

    SpriteRenderer sprite;

    private Transform parent;

    public GameObject oreCollectible;

    private int oresToSpawn;

    private float shakeDuration;  // Total time to shake the stone
    private float shakeMagnitude; // The magnitude of the shake
    private float elapsed = 0f;

    private Vector3 originalPosition;

    private int health;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();

        parent = GameObject.Find("Ores").GetComponent<Transform>();
        this.transform.SetParent(parent);

        originalPosition = transform.position;

        shakeDuration = 0.25f;
        shakeMagnitude = 0.05f;

        sprite.sprite = oreSO.oreSprite;
        health = oreSO.health;
    }

    public void OnHit()
    {
        TakeDamage(1);
        if (health <= 0) {
            SpawnInStones();
        } else {
            StartCoroutine(ShakeStone());
        }
    }

    void TakeDamage(int damage)
    {
        health -= damage;
    }

    private IEnumerator ShakeStone()
    {
        elapsed = 0f;
        while (elapsed < shakeDuration) {
            // Generate random offset for the shake
            float offsetX = Random.Range(-shakeMagnitude, shakeMagnitude);
            float offsetY = Random.Range(-shakeMagnitude, shakeMagnitude);

            // Apply the offset to the stone's position
            transform.position = new Vector3(originalPosition.x + offsetX, originalPosition.y + offsetY, originalPosition.z);

            // Increase elapsed time
            elapsed += Time.deltaTime;

            // Wait for the next frame before continuing the shake
            yield return null;
        }

        // After the shake, reset the stone to its original position
        transform.position = originalPosition;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the object hitting the stone is the stone axe
        if (collision.CompareTag("Pickaxe"))  // Ensure the axe has the tag "Axe"
        {
            OnHit();
        }
    }

    private void SpawnInStones()
    {
        for (int i = 0; i < RandomNumberGenerator(); i++) {
            GameObject ore = Instantiate(oreCollectible, transform.position, Quaternion.identity);
            ore.GetComponent<SpriteRenderer>().sprite = oreSO.collectibleSprite;
        }
        // Get the index of the current object in the list
        int index = ProximityManager.Instance.objectsToManage.IndexOf(gameObject);

        // If the object is found in the list, remove it by index
        if (index >= 0) {
            ProximityManager.Instance.objectsToManage.RemoveAt(index);
        }

        // Destroy the current GameObject
        Destroy(gameObject);
    }

    private int RandomNumberGenerator()
    {
        oresToSpawn = Random.Range(oreSO.minDropAmount, oreSO.maxDropAmount + 1);
        return oresToSpawn;
    }
}
