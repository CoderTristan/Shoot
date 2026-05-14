using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 100;
    public int currentHealth;
    [SerializeField] private UpdateHealth updateHealth;

    [Header("Death UI")]
    public GameObject deathScreen; 
    public GameObject healthScreen; 


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

            updateHealth.HealthChange(0);


            Die();
        }

    }

    private void Die()
    {
        if (healthScreen != null) healthScreen.SetActive(false);
        if (deathScreen != null) deathScreen.SetActive(true);
    }
}
