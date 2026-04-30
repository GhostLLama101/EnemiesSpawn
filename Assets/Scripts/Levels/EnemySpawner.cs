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



public class EnemySpawner : MonoBehaviour
{
    public Image level_selector;
    public GameObject button;
    public GameObject enemy;
    public SpawnPoint[] SpawnPoints; 
    Dictionary<string, Enemy> enemy_types = new Dictionary<string, Enemy>(); 
    Dictionary<string, Level> level_types = new Dictionary<string, Level>(); 
    public string currentLevelname;
    private int wave_count;
    public int delay = 2;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LoadEnemyType();
        LoadLevelType(); 
        // loop through levels and add a button for each difficulty
        int buttonUIOffset = 0;
        foreach (var kvp in level_types)
        {
            GameObject selector = Instantiate(button, level_selector.transform);
            selector.transform.localPosition = new Vector3(0, 030 + buttonUIOffset);
            selector.GetComponent<MenuSelectorController>().spawner = this;
            selector.GetComponent<MenuSelectorController>().SetLevel(kvp.Key);
            buttonUIOffset -= 50;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartLevel(string levelname)
    {
        wave_count = 1;
        currentLevelname = levelname;
        
        level_selector.gameObject.SetActive(false);
        // this is not nice: we should not have to be required to tell the player directly that the level is starting
        GameManager.Instance.player.GetComponent<PlayerController>().StartLevel();
        Debug.Log($"Starting level: {currentLevelname}");
        StartCoroutine(SpawnWave()); // I feel like we should pass the levelname to SpawnWave()
    }

    public void NextWave()
    {
        wave_count++;
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
        
        Level currentLevel = level_types[currentLevelname];         // sets the current level type
        for (int i = 0; i < currentLevel.spawns.Count; i++)                // this spawns the stuff 
        {
            
            Spawn spawn = currentLevel.spawns[i];
            Enemy enemy_data = enemy_types[spawn.enemy];
            
            SetPerameters parameters =  new SetPerameters // saves the parameters to the builder class to get later.
            {
                type = spawn.enemy,
                hp = Evaluate(spawn.hp, new Dictionary<string, int> {{ "base", enemy_data.hp }, { "wave", wave_count }}),
                damage = Evaluate(spawn.damage ?? "base" , new Dictionary<string, int> {{ "base", enemy_data.damage }, { "wave", wave_count }}),
                speed = Evaluate(enemy_data.speed.ToString(), new Dictionary<string, int>{{ "base", enemy_data.speed }, { "wave", wave_count }}),
                delay = currentLevel.spawns[i].delay,
                location = currentLevel.spawns[i].location,
                
            };
            int count = Evaluate(spawn.count, new Dictionary<string, int> { { "wave", wave_count } });
            if (count <= 0) count = 1;
            for (int index = 0; index < count; index++)
            {
                Debug.unityLogger.Log(spawn.enemy);
                yield return SpawnEnemy(parameters);
            }
        }
        yield return new WaitWhile(() => GameManager.Instance.enemy_count > 0);
        GameManager.Instance.state = GameManager.GameState.WAVEEND;
    }

    IEnumerator SpawnEnemy(SetPerameters parameters)                                // going to need to add the other perimeters like 
    {
        // get the spawn point offset
        Vector2 offset = Random.insideUnitCircle * 1.8f;

        // get actual spawn point from parameters
        SpawnPoint spawn_point = null;
        if (!string.IsNullOrEmpty(parameters.location))
        {
            // try to find ALL spawn points where the 'kind' enum matches the JSON string
            SpawnPoint[] matchingSpawns = System.Array.FindAll(SpawnPoints, sp => sp.kind.ToString().ToUpper() == parameters.location.ToUpper());
            
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

        Vector3 initial_position = spawn_point.transform.position + new Vector3(offset.x, offset.y, 0);
        GameObject new_enemy = Instantiate(enemy, initial_position, Quaternion.identity);
        
        Enemy data = enemy_types[parameters.type];                                   // get the name of the enemy to are makeing
        new_enemy.GetComponent<SpriteRenderer>().sprite = GameManager.Instance
                                     .enemySpriteManager.Get(data.sprite);           // assign the sprite of the name
        new_enemy.GetComponent<EnemyController>().SetParameters(parameters);         // assign the contoller to the name and parameters
                                                        // function in enemycontroller
        GameManager.Instance.AddEnemy(new_enemy);                                    // creat the enemy in the game
        yield return new WaitForSeconds(parameters.delay == 0 ? 2 : parameters.delay);// this probably where the delay is going to go
    }
    
    
    public void LoadEnemyType()
    {
        
        var enemytext = Resources.Load<TextAsset>("enemies");   // this loads the enemies files
        JToken jo = JToken.Parse(enemytext.text);
        foreach (var enemy in jo)
        {
            Enemy en = enemy.ToObject<Enemy>();
            enemy_types[en.name] = en;
        }
        
        // Debug print
        foreach (var kvp in enemy_types)
        {
            //Debug.Log($"Enemy: {kvp.Key} | Health: {kvp.Value.hp} | Speed: {kvp.Value.speed} | Sprite: {kvp.Value.sprite} | Damage: {kvp.Value.damage}");
            //variables.Add("base", kvp.Value.hp);
            
        }
    }

    public void LoadLevelType()
    {
        var levelstext = Resources.Load<TextAsset>("levels");
        JToken jo = JToken.Parse(levelstext.text);
        foreach (var levelIterator in jo)
        {
            Level level = levelIterator.ToObject<Level>();
            level_types[level.name] = level;
        }
        
        foreach (var kvp in level_types)
        {
            Level level = kvp.Value;
            Debug.Log($"=== LEVEL: {level.name} | Waves: {level.waves} | Total Spawns: {level.spawns.Count} ===");
            
        }
    }
}