using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("References")]
    public Rigidbody rb;
    public Transform cam;

    [Header("Movement")]
    public float groundAccel = 30f;
    public float airAccel = 12f;
    public float maxGroundSpeed = 14f;
    public float maxAirSpeed = 22f;
    public float friction = 6f;

    [Header("Jumping")]
    public float jumpForce = 12f;
    public float groundCheckDistance = 1.1f;

    [Header("Sliding")]
    public float slideBoost = 1.3f;
    public float slideFriction = 1f;
    bool sliding;

    float inputX, inputY;
    bool grounded;

    void Start()
    {
        rb.freezeRotation = true;
    }

    void Update()
    {
        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.LeftControl))
            StartSlide();
        if (Input.GetKeyUp(KeyCode.LeftControl))
            StopSlide();

        if (Input.GetKeyDown(KeyCode.Space) && grounded)
            Jump();
    }

    void FixedUpdate()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance);

        if (grounded && !sliding)
            ApplyFriction();

        MovePlayer();
    }

    void MovePlayer()
    {
        Vector3 moveDir = (cam.forward * inputY + cam.right * inputX).normalized;
        moveDir.y = 0;

        float accel = grounded ? groundAccel : airAccel;
        float maxSpeed = grounded ? maxGroundSpeed : maxAirSpeed;

        // Add acceleration
        rb.linearVelocity += moveDir * accel * Time.fixedDeltaTime;

        // Clamp horizontal speed
        Vector3 horizontalVel = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        if (horizontalVel.magnitude > maxSpeed)
        {
            horizontalVel = horizontalVel.normalized * maxSpeed;
            rb.linearVelocity = new Vector3(horizontalVel.x, rb.linearVelocity.y, horizontalVel.z);
        }
    }

    void ApplyFriction()
    {
        Vector3 horizontalVel = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        Vector3 frictionForce = -horizontalVel * friction * Time.fixedDeltaTime;
        rb.linearVelocity += new Vector3(frictionForce.x, 0, frictionForce.z);
    }

    void Jump()
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    void StartSlide()
    {
        if (!grounded) return;
        sliding = true;
        rb.linearVelocity *= slideBoost;
    }

    void StopSlide()
    {
        sliding = false;
    }
}
