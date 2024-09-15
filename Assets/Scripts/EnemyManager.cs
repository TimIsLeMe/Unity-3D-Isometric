using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private EnemyCharacter[] enemies;
    [SerializeField] private int difficultyInterval = 20; // every x seconds 1 more enemy spawns
    // Update is called once per frame
    private float start;
    private void Start()
    {
        start = Time.realtimeSinceStartup;
    }
    void Update()
    {
        if(FindAnyObjectByType<EnemyCharacter>() == null)
        {
            SpawnEnemies();
        }
    }

    public void SpawnEnemies()
    {
        int bonus = (int) start % difficultyInterval;
        for(int i = 0; i < enemies.Length; i++)
        {
            int max = Random.Range(1 + bonus, 4 + bonus);
            Debug.Log(max);
            for(int j = 0; j < max + 1; j++)
            {
                Instantiate(enemies[i], new Vector3(j * 3, 1, j * 3), Quaternion.identity);
            }
        }
    }
}
