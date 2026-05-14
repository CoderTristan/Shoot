using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 100;
    public int currentHealth;
    [SerializeField] private UpdateHealth updateHealth;

    [Header("Death UI")]
    public GameObject deathScreen; 

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        updateHealth.HealthChange(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player died");
        GetComponent<PlayerCharacter>().enabled = false;
        GetComponent<PlayerShoot>().enabled = false;


        if (deathScreen != null) deathScreen.SetActive(true);
    }
}
