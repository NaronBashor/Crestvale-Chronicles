using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : MonoBehaviour
{
    private Transform parent;

    public Sprite[] stoneSprites = new Sprite[3];

    public float shakeDuration = 0.5f;  // Total time to shake the stone
    public float shakeMagnitude = 0.1f; // The magnitude of the shake
    public float elapsed = 0f;
    private Vector3 originalPosition;

    public GameObject stonesPrefab;

    private int stoneHealth = 10;
    public int stonesToSpawn;

    private void Start()
    {
        this.GetComponent<SpriteRenderer>().sprite = stoneSprites[Random.Range(0, 2)];

        originalPosition = transform.position;

        parent = GameObject.Find("Stones").GetComponent<Transform>();
        this.transform.SetParent(parent);

        // Find ProximityManager if it hasn't been assigned
        if (ProximityManager.Instance == null) {
            Debug.Log("Proximity manager not found.");
        }

        if (ProximityManager.Instance != null) {
            ProximityManager.Instance.objectsToManage.Add(gameObject);
        }
    }

    [ContextMenu("Shake")]
    // Method to trigger the shake effect when the stone is hit
    public void OnHit()
    {
        TakeDamage(1);
        if (stoneHealth <= 0) {
            SpawnInStones();
        } else {
            StartCoroutine(ShakeStone());
        }
    }

    // Method to handle stone damage
    void TakeDamage(int damage)
    {
        stoneHealth -= damage;
    }

    // Coroutine to shake the stone
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
            Instantiate(stonesPrefab, transform.position, Quaternion.identity);
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
        stonesToSpawn = Random.Range(3, 5);
        return stonesToSpawn;
    }
}
