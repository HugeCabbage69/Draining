using UnityEngine;

public class PlayerValues : MonoBehaviour
{
    public float health = 100f;
    public float maxHealth = 100f;
    public int coins = 0;

    public float regenSpeed = 0.5f;
    public bool canRegen = true;

    public float damagePerSecond = 10f;
    public LayerMask enemyLayer;

    void Update()
    {
        // regen stuff
        if (canRegen && health < maxHealth)
        {
            health += regenSpeed * Time.deltaTime;
        }

        health = Mathf.Clamp(health, 0f, maxHealth);
        coins = Mathf.Clamp(coins, 0, 4);
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy") || ((1 << other.gameObject.layer) & enemyLayer) != 0)
        {
            health -= damagePerSecond * Time.deltaTime;

            if (health <= 0)
            {
                Debug.Log("player died");
                health = 0;
            }
        }
    }
}
