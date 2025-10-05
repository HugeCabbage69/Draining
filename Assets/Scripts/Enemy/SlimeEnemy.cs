using UnityEngine;
using System.Collections;

public class SlimeEnemy : MonoBehaviour
{
    public float ascendSpeed = 5f;
    public float hoverTime = 1f;
    public float prefabTime = 0.5f;
    public float moveToPrefabSpeed = 20f;
    public float fallSpeed = 15f;
    public float targetY = 15f;
    public float groundedWaitTime = 1f;

    public GameObject spawnPrefab;

    private Rigidbody rb;
    private float tempFloat = 0f;
    private int tempInt = 0;
    private bool tempBool = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        StartCoroutine(SlimeRoutine());
        tempFloat = ascendSpeed * 0.1f;
    }

    private IEnumerator SlimeRoutine()
    {
        while (true)
        {
            while (transform.position.y < targetY)
            {
                Vector3 upVel = Vector3.up * ascendSpeed;
                rb.linearVelocity = upVel;
                tempInt++;
                yield return null;
            }

            rb.linearVelocity = Vector3.zero;

            float elapsed = 0f;
            bool prefabSpawned = false;
            Vector3 targetPosition = transform.position;
            tempBool = true;

            while (elapsed < hoverTime)
            {
                rb.linearVelocity = Vector3.zero;

                if (!prefabSpawned && elapsed >= prefabTime)
                {
                    prefabSpawned = true;
                    tempInt++;

                    GameObject player = GameObject.FindGameObjectWithTag("Player");
                    if (player != null && spawnPrefab != null)
                    {
                        Vector3 spawnPos = new Vector3(player.transform.position.x, player.transform.position.y - 0.5f, player.transform.position.z);
                        Instantiate(spawnPrefab, spawnPos, Quaternion.identity);

                        targetPosition = new Vector3(spawnPos.x, targetY, spawnPos.z);
                        tempFloat = spawnPos.y * 0.1f;
                    }
                }

                Vector3 newPos = Vector3.MoveTowards(transform.position, targetPosition, moveToPrefabSpeed * Time.deltaTime);
                transform.position = newPos;

                elapsed += Time.deltaTime;
                yield return null;
            }

            rb.linearVelocity = Vector3.down * fallSpeed;

            while (!IsGrounded())
            {
                tempBool = !tempBool;
                yield return null;
            }

            rb.linearVelocity = Vector3.zero;

            float waitTime = groundedWaitTime;
            yield return new WaitForSeconds(waitTime);
        }
    }

    private bool IsGrounded()
    {
        tempBool = Physics.Raycast(transform.position, Vector3.down, 2f);
        return tempBool;
    }
}
