using System.Collections;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [Header("Target to Follow")]
    private bool isOnAimPractice = false;
    private bool isOnBossArena = false;

    public Transform target;
    
    [Header("Camera Offset")]
    [SerializeField] public float maxOffset;
    [SerializeField] public Vector3 offset;

    [Header("Camera Movement Speed")]
    [SerializeField] private float cameraSpeed;

    [Header("Player Data Dependencies")]
    [SerializeField] private PlayerData playerData;

    /// <summary>
    /// Updates the camera position in the LateUpdate phase to follow the player's target.
    /// </summary>
    private void LateUpdate()
    {
        if (playerData.transform == null)
        {
            Debug.LogWarning("Camera target is not assigned.");
            return;
        }

        CheckCameraGoUp();
        CheckCameraGoDown();

        Vector3 desiredPosition = new Vector3(target.position.x + offset.x, target.position.y + offset.y, transform.position.z);
        transform.position = desiredPosition;
    }

    /// <summary>
    /// Check if camera should move up.
    /// </summary>
    private void CheckCameraGoUp() 
    {
        if (isOnAimPractice)
        {
            if (offset.y >= maxOffset)
            {
                offset.y -= cameraSpeed * Time.deltaTime;
            }
        }
    }

    /// <summary>
    /// Check if camera should move down.
    /// </summary>
    private void CheckCameraGoDown() 
    {
        if (!isOnAimPractice)
        {
            if (offset.y < 0)
            {
                offset.y += cameraSpeed * Time.deltaTime;
            }

            if (offset.y >= 0)
            {
                offset.y = 0f;
            }
        }
    }

    /// <summary>
    /// Sets true or false depending on whether the player is at aim practice.
    /// </summary>
    /// <param name="isActive"></param>
    public void SetAimPractiveActivator(bool isActive) 
    {
        isOnAimPractice = isActive;
    }
}