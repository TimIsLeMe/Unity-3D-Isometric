using UnityEngine;

public class MainMenuCameraMovement : MonoBehaviour
{
    [SerializeField] private Transform pointA; // First position for the camera
    [SerializeField] private Transform pointB; // Second position for the camera
    [SerializeField] private Transform rotationPointA; // First rotation for the camera
    [SerializeField] private Transform rotationPointB; // Second rotation for the camera
    [SerializeField] private float speed = 0.0001f; // Speed of the camera movement

    private float t; // Timer to track the position between point A and point B

    void Update()
    {
        // Use Mathf.PingPong to move back and forth between 0 and 1 over time
        t = Mathf.PingPong(Time.time * speed, 1);

        // Move the camera between point A and point B
        transform.position = Vector3.Lerp(pointA.position, pointB.position, t);

        // Rotate the camera between rotationPointA and rotationPointB
        transform.rotation = Quaternion.Lerp(rotationPointA.rotation, rotationPointB.rotation, t);
    }
}