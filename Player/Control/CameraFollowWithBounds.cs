using UnityEngine;

public class CameraFollowWithBounds : MonoBehaviour
{
    public Transform player; // Reference to the player
    public float smoothSpeed = 0.125f; // Smooth factor for camera movement

    // Define the boundaries for the camera
    public float minX; // Minimum X position the camera can move to
    public float maxX; // Maximum X position the camera can move to
    public float minY; // Minimum Y position the camera can move to
    public float maxY; // Maximum Y position the camera can move to

    private Vector3 offset; // Offset between the camera and player

    void Start()
    {
        // Calculate the initial offset between the player and camera
        offset = transform.position - player.position;
    }

    void LateUpdate()
    {
        // Desired camera position is the player's position with the offset
        Vector3 desiredPosition = player.position + offset;

        // Clamp the camera's position to make sure it stays within bounds
        float clampedX = Mathf.Clamp(desiredPosition.x, minX, maxX);
        float clampedY = Mathf.Clamp(desiredPosition.y, minY, maxY);

        // Smoothly interpolate the camera's position for smoother movement
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, new Vector3(clampedX, clampedY, transform.position.z), smoothSpeed);

        // Set the camera's position to the smoothed and clamped position
        transform.position = smoothedPosition;
    }
}
