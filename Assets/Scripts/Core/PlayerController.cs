using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;
using static RPNEvaluator.RPNEvaluator;

public class PlayerController : MonoBehaviour
{
    public Hittable hp;
    public HealthBar healthui;
    public ManaBar manaui;

    public SpellCaster spellcaster;
    public SpellUI spellui;

    public int speed;

    public Unit unit;
    
    public bool dead = false;

    public bool scaling = false;

    public int power = 10;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        unit = GetComponent<Unit>();
        GameManager.Instance.player = gameObject;
    }

    public void StartLevel()
    {
        spellcaster = new SpellCaster(125, 8, Hittable.Team.PLAYER);
        StartCoroutine(spellcaster.ManaRegeneration());
        
        hp = new Hittable(100, Hittable.Team.PLAYER, gameObject);
        hp.OnDeath += Die;
        hp.team = Hittable.Team.PLAYER;

        // tell UI elements what to show
        healthui.SetHealth(hp);
        manaui.SetSpellCaster(spellcaster);
        spellui.SetSpell(spellcaster.spell);
    }

    // Update is called once per frame
    void Update()
    {
        //if (dead) Die();
        //condition for scaling the player
        if (GameManager.Instance.state == GameManager.GameState.INWAVE)
        {
            scaling = false;
        }
        if (GameManager.Instance.state == GameManager.GameState.WAVEEND && !scaling)
        {
            ScalePlayer();
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

    void OnMove(InputValue value)
    {
        if (GameManager.Instance.state == GameManager.GameState.PREGAME || GameManager.Instance.state == GameManager.GameState.GAMEOVER) return;
        unit.movement = value.Get<Vector2>()*speed;
    }

    void Die()
    {
        Debug.Log("You Lost");
        GameManager.Instance.state = GameManager.GameState.GAMEOVER;
    }

    void ScalePlayer()
    {   
        scaling = true;
        //on WaveEnd; call this function
        //Will be upgrading player max hp (scale current hp value to fit new max)
        //Ex: 80/100 = 80%, new max is 120, new hp is 80% of 120
        int cur_hp = hp.hp;
        int max_hp = hp.max_hp;
        float ratio = (float)cur_hp/max_hp;

        Dictionary<string, int> dictForRPN = new Dictionary<string, int>();
        dictForRPN["wave"] = currentWave;
        //hp
        hp.max_hp = Evaluate("95 wave 5 * +", dictForRPN);
        hp.hp = (int)(hp.max_hp*ratio);
        //mana and mana regen
        spellcaster.max_mana = Evaluate("90 wave 10 * +", dictForRPN);
        spellcaster.mana_reg = Evaluate("10 wave +", dictForRPN);
        //player power
        power = Evaluate("wave 10 *", dictForRPN);
    }

}
