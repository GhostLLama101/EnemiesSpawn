using UnityEngine;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;
using System.Linq;
using Unity.Collections;

public class EnemySpawner : MonoBehaviour
{
    public Image level_selector;
    public GameObject button;
    public GameObject enemy;
    public SpawnPoint[] SpawnPoints; 
    Dictionary<string, Enemy> enemy_types = new Dictionary<string, Enemy>(); 
    Dictionary<string, Level> level_types = new Dictionary<string, Level>(); 
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject selector = Instantiate(button, level_selector.transform);
        selector.transform.localPosition = new Vector3(0, 130);
        selector.GetComponent<MenuSelectorController>().spawner = this;
        selector.GetComponent<MenuSelectorController>().SetLevel("Start");
        LoadEnemyType();
        LoadLevelType();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartLevel(string levelname)
    {
        level_selector.gameObject.SetActive(false);
        // this is not nice: we should not have to be required to tell the player directly that the level is starting
        GameManager.Instance.player.GetComponent<PlayerController>().StartLevel();
        
        LoadEnemyType(); // load the different types of enemies in the game
        
        StartCoroutine(SpawnWave());
    }

    public void NextWave()
    {
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
                                        // make all the enemies in memory and put in dictionary
                                        // this is where you check the diction and get   
        foreach (string name in enemy_types.Keys)    // this spawns the 
        {
            yield return SpawnEnemy(name);
            
        }
        yield return new WaitWhile(() => GameManager.Instance.enemy_count > 0);
        GameManager.Instance.state = GameManager.GameState.WAVEEND;
    }
    /*IEnumerator SpawnZombie()
    {
        SpawnPoint spawn_point = SpawnPoints[Random.Range(0, SpawnPoints.Length)];
        Vector2 offset = Random.insideUnitCircle * 1.8f;
                
        Vector3 initial_position = spawn_point.transform.position + new Vector3(offset.x, offset.y, 0);
        GameObject new_enemy = Instantiate(enemy, initial_position, Quaternion.identity);

        new_enemy.GetComponent<SpriteRenderer>().sprite = GameManager.Instance.enemySpriteManager.Get(0);
        EnemyController en = new_enemy.GetComponent<EnemyController>();
        en.hp = new Hittable(50, Hittable.Team.MONSTERS, new_enemy);
        en.speed = 10;
        GameManager.Instance.AddEnemy(new_enemy);
        yield return new WaitForSeconds(0.5f);
    }*/

    IEnumerator SpawnEnemy(string Enemy_name) // going to need to add the other perameters like 
    {
        // get the spawn point
        SpawnPoint spawn_point = SpawnPoints[Random.Range(0, SpawnPoints.Length)];
        Vector2 offset = Random.insideUnitCircle * 1.8f;
                
        Vector3 initial_position = spawn_point.transform.position + new Vector3(offset.x, offset.y, 0);
        GameObject new_enemy = Instantiate(enemy, initial_position, Quaternion.identity);
        
        
        // get the name of the enemy to are makeing
        Enemy data = enemy_types[Enemy_name];
        // assign the sprite of the name
        new_enemy.GetComponent<SpriteRenderer>().sprite = GameManager.Instance.enemySpriteManager.Get(data.sprite);
        // assign the contoller to the name
        EnemyController en = new_enemy.GetComponent<EnemyController>();
        // assign the health of the name
        en.hp = new Hittable(data.hp, Hittable.Team.MONSTERS, new_enemy);
        // assign the speed of the name
        en.speed = data.speed;
        
        
        en.damage = data.damage;
        
        // creat the enemy in the game
        GameManager.Instance.AddEnemy(new_enemy);
        yield return new WaitForSeconds(0.5f); // this probably where the delay is going to go;
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
            Debug.Log($"Enemy: {kvp.Key} | Health: {kvp.Value.hp} | Speed: {kvp.Value.speed} | Sprite: {kvp.Value.sprite} | Damage: {kvp.Value.damage}");
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
    }
}