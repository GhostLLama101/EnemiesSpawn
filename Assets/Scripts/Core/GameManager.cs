using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;

public class GameManager 
{
    public enum GameState
    {
        PREGAME,
        INWAVE,
        PAUSE,
        WAVEEND,
        COUNTDOWN,
        GAMEOVER
    }
    public GameState state;

    public int countdown;
    private static GameManager theInstance;
    public static GameManager Instance {  get
        {
            if (theInstance == null) 
                theInstance = new GameManager();
            
            theInstance.InitalizeHelpers();
            return theInstance;
        }
    }

    public GameObject player;
    public ProjectileManager projectileManager;
    public SpellIconManager spellIconManager;
    public EnemySpriteManager enemySpriteManager;
    public PlayerSpriteManager playerSpriteManager;
    public RelicIconManager relicIconManager;
    public PlayerClassSelectorController playerClassSelectorController;
    public EnemySpawner enemySpawner;

    public bool AddedSpellpower = false;
    public int total_damage_dealt = 0;
    public float totalDistance = 0f;
    public float distFor10Relic = 0f;
    
    public int SkillPoints = 0;

    private List<GameObject> enemies;

    //Loads the level definitions from the JSON file
    public List<LevelClass> levelDefs = JSONReader.Load<LevelClass>("levels");    
    public LevelClass currentLevel;
    
    //Loads the level definitions from the JSON file
    public List<EnemyClass> enemyDefs = JSONReader.Load<EnemyClass>("enemies"); //Loads the enemy definitions from the JSON file

    //Time to make a dictionary for the enemies and levels for easy access
    public Dictionary<string, EnemyClass> enemyDict = new Dictionary<string, EnemyClass>();
    public Dictionary<string, LevelClass> levelDict = new Dictionary<string, LevelClass>();

    public Dictionary<string, SpellInfo> SpellsDict = JSONReader.LoadDictionary<SpellInfo>("spells");
    public List<string> spellKeys;
    public Dictionary<string, ModifierInfo> ModDict = JSONReader.LoadDictionary<ModifierInfo>("modifier");
    public Dictionary<string, PlayerClass> PlayerClasses = JSONReader.LoadDictionary<PlayerClass>("classes");
    public List<RelicInfo> RelicList = JSONReader.Load<RelicInfo>("relics");
    public Dictionary<string, RelicInfo> Relics;

    public int enemy_count { get { return enemies.Count; } }

    public int wave_count = 0;
    public PlayerClass playerClass;

    public void AddEnemy(GameObject enemy)
    {
        enemies.Add(enemy);
    }
    public void RemoveEnemy(GameObject enemy)
    {
        enemies.Remove(enemy);
    }

    public void ResetEnemyCount()
    {
        enemies.Clear();
    }

    public GameObject GetClosestEnemy(Vector3 point)
    {
        if (enemies == null || enemies.Count == 0) return null;
        if (enemies.Count == 1) return enemies[0];
        return enemies.Aggregate((a,b) => (a.transform.position - point).sqrMagnitude < (b.transform.position - point).sqrMagnitude ? a : b);
    }
    
    

    public void RegisterDamage(int amount)
    {
        total_damage_dealt += amount;
    }
    
    public void AddSkillPoint()
    {
        SkillPoints++;
        //Debug.Log("AddSkillPoint" + SkillPoints);
    }

    public void RemoveSkillPoint()
    {
        if (SkillPoints >= 0) return;
        SkillPoints--;
    }

    private GameManager()
    {
        enemies = new List<GameObject>();
    }

    private void InitalizeHelpers()
    {
        foreach (EnemyClass enemy in enemyDefs)
        {
            enemyDict[enemy.name] = enemy;
        }

        foreach (LevelClass level in levelDefs)
        {
            levelDict[level.name] = level;
        }

        spellKeys = new List<string>(SpellsDict.Keys);

        Relics = RelicList.ToDictionary(
            relic => relic.name,   
            relic => relic         
        );
    }
    
}
