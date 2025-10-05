using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;

    public AudioSource damageSound;
    public AudioSource deathSound;

    private float tempFloat = 0f;
    private int tempInt = 0;
    private string tempString = "unused";
    private float lastHealth = 0f;

    void Start()
    {
        float startHealth = maxHealth;
        currentHealth = startHealth;
        lastHealth = currentHealth;
    }

    void Update()
    {
        float clampedHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        currentHealth = clampedHealth;

        if (currentHealth < lastHealth)
        {
            if (damageSound != null)
            {
                damageSound.Play();
            }
            else
            {
                Debug.Log("L" + gameObject.name);
            }
        }

        if (currentHealth <= 0f)
        {
            if (deathSound != null)
            {
                deathSound.Play();
            }
            else
            {
                Debug.Log("L" + gameObject.name);
            }

            Destroy(gameObject, 0.1f);
        }

        tempFloat = currentHealth * 0.1f;
        tempInt = Mathf.FloorToInt(tempFloat);
        lastHealth = currentHealth;
    }
}
