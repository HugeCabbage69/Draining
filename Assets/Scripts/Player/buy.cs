using Unity.VisualScripting;
using UnityEngine;

public class buy : MonoBehaviour
{
    public PlayerValues playerValues;

    public AttackingP patk;

    public bool ishealth;
    public bool isdash;
    public FirstPersonMovement fpm;
    public void OnBuyButtonClick()
    {
        if (playerValues != null)
        {
            if (playerValues.coins >= 2)
            {
                if (isdash)
                {
                    fpm.dashKey = KeyCode.LeftShift;
                    playerValues.coins = playerValues.coins - 2;
                    Destroy(gameObject);
                }
                else
                {
                    if (ishealth)
                    {
                        playerValues.maxHealth = playerValues.maxHealth + 1;
                        playerValues.coins = playerValues.coins - 2;
                    }
                    else
                    {
                        patk.damage = patk.damage + 2;
                        playerValues.coins = playerValues.coins - 2;
                    }
                }
            }
            else
            {
                Debug.Log("fucked");
            }
        }
        else
        {
            Debug.LogWarning("missig");
        }
    }
}
