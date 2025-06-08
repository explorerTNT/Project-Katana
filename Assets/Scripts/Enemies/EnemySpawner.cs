using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;      // ������ �����
    public Transform[] spawnPoints;     // ������ ����� ������
    public float spawnInterval = 5f;    // �������� ������ (�� ������������ ��� ������� ������)

    void Start()
    {
        if (enemyPrefab == null || spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogWarning("EnemySpawner is not fully configured. Check enemyPrefab and spawnPoints.");
        }
        // �������������� ����� �������� ��� �������� �������
        // InvokeRepeating("SpawnEnemy", 0f, spawnInterval);
    }

    public void SpawnEnemy()
    {
        if (enemyPrefab == null)
        {
            Debug.LogWarning("Cannot spawn enemy: enemyPrefab is not assigned.");
            return;
        }
        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogWarning("Cannot spawn enemy: no spawn points assigned.");
            return;
        }
        int randomIndex = Random.Range(0, spawnPoints.Length);
        Instantiate(enemyPrefab, spawnPoints[randomIndex].position, Quaternion.identity);
    }

    public void SpawnAtPosition(Vector3 position)
    {
        if (enemyPrefab != null)
        {
            Instantiate(enemyPrefab, position, Quaternion.identity);
            Debug.Log($"Spawned new enemy at {position}");
        }
        else
        {
            Debug.LogWarning("Cannot spawn enemy: enemyPrefab is not assigned.");
        }
    }
}