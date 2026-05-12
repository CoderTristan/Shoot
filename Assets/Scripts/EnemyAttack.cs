using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public float attackCooldown = 1f; 
    private bool canAttack = true;

    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            TryAttack(collision.collider.GetComponent<PlayerHealth>());
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            TryAttack(other.GetComponent<PlayerHealth>());
        }
    }

    private void TryAttack(PlayerHealth player)
    {
        if (player == null || !canAttack)
            return;

        int damage = Random.Range(1, 5);

        player.TakeDamage(damage);
        StartCoroutine(AttackDelay());
    }

    private System.Collections.IEnumerator AttackDelay()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }
}
