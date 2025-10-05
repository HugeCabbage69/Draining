using UnityEngine;

public class TriggerEnable : MonoBehaviour
{
    public GameObject menu;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (menu != null)
                menu.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (menu != null)
                menu.SetActive(false);
        }
    }
}
