using UnityEngine;

public class PlayerValues : MonoBehaviour
{
    public float health = 100f;
    public float maxHealth = 100f;
    public int coins = 0;

    public float regenSpeed = 0.5f;
    public bool canRegen = true;

    void Update()
    {
        if (canRegen && health < 100f)
        {
            health += regenSpeed * Time.deltaTime;
        }

        health = Mathf.Clamp(health, 0f, maxHealth);
        coins = Mathf.Clamp(coins, 0, 4);
    }
}
