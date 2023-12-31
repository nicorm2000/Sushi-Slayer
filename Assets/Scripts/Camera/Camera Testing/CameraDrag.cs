using System;
using System.Collections;
using UnityEngine;

public class CameraDrag : MonoBehaviour
{
    [Header("Drag Configuration")]
    [SerializeField] private Transform targetTransform;
    [SerializeField] private float cameraDragSpeed;
    [SerializeField] private float returnSpeedTime;
    [SerializeField] private AnimationCurve returnAnimationCurve;
    [SerializeField] private float maxDraggingDistance;

    [Header("Camera Configuration")]
    [SerializeField] private float offsetZ;

    [Header("Player Input Manager Dependencies")]
    [SerializeField] private PlayerInputManager playerInputManager;

    public static Vector3 initialPosition;
    private Vector3 releasePosition;
    private Vector3 dragStartPosition;
    private float timer = 0f;
    private bool isDragging = false;
    private Coroutine coroutine = null;

    /// <summary>
    /// Subscribes to the right mouse button down and up events.
    /// </summary>
    private void Awake()
    {
        PlayerInputManager.OnRightMouseButtonDown += StartDragging;
        PlayerInputManager.OnRightMouseButtonUp += StopDragging;
    }

    /// <summary>
    /// Unsubscribes from the right mouse button down and up events.
    /// </summary>
    private void OnDestroy()
    {
        PlayerInputManager.OnRightMouseButtonDown -= StartDragging;
        PlayerInputManager.OnRightMouseButtonUp -= StopDragging;
    }

    /// <summary>
    /// Updates the position of the object based on input and dragging state.
    /// </summary>
    private void Update()
    {
        initialPosition = new Vector3(targetTransform.position.x, targetTransform.position.y, targetTransform.position.z + offsetZ);

        if (!isDragging && coroutine == null)
        {
            transform.position = initialPosition;
        }

        if (isDragging)
        {
            float mouseX = playerInputManager.mouseDelta.x;
            float mouseY = playerInputManager.mouseDelta.y;
            Vector3 movement = new Vector3(-mouseX, -mouseY, 0f) * cameraDragSpeed * Time.deltaTime;
            Vector3 newPosition = transform.position + movement;

            if (Vector3.Distance(dragStartPosition, newPosition) > maxDraggingDistance)
            {
                return;
            }

            transform.Translate(movement, Space.World);
        }
    }

    /// <summary>
    /// Starts the dragging behavior when the right mouse button is pressed.
    /// </summary>
    /// <param name="mousePosition">The position of the mouse when the button is pressed.</param>
    private void StartDragging(Vector2 mousePosition)
    {
        isDragging = true;
        dragStartPosition = transform.position;
    }

    /// <summary>
    /// Stops the dragging behavior when the right mouse button is released.
    /// </summary>
    /// <param name="mousePosition">The position of the mouse when the button is released.</param>
    private void StopDragging(Vector2 mousePosition)
    {
        isDragging = false;

        if (coroutine == null)
        {
            releasePosition = transform.position;
            coroutine = StartCoroutine(ReturnToOriginalPosition());
        }
    }

    /// <summary>
    /// Coroutine that returns the object to its original position after being released from dragging.
    /// </summary>
    private IEnumerator ReturnToOriginalPosition()
    {
        timer = 0;

        while (timer < returnSpeedTime)
        {
            float progress = timer / returnSpeedTime;
            float curveValue = returnAnimationCurve.Evaluate(progress);
            transform.position = Vector3.Lerp(releasePosition, initialPosition, curveValue);
            timer += Time.deltaTime;

            yield return null;
        }

        transform.position = initialPosition;
        coroutine = null;
    }
}