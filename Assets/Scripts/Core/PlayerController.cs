using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;
using static RPNEvaluator.RPNEvaluator;
using UnityEngine.Tilemaps;
//using Microsoft.VisualStudio.Editor;

public class PlayerController : MonoBehaviour
{
    public Hittable hp;
    public HealthBar healthui;
    public ManaBar manaui;

    public SpellCaster spellcaster;
    public SpellUI spellui; // this is the spell

    public int speed;

    public Unit unit;
    
    public bool dead = false;

    public bool scaling = false;
    public PlayerClass playerClass;
    public Dictionary<string, int> RPNDict = new Dictionary<string, int>();
    
    private Coroutine _notMoveCoroutine;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        unit = GetComponent<Unit>();
        GameManager.Instance.player = gameObject;
    }

    public void StartLevel()
    {
        //Make a deep copy cus why not
        playerClass = GameManager.Instance.playerClass.Duplicate();
        RPNDict["wave"] = GameManager.Instance.wave_count;

        //MANA
        int mana = Evaluate(playerClass.mana, RPNDict);
        int mana_reg = Evaluate(playerClass.mana_regeneration, RPNDict);

        spellcaster = new SpellCaster(mana, mana_reg, Hittable.Team.PLAYER);
        StartCoroutine(spellcaster.ManaRegeneration());
        
        //HEALTH
        int health = Evaluate(playerClass.health, RPNDict);
        hp = new Hittable(health, Hittable.Team.PLAYER, gameObject);
        hp.OnDeath += Die;
        hp.team = Hittable.Team.PLAYER;

        //SPEED
        speed = Evaluate(playerClass.speed, RPNDict);

        //UI
        //SPRITE
        GameManager.Instance.playerSpriteManager.PlaceSprite(playerClass.sprite,
                            GameManager.Instance.playerSpriteManager.image);
        //OTHER
        healthui.SetHealth(hp);
        manaui.SetSpellCaster(spellcaster);
        spellui.SetSpell(spellcaster.spells[spellcaster.current_spell]);
        
        DontMoveRelic dontMoveRelic = new DontMoveRelic(); // for testing
        KillEnemyRelic killedTheEnemy = new KillEnemyRelic(this); // for testing
        TakeDamageMana tookDamageMana = new TakeDamageMana(); // for testing
        MoveGainSpellpower moveGainSpellpower = new MoveGainSpellpower();
        SpellsGivePower spellsGivePower = new SpellsGivePower();


    }

    // Update is called once per frame
    void Update()
    {
        //Scaling the player
        if (GameManager.Instance.state == GameManager.GameState.WAVEEND)
        {
            scaling = false;
        }
        if (GameManager.Instance.state == GameManager.GameState.COUNTDOWN && !scaling)
        {
            Debug.Log("Scaling");
            ScalePlayer();
        }

        //Player spell switching
        //Should be robust
        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            spellcaster.current_spell = 0;
        }
        else if (Keyboard.current.digit2Key.wasPressedThisFrame && spellcaster.spells.Count > 1)
        {
            spellcaster.current_spell = 1;
        }
        else if (Keyboard.current.digit3Key.wasPressedThisFrame && spellcaster.spells.Count > 2)
        {
            spellcaster.current_spell = 2;
        }
        else if (Keyboard.current.digit4Key.wasPressedThisFrame && spellcaster.spells.Count > 3)
        {
            spellcaster.current_spell = 3;
        }
        
        
    }

    void OnAttack(InputValue value)
    {
        if (GameManager.Instance.state == GameManager.GameState.PREGAME || GameManager.Instance.state == GameManager.GameState.GAMEOVER) return;
        Vector2 mouseScreen = Mouse.current.position.value;
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(mouseScreen);
        mouseWorld.z = 0;
        StartCoroutine(spellcaster.Cast(transform.position, mouseWorld));
    }
    //totalDistance += distance;
    void OnMove(InputValue value) // add this to the observation
    {
        if (GameManager.Instance.state == GameManager.GameState.PREGAME || GameManager.Instance.state == GameManager.GameState.GAMEOVER) return;
        Vector2 movement = value.Get<Vector2>();
        
        unit.movement = movement *speed;
        
        
        if (movement.sqrMagnitude > 0.01f)
        {
            
            if (_notMoveCoroutine != null)
            {
                StopCoroutine(_notMoveCoroutine);
                _notMoveCoroutine = null;
            }
        }
        else
        {
            if (_notMoveCoroutine == null)
            {
                _notMoveCoroutine = StartCoroutine(NotMovingTimer());
            }
        }
        
    }

    void Die()
    {
        Debug.Log("You Lost");
        GameManager.Instance.state = GameManager.GameState.GAMEOVER;
    }

    void ScalePlayer()
    {   
        scaling = true; //scale only once please
        RPNDict["wave"] = GameManager.Instance.wave_count;

        //hp
        hp.SetMaxHP(Evaluate(playerClass.health, RPNDict));

        //mana and mana regen
        spellcaster.max_mana = Evaluate(playerClass.mana, RPNDict);
        spellcaster.mana_reg = Evaluate(playerClass.mana_regeneration, RPNDict);

        //player power
        spellcaster.power = Evaluate(playerClass.spellpower, RPNDict);
        //Player speed
        speed = Evaluate(playerClass.speed, RPNDict);

        //now update UI(s)
        healthui.SetHealth(hp);
        manaui.SetSpellCaster(spellcaster);

        //Let the world know
        EventBus.Instance.DoOnScaledPlayer();
    }

    private IEnumerator NotMovingTimer()
    {
        yield return new WaitForSeconds(3f);
        _notMoveCoroutine = null;
        Debug.Log("firing event DoNotMove");
        EventBus.Instance.DoNotMove();
    }
}
