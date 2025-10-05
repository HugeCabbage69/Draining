using System.Collections;
using UnityEngine;

public class DasherEnemy : MonoBehaviour
{
    public float dashDistance = 10f;
    public float dashSpeed = 20f;
    public float waitTime = 2f;

    Rigidbody rb;
    Transform player;

    bool isDashing = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null)
        {
            player = p.transform;
        }

        StartCoroutine(DashLoop());
    }

    IEnumerator DashLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);

            if (player != null && !isDashing)
            {
                StartCoroutine(Dash());
            }
        }
    }

    IEnumerator Dash()
    {
        isDashing = true;

        Vector3 start = transform.position;
        Vector3 dir = (player.position - start).normalized;
        Vector3 target = start + dir * dashDistance;

        RaycastHit hit;
        if (Physics.Raycast(start, dir, out hit, dashDistance))
        {
            target = hit.point;
        }

        while ((target - transform.position).sqrMagnitude > 0.05f)
        {
            Vector3 move = dir * dashSpeed * Time.fixedDeltaTime;
            if (move.sqrMagnitude > (target - transform.position).sqrMagnitude)
            {
                move = target - transform.position;
            }

            rb.MovePosition(transform.position + move);
            yield return new WaitForFixedUpdate();
        }

        rb.MovePosition(target);
        isDashing = false;
    }

    void OnCollisionEnter(Collision col)
    {
        if (isDashing)
        {
            StopAllCoroutines();
            StartCoroutine(DashLoop());
            isDashing = false;
        }
    }
}
