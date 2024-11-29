using UnityEngine;

public class FishingLine : MonoBehaviour
{
    public Transform rodTipReference;  // The reference point (empty GameObject) for the rod tip
    public Transform hookPosition;     // The point where the fishing line ends (hook or fish position)

    private LineRenderer lineRenderer;

    // Control the number of segments for smooth curvature
    public int lineSegments = 10;  // More segments for smoother curve

    // Adjustable parameter for controlling the droop effect
    public float droopAmount = 0.3f;  // How much the line droops

    // Reference to the player or character for flipping checks
    public PlayerController player;

    public bool fish;
    public bool leftRight;

    void Start()
    {
        fish = false;
        player = GetComponent<PlayerController>();

        // Get the LineRenderer component on this object
        lineRenderer = GetComponent<LineRenderer>();

        // Set the width of the line
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;

        // Set the number of positions based on segments for smooth curvature
        lineRenderer.positionCount = lineSegments;
        leftRight = false;
    }

    void Update()
    {
        // Update the fishing line each frame if fishing is active
        UpdateRodTipPosition();
        if (fish) {
            UpdateFishingLine();
        }
    }

    // Dynamically adjust the rod tip position based on player flipping
    void UpdateRodTipPosition()
    {
        // Check if the player is facing left or right using the player's scale or a flip flag
        if (GetComponent<Animator>().GetFloat("IdleX") < 0 || GetComponent<Animator>().GetFloat("xInput") < 0 && GetComponent<Animator>().GetFloat("yInput") == 0) {
            // If facing left, adjust the rod tip's local position for the left side
            leftRight = true;
            rodTipReference.localPosition = new Vector3(-2f, 0.933f, 0); // Adjust these values based on your model
            hookPosition.localPosition = new Vector3(-3f, -0.777f, 0);
        } else if (GetComponent<Animator>().GetFloat("IdleX") > 0 || GetComponent<Animator>().GetFloat("xInput") > 0 && GetComponent<Animator>().GetFloat("yInput") == 0) {
            leftRight = true;
            // If facing right, adjust the rod tip's local position for the right side
            rodTipReference.localPosition = new Vector3(2f, 0.933f, 0); // Adjust these values based on your model
            hookPosition.localPosition = new Vector3(3f, -0.777f, 0);
        } else if (GetComponent<Animator>().GetFloat("IdleY") > 0 || GetComponent<Animator>().GetFloat("yInput") > 0 && GetComponent<Animator>().GetFloat("xInput") == 0) {
            leftRight = false;
            // If facing up, adjust the rod tip's local position
            rodTipReference.localPosition = new Vector3(-0.567f, 1.999f, 0); // Adjust these values based on your model
            hookPosition.localPosition = new Vector3(0f, 4f, 0);
        } else if (GetComponent<Animator>().GetFloat("IdleY") < 0 || GetComponent<Animator>().GetFloat("yInput") < 0 && GetComponent<Animator>().GetFloat("xInput") == 0) {
            leftRight = false;
            // If facing down, adjust the rod tip's local position
            rodTipReference.localPosition = new Vector3(0.461f, -2f, 0); // Adjust these values based on your model
            hookPosition.localPosition = new Vector3(0.063f, -4f, 0);
        }
    }

    void UpdateFishingLine()
    {
        // Only draw the line when facing left or right (leftRight is true)
        if (!leftRight) {
            // If not facing left or right, disable the line renderer
            lineRenderer.enabled = false;
            lineRenderer.startWidth = 0f;
            lineRenderer.endWidth = 0f;
            return;
        }

        // Enable the line renderer when facing left or right
        lineRenderer.enabled = true;
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;

        // Get the positions of the rod tip (using the reference GameObject) and the hook
        Vector3 startPoint = rodTipReference.position;  // Dynamically updates based on animation and flip
        Vector3 endPoint = hookPosition.position;

        // Loop through each segment and apply a curved droop effect
        for (int i = 0; i < lineSegments; i++) {
            // Interpolate between the start and end points (t goes from 0 to 1)
            float t = i / (float)(lineSegments - 1);

            // Linearly interpolate between start and end point
            Vector3 position = Vector3.Lerp(startPoint, endPoint, t);

            if (leftRight) {
                // Apply the droop effect using a sine wave curve, peaking in the middle of the line
                float droop = Mathf.Sin(t * Mathf.PI) * droopAmount;

                // Lower the position along the y-axis to simulate the line drooping
                position.y -= droop;
            }

            // Set the position in the LineRenderer
            lineRenderer.SetPosition(i, position);
        }
    }
}
