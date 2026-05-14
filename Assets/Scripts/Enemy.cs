using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 10f;
    public float health = 100f;

    [Header("Climbing")]
    public float climbSpeed = 6f;
    public float climbCheckDistance = 0.4f;
    public float wallStickForce = 0.5f;

    [Header("Climb State")]
    public float climbGraceTime = 0.1f;

    private Rigidbody rb;
    private Transform player;

    private bool isClimbing;
    private float climbTimer;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        rb.freezeRotation = true;
        rb.useGravity = true;
    }

    void FixedUpdate()
    {
        // Find player
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");

            if (p != null)
                player = p.transform;
            else
                return;
        }

        // Direction to player
        Vector3 direction =
            (player.position - transform.position).normalized;

        Vector3 flatDirection =
            new Vector3(direction.x, 0f, direction.z);

        // Face movement
        if (flatDirection != Vector3.zero)
        {
            transform.forward = flatDirection;
        }

        // Update climb state
        UpdateClimbing();

        // =========================
        // CLIMBING
        // =========================
        if (isClimbing)
        {
            rb.useGravity = false;

            Vector3 climbVelocity =
                (transform.forward * wallStickForce) +
                (Vector3.up * climbSpeed);

            rb.linearVelocity = climbVelocity;
        }
        else
        {
            rb.useGravity = true;

            rb.linearVelocity = new Vector3(
                flatDirection.x * speed,
                rb.linearVelocity.y,
                flatDirection.z * speed
            );
        }
    }

    void UpdateClimbing()
    {
        Vector3 origin =
            transform.position + Vector3.up * 0.5f;

        // Front wall check
        bool wallFront = Physics.SphereCast(
            origin,
            0.3f,
            transform.forward,
            out RaycastHit hit,
            climbCheckDistance
        );

        // Upper wall check
        bool wallAbove = Physics.SphereCast(
            origin + Vector3.up,
            0.3f,
            transform.forward,
            out RaycastHit hit2,
            climbCheckDistance
        );

        bool touchingWall = wallFront && wallAbove;

        // If touching wall → refresh timer
        if (touchingWall)
        {
            climbTimer = climbGraceTime;
        }
        else
        {
            climbTimer -= Time.fixedDeltaTime;
        }

        // Stay climbing while timer active
        isClimbing = climbTimer > 0f;
    }

    public void HitEnemy(float power)
    {
        health -= power;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Vector3 origin =
            transform.position + Vector3.up * 0.5f;

        Gizmos.DrawLine(
            origin,
            origin + transform.forward * climbCheckDistance
        );

        Gizmos.DrawLine(
            origin + Vector3.up,
            origin + Vector3.up +
            transform.forward * climbCheckDistance
        );
    }
}