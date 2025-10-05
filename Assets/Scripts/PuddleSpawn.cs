using UnityEngine;

public class SpawnerWhenMoving : MonoBehaviour
{
    public GameObject spawnPrefab;
    public float spawnInterval = 0.8f;
    public Rigidbody playerRigidbody;
    public Transform spawnPoint;

    private float spawnTime = 0f;

    void Update()
    {
        if (playerRigidbody == null) return;

        bool isMoving = new Vector3(playerRigidbody.linearVelocity.x, 0, playerRigidbody.linearVelocity.z).magnitude > 0.1f;

        if (isMoving)
        {
            spawnTime += Time.deltaTime;

            if (spawnTime >= spawnInterval)
            {
                SpawnPrefab();
                spawnTime = 0f;
            }
        }
        else
        {
            spawnTime = 0f;
        }
    }

    void SpawnPrefab()
    {
        if (spawnPrefab != null)
        {
            Vector3 spawnPos = spawnPoint.position;

            Instantiate(spawnPrefab, spawnPos, Quaternion.identity);
        }
    }
}
