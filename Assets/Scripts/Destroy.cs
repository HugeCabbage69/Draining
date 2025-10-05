using UnityEngine;

public class Destroy : MonoBehaviour
{
    public float lifetime = 0.7f;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }
}
