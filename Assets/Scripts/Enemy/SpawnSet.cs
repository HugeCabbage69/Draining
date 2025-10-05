using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSet : MonoBehaviour
{
    public GameObject warningPrefab;
    public List<GameObject> enemyPrefabs;
    public int spawnCount = 3;
    public float warningTime = 2f;
    public float waveDelay = 5f;

    private int waveNum = 1;
    private bool spawning = false;

    void Start()
    {
        StartCoroutine(SpawnWaves());
    }

    IEnumerator SpawnWaves()
    {
        while (true)
        {
            if (GameObject.FindObjectsOfType<EnemyHealth>().Length > 0 || GameObject.FindGameObjectsWithTag("Enemy").Length > 0)
            {
                yield return null;
            }
            else
            {
                if (!spawning)
                {
                    spawning = true;
                    StartCoroutine(SpawnWave());
                }
            }
            yield return null;
        }
    }

    IEnumerator SpawnWave()
    {
        List<Vector3> spawnPositions = new List<Vector3>();

        for (int i = 0; i < spawnCount; i++)
        {
            float x = Random.Range(-30f, 31f);
            float z = Random.Range(-22f, 13f);
            Vector3 pos = new Vector3(x, 1f, z);
            spawnPositions.Add(pos);

            Instantiate(warningPrefab, pos, Quaternion.identity);
        }

        yield return new WaitForSeconds(warningTime);

        for (int i = 0; i < spawnPositions.Count; i++)
        {
            if (enemyPrefabs.Count > 0)
            {
                int r = Random.Range(0, enemyPrefabs.Count);
                Vector3 spawnAbove = spawnPositions[i] + new Vector3(0, 9f, 0);
                GameObject enemy = Instantiate(enemyPrefabs[r], spawnAbove, Quaternion.identity);

                enemy.tag = "Enemy";

                EnemyHealth eh = enemy.GetComponent<EnemyHealth>();
                if (eh != null)
                {
                    float bonus = Mathf.Floor(waveNum / 2);
                    eh.maxHealth += bonus;
                    eh.currentHealth = eh.maxHealth;
                }
            }
        }

        waveNum++;

        spawnCount++;

        yield return new WaitForSeconds(waveDelay);

        spawning = false;
    }
}
