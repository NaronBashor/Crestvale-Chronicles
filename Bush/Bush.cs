using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bush : MonoBehaviour
{
    public float shakeDuration = 0.5f;
    public float shakeMagnitude = 0.1f;
    public float elapsed = 0f;
    private Vector3 originalPosition;

    private Transform parent;

    public GameObject flaxPrefab;

    private int bushHealth;
    public int bushesToSpawn;

    private void Start()
    {
        originalPosition = transform.position;
        bushHealth = 1;
        parent = GameObject.Find("Bushes").GetComponent<Transform>();
        this.transform.SetParent(parent);

        // Find ProximityManager if it hasn't been assigned
        if (ProximityManager.Instance == null) {
            Debug.Log("Proximity manager not found.");
        }

        if (ProximityManager.Instance != null) {
            ProximityManager.Instance.objectsToManage.Add(gameObject);
        }
    }

    // Method to trigger the shake effect when the bush is hit
    public void OnHit()
    {
        TakeDamage(1);
        if (bushHealth <= 0) {
            SpawnInBushes();
        }
    }

    // Method to handle bush damage
    void TakeDamage(int damage)
    {
        bushHealth -= damage;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the object hitting the bush is the bush axe
        if (collision.CompareTag("Scythe"))  // Ensure the axe has the tag "Axe"
        {
            OnHit();
        }
        if (collision.CompareTag("Feet")) {
            StartCoroutine(ShakeBush());
        }
    }

    // Coroutine to shake the stone
    private IEnumerator ShakeBush()
    {
        elapsed = 0f;
        while (elapsed < shakeDuration) {
            // Generate random offset for the shake
            float offsetX = Random.Range(-shakeMagnitude, shakeMagnitude);
            float offsetY = Random.Range(-shakeMagnitude, shakeMagnitude);

            // Apply the offset to the bushes position
            transform.position = new Vector3(originalPosition.x + offsetX, originalPosition.y + offsetY, originalPosition.z);

            // Increase elapsed time
            elapsed += Time.deltaTime;

            // Wait for the next frame before continuing the shake
            yield return null;
        }

        // After the shake, reset the bush to its original position
        transform.position = originalPosition;
    }

    private void SpawnInBushes()
    {
        for (int i = 0; i < RandomNumberGenerator(); i++) {
            Instantiate(flaxPrefab, transform.position, Quaternion.identity);
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
        bushesToSpawn = Random.Range(0, 3);
        return bushesToSpawn;
    }
}
