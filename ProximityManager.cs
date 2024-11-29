using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProximityManager : MonoBehaviour
{
    public static ProximityManager Instance;

    public Transform player;  // Reference to the player object
    public float activationDistance = 10f;  // Distance to activate objects
    public float checkInterval = 1f;  // How often to check for proximity

    public List<GameObject> objectsToManage = new List<GameObject>();

    private void Awake()
    {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);  // Ensure there's only one instance
        }
    }

    private void Start()
    {
        // Start checking the proximity at regular intervals
        StartCoroutine(CheckProximity());
    }

    private IEnumerator CheckProximity()
    {
        while (true) {
            foreach (GameObject obj in objectsToManage) {
                // Calculate the distance between the player and the object
                float distance = Vector3.Distance(player.position, obj.transform.position);

                // Activate or deactivate the object based on the player's distance
                if (distance <= activationDistance && !obj.activeInHierarchy) {
                    obj.SetActive(true);  // Activate the object if within range and inactive
                } else if (distance > activationDistance && obj.activeInHierarchy) {
                    obj.SetActive(false);  // Deactivate the object if out of range and active
                }
            }

            // Wait for the next check interval
            yield return new WaitForSeconds(checkInterval);
        }
    }

    [ContextMenu("Clear List")]
    private void OnApplicationQuit()
    {
        objectsToManage.Clear();
    }
}
