using System;
using UnityEngine;

public class CubeInput : MonoBehaviour
{
    [Header("Player Configuration")]
    [SerializeField] private float moveSpeed;

    [Header("Interactable Space")]
    [SerializeField] private float screenEdgeThreshold;

    public static event Action<Vector2> OnRightMouseButtonDown;
    public static event Action<Vector2> OnRightMouseButtonUp;

    private bool isRightMouseButtonHeld = false;

    private void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += new Vector3(0, 5f, 0) * moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position += new Vector3(0, -5f, 0) * moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.position += new Vector3(-5f, 0, 0) * moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += new Vector3(5f, 0, 0) * moveSpeed * Time.deltaTime;
        }

        if (Input.GetMouseButtonDown(1))
        {
            isRightMouseButtonHeld = true;
        }
        else if (Input.GetMouseButtonUp(1))
        {
            isRightMouseButtonHeld = false;
            OnRightMouseButtonUp?.Invoke(Input.mousePosition);
        }

        if (isRightMouseButtonHeld)
        {
            if (!IsMouseInsideScreen())
            {
                OnRightMouseButtonDown?.Invoke(Input.mousePosition);
            }
            else
            {
                OnRightMouseButtonUp?.Invoke(Input.mousePosition);
            }
        }
    }

    /// <summary>
    /// Checks if the mouse is at the edge of the screen.
    /// </summary>
    /// <returns>True if the mouse is at the screen edge, false otherwise.</returns>
    public bool IsMouseInsideScreen()
    {
        Vector3 mousePosition = Input.mousePosition;
        return mousePosition.x <= screenEdgeThreshold || mousePosition.x >= Screen.width - screenEdgeThreshold || 
            mousePosition.y <= screenEdgeThreshold || mousePosition.y >= Screen.height - screenEdgeThreshold;
    }

    //Better camera movement
    //if (Input.GetMouseButtonDown(1))
    //{
    //    OnRightMouseButtonDown?.Invoke(Input.mousePosition);
    //}
    //
    //if (Input.GetMouseButtonUp(1))
    //{
    //    OnRightMouseButtonUp?.Invoke(Input.mousePosition);
    //}
    //Limiting camera mouse movement with rect area
    //if (IsMouseWithinArea())
    //{
    //    Debug.Log("a");
    //}
    //
    //if (Input.GetMouseButtonDown(1))
    //{
    //    if(IsMouseWithinArea())
    //    {
    //        OnRightMouseButtonDown?.Invoke(Input.mousePosition);
    //    }
    //    else
    //    {
    //        OnRightMouseButtonUp?.Invoke(Input.mousePosition);
    //    }
    //}
    //private bool IsMouseWithinArea()
    //{
    //    Vector2 mousePosition = GetLocalMousePosition();
    //    Rect areaRect = new Rect(Vector2.zero, mouseCollisionArea.rect.size);
    //    return areaRect.Contains(mousePosition);
    //}
    //
    //private Vector2 GetLocalMousePosition()
    //{
    //    RectTransformUtility.ScreenPointToLocalPointInRectangle(mouseCollisionArea, Input.mousePosition, null, out Vector2 localMousePosition);
    //    return localMousePosition;
    //}
    //Limiting camera mouse movement with time
    //if (isTimerRunning)
    //{
    //    timer += Time.deltaTime;
    //
    //    if (timer >= holdingTime)
    //    {
    //        isTimerRunning = false;
    //        timer = 0f;
    //        OnRightMouseButtonUp?.Invoke(Input.mousePosition);
    //    }
    //}
    //if (Input.GetMouseButtonUp(1) || isTimerRunning)
    //{
    //    timer += Time.deltaTime;
    //    if (timer >= holdingTime)
    //    {
    //        isTimerRunning = false;
    //        timer = 0f;
    //    }
    //    OnRightMouseButtonUp?.Invoke(Input.mousePosition);
    //}
}