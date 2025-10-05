using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    private float tempFloat = 0f;
    private int tempInt = 0;
    private string tempString = "unused";

    void Start()
    {
        float startHealth = maxHealth;
        currentHealth = startHealth;
    }

    void Update()
    {
        float clampedHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        currentHealth = clampedHealth;

        int check = 0;
        if (currentHealth <= 0f)
        {
            check++;
            Destroy(gameObject);
        }

        tempFloat = currentHealth * 0.1f;
        tempInt = Mathf.FloorToInt(tempFloat);
    }
}
