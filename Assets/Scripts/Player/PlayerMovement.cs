using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashLength = 0.5f;
    [SerializeField] private float dashCooldown = 1;
    [SerializeField] private Material _playerDashMaterial;
    
    private Rigidbody2D _rigidBody;
    private BoxCollider2D _playerCollider;
    private Color _originalColor;
    private float activeMoveSpeed;
    private float dashCounter;
    private float dashCooldownCounter;

    [HideInInspector]
    public float lastHorizontalVector;
    [HideInInspector]
    public float lastVerticalVector;
    [HideInInspector]
    public Vector2 movementDirection;

    private void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _playerCollider = GetComponent<BoxCollider2D>();
        _playerDashMaterial = GetComponent<Renderer>().material;
        _originalColor = _playerDashMaterial.color;
        activeMoveSpeed = movementSpeed;
    }

    private void Update()
    {
        InputManagement();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void InputManagement()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        movementDirection = new Vector2(moveX, moveY).normalized;

        if (movementDirection.x != 0)
            lastHorizontalVector = movementDirection.x;

        if (movementDirection.y != 0)
            lastVerticalVector = movementDirection.y;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (dashCooldownCounter <= 0 && dashCounter <= 0)
            {
                _playerCollider.enabled = false;
                _playerDashMaterial.color = Color.cyan;
                activeMoveSpeed = dashSpeed;
                dashCounter = dashLength;
            }
        }

        if (dashCounter > 0)
        {
            dashCounter -= Time.deltaTime;

            if (dashCounter <= 0) 
            {
                activeMoveSpeed = movementSpeed;
                dashCooldownCounter = dashCooldown;
                _playerCollider.enabled = true;
                _playerDashMaterial.color = _originalColor;
            }
        }

        if (dashCooldownCounter > 0)
        {
            dashCooldownCounter -= Time.deltaTime;  
        }
    }

    private void Move()
    {
        _rigidBody.velocity = new Vector2(movementDirection.x * activeMoveSpeed, movementDirection.y * activeMoveSpeed);
    }
}