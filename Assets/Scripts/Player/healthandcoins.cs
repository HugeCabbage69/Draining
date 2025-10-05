using UnityEngine;
using UnityEngine.UI;

public class healthandcoins : MonoBehaviour
{
    public PlayerValues playerValues;
    public Image fillImage;

    public bool IsCoin = false;

    void Update()
    {
        if (playerValues == null || fillImage == null) return;

        if (IsCoin)
        {
            fillImage.fillAmount = Mathf.Clamp01(playerValues.coins / 4f);
        }
        else
        {
            fillImage.fillAmount = Mathf.Clamp01(playerValues.health / playerValues.maxHealth);
        }
    }
}
