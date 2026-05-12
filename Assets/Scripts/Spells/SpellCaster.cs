using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpellCaster 
{
    public int mana;
    public int max_mana;
    public int mana_reg;
    public Hittable.Team team;
    public List<Spell> spells;
    //public List<List<Modifier>> modsOfSpells;
    
    public int current_spell = 0;
    public int maxSpellCount = 4;
    public int power = 10;
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

        
        core = new ArcaneBolt(this);
        
        spells = new List<Spell> { SpellBuilder.Build(core, core.GetModifiers()) }; // getMod return dictionary
    }
    
    public void AddSpell(Spell spell) // just call addSpell and it should replace if 4 max
    {
        if (spells.Count < maxSpellCount)
            spells.Add(spell);
        else
            spells[current_spell] = spell;
    }
    
    public void RebuildSpell(Spell spell)
    {
        int index = spells.IndexOf(spell);
        Spell freshCore = new ArcaneBolt(this);
        freshCore.modifiers = spell.modifiers;
        spells[index] = SpellBuilder.Build(freshCore, freshCore.GetModifiers());
    }

    public IEnumerator Cast(Vector3 where, Vector3 target)
    {
        Spell spell = spells[current_spell];
        if (mana >= spell.GetManaCost() && spell.IsReady())
        {
            mana -= spells[current_spell].GetManaCost();
            yield return spells[current_spell].Cast(where, target, team);
        }
        yield break;
    }
}
