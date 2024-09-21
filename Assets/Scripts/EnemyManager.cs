using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private EnemyCharacter[] enemies;
    [SerializeField] private int[] enemieAmountMod = new int[] {1, 3}; // needs atleast same length as enemies
    [SerializeField] private int upperSpawnBound = 2;
    [SerializeField] private float difficultyIncrease = 1.25f;
    [SerializeField] private float timeToSpawn = 15;
    [SerializeField] private float spawnCountdown = 0;
    [SerializeField] private int startingWave = 1;
    [SerializeField] private EnemySpawnpoint[] spawnpoints;
    private int wave = 1;
    private float timeCounter;
    private void Start()
    {
        wave = startingWave;
        if (enemieAmountMod.Length < enemies.Length)
        {
            throw new UnityException("Unspecified enemy spawn amount in 'enemieAmountMod' with length of: " +
            enemieAmountMod.Length +
            "\n When length should be atleast that of 'enemies': " + enemies.Length);
        }
    }
    void Update()
    {
        timeCounter += Time.deltaTime;
        spawnCountdown -= Time.deltaTime;
        if (spawnCountdown <= 0)
        {
            spawnCountdown = timeToSpawn;
            wave++;
            TriggerSpawnpoints();
        }
    }

    public void TriggerSpawnpoints()
    {
        int[] enemyAmount = new int[enemies.Length];
        int amnt = (int) (wave * difficultyIncrease) + 1;
        for(int i = 0; i < enemies.Length; i++)
        {
            int max = UnityEngine.Random.Range(amnt, upperSpawnBound + amnt) * enemieAmountMod[i];
            enemyAmount[i] = max;
        }
        int[] partialEnemyAmount = new int[spawnpoints.Length];
        for (int i = 0; i < enemyAmount.Length; i++)
        {
            partialEnemyAmount[i] = Mathf.Max(enemyAmount[i] / spawnpoints.Length, 1);
        }
        Debug.Log(partialEnemyAmount[0] + ", " + partialEnemyAmount[1]);
        foreach (EnemySpawnpoint spawnpoint in spawnpoints) 
        {
            spawnpoint.SpawnEnemies(enemies, partialEnemyAmount);
        }
    }
}
