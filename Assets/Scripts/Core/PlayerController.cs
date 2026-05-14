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
    public SpellUI spellui; // this is the spell

    public int speed;

    public Unit unit;
    
    public bool dead = false;

    public bool scaling = false;

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
        spellui.SetSpell(spellcaster.spells[spellcaster.current_spell]);
    }

    // Update is called once per frame
    void Update()
    {
        //Scaling the player
        if (GameManager.Instance.state == GameManager.GameState.INWAVE)
        {
            scaling = false;
        }
        if (GameManager.Instance.state == GameManager.GameState.WAVEEND && !scaling)
        {
            ScalePlayer();
            healthui.SetHealth(hp);
        }

        //Player spell switching
        //Should be robust
        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            Debug.Log("1");
            spellcaster.current_spell = 0;
            Debug.Log("Selected spell: " + spellcaster.spells[0].GetName());
           // spellui.SetSpell(spellcaster.spells[spellcaster.current_spell]);
        }
        else if (Keyboard.current.digit2Key.wasPressedThisFrame && spellcaster.spells.Count > 1)
        {
            Debug.Log("2");
            spellcaster.current_spell = 1;
            Debug.Log("Selected spell: " + spellcaster.spells[1].GetName());
            //spellui.SetSpell(spellcaster.spells[spellcaster.current_spell]);
        }
        else if (Keyboard.current.digit3Key.wasPressedThisFrame && spellcaster.spells.Count > 2)
        {
            Debug.Log("3");
            spellcaster.current_spell = 2;
            Debug.Log("Selected spell: " + spellcaster.spells[2].GetName());
            //spellui.SetSpell(spellcaster.spells[spellcaster.current_spell]);
        }
        else if (Keyboard.current.digit4Key.wasPressedThisFrame && spellcaster.spells.Count > 3)
        {
            Debug.Log("4");
            spellcaster.current_spell = 3;
            Debug.Log("Selected spell: " + spellcaster.spells[3].GetName());
           // spellui.SetSpell(spellcaster.spells[spellcaster.current_spell]);
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
        scaling = true; //scale only once please
        Dictionary<string, int> dictForRPN = new Dictionary<string, int>();
        dictForRPN["wave"] = GameManager.Instance.wave_count;

        //hp
        hp.SetMaxHP(Evaluate("95 wave 5 * +", dictForRPN));

        //mana and mana regen
        spellcaster.max_mana = Evaluate("90 wave 10 * +", dictForRPN);
        spellcaster.mana_reg = Evaluate("10 wave +", dictForRPN);

        //player power
        spellcaster.power = Evaluate("wave 10 *", dictForRPN);

        //spellcaster.FillSpells();

        //now update UI(s)
        //TODO:
        spellui.SetSpell(spellcaster.spells[0]);// use this for all spells when we have the UI set up
        //or find a way to do it in the loop
        healthui.SetHealth(hp);
        manaui.SetSpellCaster(spellcaster);

    }

}
