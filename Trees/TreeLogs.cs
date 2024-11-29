using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeLogs : MonoBehaviour
{
    public Item logItem;

    private InventoryManager invManager; // Reference to InventoryManager to add the log to the player's inventory
    private Transform player; // Reference to the player

    private Transform parent;
    private Rigidbody2D rb;

    [SerializeField] private Sprite[] logTypeSprites = new Sprite[2];
    public float bounceForce = 2f;  // The force applied to make the logs bounce
    public float bounceTorque = 10f; // The torque applied for rotation (optional)

    // Movement towards player
    public float playerDetectRange = 5f; // Range within which the logs detect and move towards the player
    public float moveSpeed = 5f;         // Speed at which the logs move towards the player
    public float collectDistance = 0.5f; // Distance from the player at which the logs will be collected/destroyed

    public bool canCollect = false;

    private void Start()
    {
        invManager = GameObject.Find("Inventory Manager").GetComponent<InventoryManager>();

        canCollect = false;
        // Set logs parent
        parent = GameObject.Find("Logs").GetComponent<Transform>();
        this.transform.SetParent(parent);

        // Get the Rigidbody2D component attached to the logs
        rb = GetComponent<Rigidbody2D>();

        // Randomly select a log sprite
        this.GetComponent<SpriteRenderer>().sprite = logTypeSprites[RandomNumberRange()];
        player = GameObject.Find("Body").GetComponent<Transform>();

        // Apply a random force and torque to create a bounce effect
        StartCoroutine(ApplyBounce());
    }

    private void FixedUpdate()
    {
        if (!canCollect) { return; }

        // Check if the player is within range
        if (Vector2.Distance(transform.position, player.position) <= playerDetectRange) {
            // Move towards the player using Rigidbody2D physics
            MoveTowardsPlayer();
        }

        // Check if the logs are close enough to the player to be collected
        if (Vector2.Distance(transform.position, player.position) <= collectDistance) {
            CollectLog();
        }
    }

    // Method to apply random bounce effect
    private IEnumerator ApplyBounce()
    {
        // Apply a random force to make the logs bounce in a random direction
        Vector2 randomDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(0.5f, 1f)).normalized;
        rb.AddForce(randomDirection * bounceForce, ForceMode2D.Impulse);

        // Apply a random torque to make the logs rotate slightly
        float randomTorque = Random.Range(-bounceTorque, bounceTorque);
        rb.AddTorque(randomTorque, ForceMode2D.Impulse);

        yield return new WaitForSeconds(1);
        canCollect = true;
    }

    // Move the logs towards the player using Rigidbody2D
    private void MoveTowardsPlayer()
    {
        // Calculate the direction from the log to the player
        Vector2 direction = (player.position - transform.position).normalized;

        // Move the logs towards the player by adjusting the Rigidbody2D velocity
        rb.velocity = direction * moveSpeed;
    }

    // Method to "collect" the log (add to inventory, destroy the log, etc.)
    private void CollectLog()
    {
        // Add the log to the player's inventory (if needed, using InventoryManager)
        invManager.AddItem(logItem);

        // Destroy the log object when collected
        Destroy(gameObject);
    }

    private int RandomNumberRange()
    {
        int number = Random.Range(0, 2);
        return number;
    }
}
