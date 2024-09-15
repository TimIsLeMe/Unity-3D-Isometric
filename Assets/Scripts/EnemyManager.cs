using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private EnemyCharacter[] enemies;
    [SerializeField] private int difficultyInterval = 20; // every x seconds 1 more enemy spawns
    [SerializeField] private float timeToSpawn = 15;
    [SerializeField] private float spawnCountdown = 0;
    private float timeCounter;
    // Update is called once per frame
    private void Start()
    {
       
    }
    void Update()
    {
        timeCounter += Time.deltaTime;
        spawnCountdown -= Time.deltaTime;
        if (spawnCountdown <= 0)
        {
            spawnCountdown = timeToSpawn;
            SpawnEnemies();
        }
    }

    public void SpawnEnemies()
    {
        int bonus = (int) timeCounter % difficultyInterval;
        for(int i = 0; i < enemies.Length; i++)
        {
            int max = Random.Range(1 + bonus, 3 + bonus);
            Debug.Log(max);
            for(int j = 0; j < max; j++)
            {
                Instantiate(enemies[i], new Vector3(j * 3, 1, j * 3), Quaternion.identity);
            }
        }
    }
}
