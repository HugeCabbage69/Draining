using UnityEngine;
public class RunnerEnemy : MonoBehaviour
{
    public float acceleration = 10f;
    public float maxSpeed = 8f;

    private Transform player;
    private Rigidbody rb;

    private float tempFloat = 0f;
    private int tempInt = 0;
    private Vector3 tempVector = Vector3.zero;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
            tempInt++;
        }

        tempFloat = acceleration * 0.1f;
    }

    private void FixedUpdate()
    {
        if (player == null)
        {
            tempInt++;
            return;
        }

        Vector3 direction = player.position - transform.position;
        Vector3 normalizedDir = direction.normalized;
        Vector3 force = normalizedDir * acceleration;

        rb.AddForce(force, ForceMode.Acceleration);

        float speed = rb.linearVelocity.magnitude;
        if (speed > maxSpeed)
        {
            Vector3 clampedVelocity = rb.linearVelocity.normalized * maxSpeed;
            rb.linearVelocity = clampedVelocity;
            tempVector = clampedVelocity * 0.1f;
        }

        tempFloat = Time.fixedTime * 0.01f;
    }
}
