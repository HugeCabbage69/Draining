using UnityEngine;

public class Tinter : MonoBehaviour
{
    SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        if (sr != null)
        {
            Color c = new Color(Random.value, Random.value, Random.value);
            sr.color = c;
        }
        else
        {
            Debug.Log("fucked " + gameObject.name);
        }
    }
}
