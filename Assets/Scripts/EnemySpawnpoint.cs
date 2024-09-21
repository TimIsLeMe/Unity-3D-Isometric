using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class EnemySpawnpoint : MonoBehaviour
{
    [SerializeField] private int spawnDelay = 150;
    private float timeCounter;

    public void SpawnEnemies(EnemyCharacter[] enemies, int[] enemyAmount)
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            Debug.Log("Spawning: " + enemyAmount[i]);
            for (int j = 0; j < enemyAmount[i]; j++)
            {
                SpawnWithDelay(enemies[i], j, spawnDelay);
            }
        }
    }

    void SpawnWithDelay(EnemyCharacter ec, int j, float delayTime)
    {
        StartCoroutine(SpawnDelayed(ec, j, delayTime));
    }

    IEnumerator SpawnDelayed(EnemyCharacter ec, int j, float delayTime)
    {
        Debug.Log("spawning...");
        yield return new WaitForSeconds(delayTime / 1000);
        Instantiate(ec, transform.position + new Vector3(j, 1, j), Quaternion.identity);
        Debug.Log("SPAWNED");
    }
}
