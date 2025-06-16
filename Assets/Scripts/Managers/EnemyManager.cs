using UnityEngine;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour, ISaveable
{
    public PlayerHealth playerHealth;
    public GameObject enemy;
    public float spawnTime = 3f;
    public Transform[] spawnPoints;
    private List<GameObject> spawnedEnemies = new List<GameObject>();

    void Start()
    {
        if (SaveManager.Instance.HasSaveFile())
        {
            ClearExistingEnemies();
        }
        InvokeRepeating("Spawn", spawnTime, spawnTime);
    }

    void ClearExistingEnemies()
    {
        var existingEnemies = FindObjectsOfType<EnemyHealth>();
        foreach (var enemyHealth in existingEnemies)
        {
            if (enemyHealth.gameObject != null)
            {
                Destroy(enemyHealth.gameObject);
            }
        }
    }

    void Spawn()
    {
        if (playerHealth.CurrentHealth <= 0f)
            return;

        int spawnPointIndex = Random.Range(0, spawnPoints.Length);
        GameObject newEnemy = Instantiate(enemy, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
        spawnedEnemies.Add(newEnemy);
    }

    public void Save(SaveData saveData)
    {
        // Nothing needed here, each enemy saves itself
    }

    public void Load(SaveData saveData)
    {
        ClearExistingEnemies();

        foreach (var enemyData in saveData.enemyData)
        {
            GameObject newEnemy = Instantiate(enemy, enemyData.position.ToVector3(), enemyData.rotation.ToQuaternion());
            var enemyHealth = newEnemy.GetComponent<EnemyHealth>();
            enemyHealth.SetEnemyId(enemyData.enemyId);
            newEnemy.SetActive(enemyData.isActive);
        }
    }
}
