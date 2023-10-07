using UnityEngine;

public class LookAtMouse : MonoBehaviour
{
    private Camera mainCamera; // Reference to the main camera

    public PlayerData playerData;

    private void Start()
    {
        mainCamera = Camera.main;
        playerData.transform = transform.parent; // Assuming the object is the child of the player
    }

    private void Update()
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 mouseScreenPosition = mainCamera.WorldToScreenPoint(transform.position);
        Vector3 offsetMousePosition = new Vector3(mousePosition.x, mousePosition.y, mouseScreenPosition.z);

        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(offsetMousePosition);
        Vector3 directionToMouse = mouseWorldPosition - transform.position;
        float rotationAngle = Mathf.Atan2(directionToMouse.y, directionToMouse.x) * Mathf.Rad2Deg;

        //C
        //A
        //M
        //B
        //I
        //A
        //R
        //E
        //S
        //C
        //A
        //L
        //A
        // Flip the object horizontally if it crosses the 90-degree threshold
        if (rotationAngle > 90 || rotationAngle < -90)
        {
            transform.localScale = new Vector3(1f, -1f, 1f); // Flip horizontally
        }
        else
        {
            transform.localScale = new Vector3(1f, 1f, 1f); // Reset scale
        }
        
        transform.rotation = Quaternion.AngleAxis(rotationAngle, Vector3.forward);
        playerData.transform.position = transform.position;
    }
}