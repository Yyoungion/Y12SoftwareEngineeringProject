using UnityEngine;

public class CamaerFollow : MonoBehaviour
{
    // The object the camera will follow
    public Transform target;
    
    // Controls how smoothly the camera follows the target
    public float smoothSpeed = 0.125f;
    
    // The camera's offset from the target
    public Vector3 offset;

    // Called after all Update() methods have finished each frame
    void LateUpdate()
    {
        // Exit if no target has been assigned
        if (target == null) return;

        // Calculate the desired camera position using the target's position and offset
        Vector3 desiredPosition = target.position + offset;
        
        // Smoothly move the camera towards the desired position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        
        // Update the camera's position
        transform.position = smoothedPosition;
    }
}