using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class SpellCaster 
{
    public int mana;
    public int max_mana;
    public int mana_reg;
    public Hittable.Team team;
    public int maxSpellCount = 4;
    public List<Spell> spells = new List<Spell>();
    public int current_spell = 0;
    
    public int power = 10; //starting power
    public Spell core;
    
    public IEnumerator ManaRegeneration()
    {
        while (true)
        {
            mana += mana_reg;
            mana = Mathf.Min(mana, max_mana);
            yield return new WaitForSeconds(1);
        }
    }
    
    public SpellCaster(int mana, int mana_reg, Hittable.Team team)
    {
        this.mana = mana;
        this.max_mana = mana;
        this.mana_reg = mana_reg;
        this.team = team;

        //SpellBuilder builder = new SpellBuilder();
        core = new Spell(this, GameManager.Instance.SpellsDict["arcane_bolt"]);
        spells.Add(core);
    }
    
    public bool IsFull() => spells.Count >= maxSpellCount;

    public void AddSpell(Spell spell)
    {
        if (!IsFull())
            spells.Add(spell);
    }
    
    public void RebuildSpell(Spell spell)
    {
        int index = spells.IndexOf(spell);
        Spell freshCore = new Spell(this, spell.spellInfo);
        freshCore.modifiers = spell.modifiers;
        spells[index] = SpellBuilder.Build(freshCore, freshCore.GetModifiers());
    }

    public IEnumerator Cast(Vector3 where, Vector3 target)
    {        
        Spell spell = new Spell(this, spells[current_spell].spellInfo);
        if (mana >= spell.GetManaCost() && spell.IsReady())
        {
            mana -= spells[current_spell].GetManaCost();
            yield return spells[current_spell].Cast(where, target, team);
        }
        yield break;
    }
}
