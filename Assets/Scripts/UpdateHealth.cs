using UnityEngine;
using TMPro;

public class UpdateHealth : MonoBehaviour
{
    [SerializeField] private TMP_Text healthText;

    public void HealthChange(int health)
    {
        healthText.text = health.ToString();
    }
    
}
