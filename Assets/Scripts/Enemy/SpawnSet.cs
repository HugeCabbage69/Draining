using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnSet : MonoBehaviour
{
    public GameObject warningPrefab;
    public List<GameObject> enemyPrefabs;
    public int spawnCount = 3;
    public float warningTime = 2f;
    public float waveDelay = 5f;
    public Text waveText;

    private int waveNum = 1;
    private bool spawning = false;

    void Start()
    {
        if (waveText != null)
        {
            waveText.text = "Wave " + waveNum;
        }

        StartCoroutine(SpawnWave());
    }

    void Update()
    {
        if (!spawning)
        {
            int enemyCount = GameObject.FindObjectsOfType<EnemyHealth>().Length + GameObject.FindGameObjectsWithTag("Enemy").Length;
            if (enemyCount == 0)
            {
                StartCoroutine(StartNextWave());
            }
        }
    }

    IEnumerator StartNextWave()
    {
        spawning = true;

        float timer = waveDelay;
        while (timer > 0)
        {
            if (waveText != null)
            {
                waveText.text = "New Wave Starting In " + Mathf.Ceil(timer);
            }
            timer -= Time.deltaTime;
            yield return null;
        }

        waveNum++;
        if (waveText != null)
        {
            waveText.text = "Wave " + waveNum;
        }

        StartCoroutine(SpawnWave());
        yield return null;
    }

    IEnumerator SpawnWave()
    {
        spawning = true;

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

        spawnCount++;

        spawning = false;
    }
}
