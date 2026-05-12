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

        //if ()
        //SpellInfo mySpell = GameManager.Instance.SpellsDict["arcane_bolt"];
        //Debug.Log($"Damage: {mySpell.damage.amount}");

        // tell UI elements what to show
        healthui.SetHealth(hp);
        manaui.SetSpellCaster(spellcaster);
        spellui.SetSpell(spellcaster.spells[spellcaster.current_spell]);
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
            healthui.SetHealth(hp);
        }
        //TODO: if player switches spells, change what spell the UI has, also switch the spells
        //spellui.SetSpell(spellcaster.spells[spellcaster.current_spell]);
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
        Dictionary<string, int> dictForRPN = new Dictionary<string, int>();
        dictForRPN["wave"] = GameManager.Instance.wave_count;

        //hp; changed it because I can read instructions nowXD
        hp.SetMaxHP(Evaluate("95 wave 5 * +", dictForRPN));

        //mana and mana regen
        spellcaster.max_mana = Evaluate("90 wave 10 * +", dictForRPN);
        spellcaster.mana_reg = Evaluate("10 wave +", dictForRPN);

        //player power
        spellcaster.power = Evaluate("wave 10 *", dictForRPN);

        //overwrite the old spells with new ones based on new power
        for (int i = 0; i < spellcaster.spells.Count; i++)
        {
            Spell old = spellcaster.spells[i];
            Spell newSpell = new Spell(spellcaster, old.spellInfo);
            SpellBuilder.Build(newSpell, old.modifiers);

            spellcaster.spells[i] = newSpell;
        }
        //now update UI(s)
        spellui.SetSpell(spellcaster.spells[spellcaster.current_spell]);
        healthui.SetHealth(hp);
        manaui.SetSpellCaster(spellcaster);

    }

}
