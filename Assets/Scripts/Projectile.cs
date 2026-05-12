using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Lifetime")]
    public float lifeTime = 5f;

    [Header("Explosion")]
    public float radius = 10f;
    public float explosionForce = 20f;

    [Header("Directional Control")]
    [Range(0f, 2f)]
    public float upwardForce = 0.1f;

    [Range(0f, 1f)]
    public float distanceFalloff = 0.7f;
    // 0 = same force everywhere
    // 1 = weak force at edge of radius

    [Header("Velocity Limits")]
    public float maxVelocity = 12f;
    public float damping = 2f;

    [Header("Damage")]
    public int damage = 5; // base value, not used anymore but kept for inspector clarity

    private bool exploded = false;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (exploded)
            return;

        exploded = true;
        Explode();
    }

    private void Explode()
    {

        Collider[] hits = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider hit in hits)
        {
            if (!hit.CompareTag("Enemy"))
                continue;

            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb != null)
            {
                ApplyControlledExplosion(rb);
            }

            Enemy enemy = hit.GetComponent<Enemy>();
            if (enemy != null)
            {
                // Random damage between 10 and 30
                int randomDamage = Random.Range(10, 31); 
                enemy.HitEnemy(randomDamage);
            }
        }

        Destroy(gameObject);
    }

    private void ApplyControlledExplosion(Rigidbody rb)
    {
        Vector3 explosionCenter = transform.position;
        Vector3 targetPosition = rb.worldCenterOfMass;

        // Direction from explosion to target
        Vector3 direction = targetPosition - explosionCenter;

        // Distance from center
        float distance = direction.magnitude;

        // Normalize direction
        direction.Normalize();

        // Add custom upward force
        direction.y += upwardForce;

        // Normalize again after modifying Y
        direction.Normalize();

        // Distance-based falloff
        float normalizedDistance = distance / radius;

        float falloffMultiplier =
            Mathf.Lerp(
                1f,
                1f - normalizedDistance,
                distanceFalloff
            );

        // Final force
        Vector3 finalForce =
            direction *
            explosionForce *
            falloffMultiplier;

        // Apply force
        rb.AddForce(finalForce, ForceMode.Impulse);

        // Smooth movement
        rb.linearDamping = damping;

        // Clamp velocity
        rb.linearVelocity =
            Vector3.ClampMagnitude(
                rb.linearVelocity,
                maxVelocity
            );
    }
}
