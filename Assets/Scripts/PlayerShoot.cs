using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{
    [Header("References")]
    public Transform shootPoint;
    public GameObject projectilePrefab;

    [Header("Settings")]
    public float projectileSpeed = 80f;
    public float shootCooldown = 0.35f;

    private bool canShoot = true;

    public void OnAttack(bool pressed)
    {
        if (pressed && canShoot)
        {
            ShootProjectile();
            StartCoroutine(ShootDelay());
        }
    }

    private void ShootProjectile()
    {
        GameObject proj = Instantiate(projectilePrefab, shootPoint.position, shootPoint.rotation);

        Rigidbody rb = proj.GetComponent<Rigidbody>();
        rb.linearVelocity = shootPoint.forward * projectileSpeed;
    }

    private System.Collections.IEnumerator ShootDelay()
    {
        canShoot = false;
        yield return new WaitForSeconds(shootCooldown);
        canShoot = true;
    }
}
