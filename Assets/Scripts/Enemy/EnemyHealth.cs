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

    public AudioSource coinsound;

    GameObject playerObj;
    PlayerValues playerScript;

    void Start()
    {
        float startHealth = maxHealth;
        currentHealth = startHealth;
        lastHealth = currentHealth;

        playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            playerScript = playerObj.GetComponent<PlayerValues>();
        }
        else
        {
            Debug.Log("dead");
        }
    }

    void Update()
    {
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        if (currentHealth < lastHealth)
        {
            if (damageSound != null)
            {
                damageSound.Play();
            }
            else
            {
                Debug.Log("enemy hurt sound missing on " + gameObject.name);
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
                Debug.Log("enemy died " + gameObject.name);
            }

            if (playerScript != null)
            {
                int rng = Random.Range(0, 15);
                if (rng == 0)
                {
                    coinsound.Play();
                    playerScript.coins += 1;
                    Debug.Log("+1");
                }
            }

            Destroy(gameObject, 0.1f);
        }

        tempFloat = currentHealth * 0.1f;
        tempInt = Mathf.FloorToInt(tempFloat);
        lastHealth = currentHealth;
    }
}
