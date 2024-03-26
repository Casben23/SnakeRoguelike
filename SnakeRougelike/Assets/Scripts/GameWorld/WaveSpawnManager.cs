using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawnManager : MonoBehaviour
{
    [SerializeField] private float m_MinTimeBtwSpawns = 3f, m_MaxTimeBtwSpawn = 5f;
    [SerializeField] private int m_MinEnemiesPerGroup = 2;
    [SerializeField] private int m_MaxEnemiesPerGroup = 10;

    [SerializeField] private int m_EnemiesPerGroup = 4;
    [SerializeField] private GameObject m_EnemySpawnIndicator;

    private List<EnemySpawn> m_EnemiesToSpawn = new List<EnemySpawn>();
    private bool m_WaveCompleted = false;
    private int m_EnemiesToKill = 0;

    public void InitiateNewWave(List<EnemySpawn> InEnemiesToSpawn)
    {
        m_EnemiesToSpawn = InEnemiesToSpawn;
        m_EnemiesToKill = m_EnemiesToSpawn.Count;

        int random = Random.Range(m_MinEnemiesPerGroup, m_EnemiesToSpawn.Count);
        m_EnemiesPerGroup = Mathf.Clamp(random, m_MinEnemiesPerGroup, m_MaxEnemiesPerGroup);

        m_WaveCompleted = false;

        StartCoroutine(SpawnEnemies());
    }

    public IEnumerator SpawnEnemies()
    {
        int totalEnemiesSpawned = 0;

        while(totalEnemiesSpawned < m_EnemiesToSpawn.Count)
        {
            Vector2 enemieIndicatorSpawnPosition = GetRandomSpawnPosition();
            GameObject spawnIndicator = Instantiate(m_EnemySpawnIndicator, enemieIndicatorSpawnPosition, Quaternion.identity);
            Destroy(spawnIndicator, 2);

            int enemiesInGroup = Mathf.Min(m_EnemiesPerGroup, m_EnemiesToSpawn.Count - totalEnemiesSpawned);

            yield return new WaitForSeconds(1.5f);

            for (int i = 0; i < enemiesInGroup; i++)
            {
                Vector2 spawnPosition = enemieIndicatorSpawnPosition + Random.insideUnitCircle * Random.Range(1, 2);

                Instantiate(m_EnemiesToSpawn[totalEnemiesSpawned].Enemy, spawnPosition, Quaternion.identity);
                totalEnemiesSpawned++;
                yield return new WaitForSeconds(0.3f);
            }

            float timeBtwSpawns = Random.Range(m_MinTimeBtwSpawns, m_MaxTimeBtwSpawn);
            timeBtwSpawns *= m_EnemiesPerGroup / m_MaxEnemiesPerGroup;

            yield return new WaitForSeconds(timeBtwSpawns);
        }
    }

    private Vector2 GetRandomSpawnPosition()
    {
        float spawnY = Random.Range
                (Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).y, Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height)).y);
        float spawnX = Random.Range
            (Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).x, Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x);

        Vector2 spawnPosition = new Vector2(spawnX, spawnY);

        return spawnPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_WaveCompleted == false)
        {
            if(m_EnemiesToKill <= 0)
            {
                m_WaveCompleted = true;
            }
        }
    }

    public void OnEnemyDeath()
    {
        m_EnemiesToKill -= 1;
    }

    public bool IsWaveCompleted()
    {
        return m_WaveCompleted;
    }
}
