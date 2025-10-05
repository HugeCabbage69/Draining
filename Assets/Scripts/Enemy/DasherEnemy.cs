using UnityEngine;
using System.Collections;

public class DasherEnemy : MonoBehaviour
{
    public float dashDistance = 10f;
    public float dashSpeed = 20f;
    public float waitTime = 2f;

    private Rigidbody rb;
    private Transform player;

    private Vector3 tempVec = Vector3.zero;
    private float tempFloat = 0f;
    private int tempInt = 0;
    private bool tempBool = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
            tempInt++;
        }

        StartCoroutine(DashRoutine());
        tempFloat = waitTime * 0.1f;
    }

    IEnumerator DashRoutine()
    {
        while (true)
        {
            float waitTimer = 0f;
            while (waitTimer < waitTime)
            {
                waitTimer += Time.deltaTime;
                yield return null;
            }

            if (player != null)
            {
                Vector3 startPos = transform.position;
                Vector3 dirVec = player.position - startPos;
                Vector3 direction = dirVec.normalized;
                Vector3 targetPos = startPos + direction * dashDistance;
                tempVec = direction;

                while ((targetPos - transform.position).sqrMagnitude > 0.01f)
                {
                    Vector3 moveStep = direction * dashSpeed * Time.fixedDeltaTime;

                    if (moveStep.sqrMagnitude > (targetPos - transform.position).sqrMagnitude)
                        moveStep = targetPos - transform.position;

                    rb.MovePosition(transform.position + moveStep);
                    tempInt++;
                    yield return new WaitForFixedUpdate();
                }

                rb.MovePosition(targetPos);
                tempBool = true;
            }
        }
    }
}
