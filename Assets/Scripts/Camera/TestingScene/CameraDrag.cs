using System.Collections;
using UnityEngine;

public class CameraDrag : MonoBehaviour
{
    [Header("Drag Configuration")]
    [SerializeField] private Transform targetTransform;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float returnSpeedTime;
    [SerializeField] private AnimationCurve returnAnimationCurve;

    [Header("Cammera Configuration")]
    [SerializeField] private float offsetZ;

    private Vector3 initialPosition;
    private Vector3 releasePosition;
    private float timer = 0f;
    private bool isDragging = false;
    private Coroutine coroutine = null;

    /// <summary>
    /// Subscribes to the right mouse button down and up events.
    /// </summary>
    private void Start()
    {
        CubeInput.OnRightMouseButtonDown += StartDragging;
        CubeInput.OnRightMouseButtonUp += StopDragging;
    }

    /// <summary>
    /// Unsubscribes from the right mouse button down and up events.
    /// </summary>
    private void OnDestroy()
    {
        CubeInput.OnRightMouseButtonDown -= StartDragging;
        CubeInput.OnRightMouseButtonUp -= StopDragging;
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
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");
            Vector3 movement = new Vector3(-mouseX, -mouseY, 0f) * moveSpeed * Time.deltaTime;
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
    }

    /// <summary>
    /// Stops the dragging behavior when the right mouse button is released.
    /// </summary>
    /// <param name="mousePosition">The position of the mouse when the button is released.</param>
    private void StopDragging(Vector2 mousePosition)
    {
        isDragging = false;
        releasePosition = transform.position;

        if (coroutine == null)
        {
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

            if (isDragging)
            {
                transform.position = initialPosition;
                yield return null;
            }

            yield return null;
        }

        transform.position = initialPosition;
        coroutine = null;
    }
}