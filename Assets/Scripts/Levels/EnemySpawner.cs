using UnityEngine;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;
using System.Linq;
using Unity.Collections;
using System.Runtime.CompilerServices;
using UnityEngine.VFX;
using static RPNEvaluator.RPNEvaluator;
using UnityEngine.SceneManagement;


public class EnemySpawner : MonoBehaviour
{
    public Image level_selector;
    public GameObject button;
    public GameObject enemy;
    public SpawnPoint[] SpawnPoints; 
    public GameObject rewardButton;
    Dictionary<string, EnemyClass> enemy_types;
    Dictionary<string, LevelClass> level_types;
    public string currentLevelname;
    //private int wave_count;
    public int delay = 2;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemy_types = GameManager.Instance.enemyDict;

        level_types = GameManager.Instance.levelDict;
        // loop through levels and add a button for each difficulty
        
        int totalLevels = level_types.Count;
        float spacing = 50f;
        float startY = ((totalLevels - 1) * spacing) / 2f;
        float currentY = startY;
        foreach (var kvp in level_types)
        {
            GameObject selector = Instantiate(button, level_selector.transform);
            selector.transform.localPosition = new Vector3(0, currentY, 0);
            selector.GetComponent<MenuSelectorController>().spawner = this;
            selector.GetComponent<MenuSelectorController>().SetLevel(kvp.Key);
            currentY -= spacing;
        }
        GameManager.Instance.enemySpawner = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartLevel(string levelname)
    {
        GameManager.Instance.wave_count = 1;
        currentLevelname = levelname;
        
        level_selector.gameObject.SetActive(false);
        // this is not nice: we should not have to be required to tell the player directly that the level is starting
        GameManager.Instance.player.GetComponent<PlayerController>().StartLevel();
        Debug.Log($"Starting level: {currentLevelname}");
        
        StartCoroutine(SpawnWave()); // I feel like we should pass the levelname to SpawnWave()
    }

    public void NextWave()
    {
        GameManager.Instance.wave_count++;
        StartCoroutine(SpawnWave());
    }
        IEnumerator SpawnWave()
    {
        GameManager.Instance.state = GameManager.GameState.COUNTDOWN; // This is for countdown till the next wave
        GameManager.Instance.countdown = 3;
        for (int i = 3; i > 0; i--)
        {
            yield return new WaitForSeconds(1);
            GameManager.Instance.countdown--;
        }
        GameManager.Instance.state = GameManager.GameState.INWAVE;
        Debug.Log("Starting Wave: " + GameManager.Instance.wave_count);

        LevelClass currentLevel = level_types[currentLevelname]; // sets the current level type

        // Keep track of how many active enemy types are currently spawning
        int activeSpawningRoutines = currentLevel.spawns.Count;

        // Start ALL enemy type spawning loops at the exact same time
        for (int i = 0; i < currentLevel.spawns.Count; i++)
        {
            Spawns spawn = currentLevel.spawns[i];
            
            // Fire off an independent sub-coroutine for this specific enemy type
            StartCoroutine(SpawnEnemyTypeSequence(spawn, currentLevel, () => {
                activeSpawningRoutines--; // Decrement counter when this specific enemy finishes all its batches
            }));
        }

        // Wait until EVERY single enemy type has finished firing its spawn batches
        yield return new WaitUntil(() => activeSpawningRoutines == 0);

        // Wait until the player actually clears the battlefield
        yield return new WaitWhile(() => GameManager.Instance.enemy_count > 0);
        GameManager.Instance.state = GameManager.GameState.WAVEEND;
    }

    // New helper coroutine to handle an individual enemy type's batch timing in parallel
    IEnumerator SpawnEnemyTypeSequence(Spawns spawn, LevelClass currentLevel, System.Action onComplete)
    {
        EnemyClass enemy_data = enemy_types[spawn.enemy];
        Dictionary<string, int> RPNDict = new Dictionary<string, int>();
        RPNDict["wave"] = GameManager.Instance.wave_count;

        SetPerameters parameters = new SetPerameters();
        parameters.type = spawn.enemy;
        RPNDict["base"] = enemy_data.hp;
        parameters.hp = Evaluate(spawn.hp, RPNDict);
        RPNDict["base"] = enemy_data.damage;
        parameters.damage = Evaluate(spawn.damage, RPNDict);
        RPNDict["base"] = enemy_data.speed;
        parameters.speed = Evaluate(enemy_data.speed.ToString(), RPNDict);
        RPNDict["base"] = 2; // default delay if not specified
        parameters.delay = Evaluate(spawn.delay.ToString(), RPNDict);
        parameters.location = spawn.location;
        RPNDict["base"] = 1; // default count if not specified
        int count = Evaluate(spawn.count, RPNDict);
        if (count <= 0) count = 1;

        int[] sequence = (spawn.sequence != null && spawn.sequence.Length > 0) ? spawn.sequence : new int[] { 1 };

        int sequenceIndex = 0;
        int spawnedSoFar = 0;

        while (spawnedSoFar < count)
        {
            int batchSize = sequence[sequenceIndex % sequence.Length];
            batchSize = Mathf.Min(batchSize, count - spawnedSoFar);

            for (int index = 0; index < batchSize; index++)
            {
                SpawnEnemy(parameters);
            }

            spawnedSoFar += batchSize;
            sequenceIndex++;

            if (spawnedSoFar < count)
            {
                float waitTime = parameters.delay == 0 ? 2f : parameters.delay;
                yield return new WaitForSeconds(waitTime);
            }
        }

        // Notify the main SpawnWave coroutine that this type is done spawning
        onComplete?.Invoke();
    }


    void SpawnEnemy(SetPerameters parameters)                                // going to need to add the other perimeters like 
    {

        SpawnPoint spawn_point = null;
        if (!string.IsNullOrEmpty(parameters.location))
        {
            SpawnPoint[] matchingSpawns = System.Array.FindAll(SpawnPoints, sp => 
                parameters.location.ToUpper().Contains(sp.kind.ToString().ToUpper())
            );
            if (matchingSpawns.Length > 0)
            {
                spawn_point = matchingSpawns[Random.Range(0, matchingSpawns.Length)];
            }
            else
            {
                // fallback: Check if the JSON used the exact GameObject name instead (e.g., "RedSpawnSouthWing")
                spawn_point = System.Array.Find(SpawnPoints, sp => sp.name == parameters.location);
            }
        }
        
        if (spawn_point == null)
        {
            spawn_point = SpawnPoints[Random.Range(0, SpawnPoints.Length)];
        }

        Debug.Log($"Spawning {parameters.type} at {spawn_point.name} | position: {spawn_point.transform.position}");

        Vector3 initial_position = spawn_point.GetRandomPosition();

        GameObject new_enemy = Instantiate(enemy, initial_position, Quaternion.identity);
        
        EnemyClass data = enemy_types[parameters.type];                                   // get the name of the enemy to are makeing
        new_enemy.GetComponent<SpriteRenderer>().sprite = GameManager.Instance
                                     .enemySpriteManager.Get(data.sprite);           // assign the sprite of the name
        new_enemy.GetComponent<EnemyController>().SetParameters(parameters);         // assign the contoller to the name and parameters
                                                        // function in enemycontroller
        GameManager.Instance.AddEnemy(new_enemy);                                    // creat the enemy in the game
    }
    
    
    public void RestartLevel()
    {
        GameManager.Instance.state = GameManager.GameState.PREGAME;
        StopAllCoroutines(); // stop SpawnWave from finishing
        GameManager.Instance.ResetEnemyCount();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}