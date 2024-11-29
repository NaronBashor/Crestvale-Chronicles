using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OreCollectible : MonoBehaviour
{
    public Item oreItem;

    public int minAmount;
    public int maxAmount;

    private InventoryManager invManager;
    private Transform player;

    private Transform parent;
    private Rigidbody2D rb;

    public float bounceForce = 2f;
    public float bounceTorque = 10f;

    // Movement towards player
    public float playerDetectRange = 5f;
    public float moveSpeed = 5f;
    public float collectDistance = 0.5f;

    public bool canCollect = false;

    private void Start()
    {
        invManager = GameObject.Find("Inventory Manager").GetComponent<InventoryManager>();

        canCollect = false;

        parent = GameObject.Find("Ores").GetComponent<Transform>();
        this.transform.SetParent(parent);

        rb = GetComponent<Rigidbody2D>();

        player = GameObject.Find("Body").GetComponent<Transform>();

        StartCoroutine(ApplyBounce());
    }

    private void FixedUpdate()
    {
        if (!canCollect) { return; }

        if (Vector2.Distance(transform.position, player.position) <= playerDetectRange) {
            MoveTowardsPlayer();
        }

        if (Vector2.Distance(transform.position, player.position) <= collectDistance) {
            CollectOre();
        }
    }

    private IEnumerator ApplyBounce()
    {
        Vector2 randomDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(0.5f, 1f)).normalized;
        rb.AddForce(randomDirection * bounceForce, ForceMode2D.Impulse);

        float randomTorque = Random.Range(-bounceTorque, bounceTorque);
        rb.AddTorque(randomTorque, ForceMode2D.Impulse);

        yield return new WaitForSeconds(1);
        canCollect = true;
    }

    private void MoveTowardsPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;

        rb.velocity = direction * moveSpeed;
    }

    private void CollectOre()
    {
        invManager.AddItem(oreItem);

        Destroy(gameObject);
    }

    private int RandomNumberRange()
    {
        int number = Random.Range(minAmount, maxAmount);
        return number;
    }
}
