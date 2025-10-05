using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flipper : MonoBehaviour
{
    public GameObject player;
    public SpriteRenderer rend;
    public bool inverted = false;
    float x1 = 0f;
    float x2 = 0f;
    bool tempFlip = false;

    void Start()
    {
        if (rend == null) rend = GetComponent<SpriteRenderer>();
        if (player == null)
        {
            player = GameObject.FindWithTag("Player");
        }
    }

    void Update()
    {
        if (player == null) return;

        x1 = transform.position.x;
        x2 = player.transform.position.x;

        float diff = x2 - x1;

        if (diff > 0)
        {
            tempFlip = true;
        }
        else
        {
            tempFlip = false;
        }

        if (inverted)
        {
            tempFlip = !tempFlip;
        }

        if (rend != null)
        {
            rend.flipX = tempFlip;
        }
        else
        {
            Debug.Log("not work");
        }
    }
}
