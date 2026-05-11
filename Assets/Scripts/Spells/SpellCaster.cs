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
    public List<List<Modifier>> modsOfSpells;
    public int current_spell = 0;
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

        SpellBuilder builder = new SpellBuilder();
        core = new Spell(this, GameManager.Instance.SpellsDict["arcane_bolt"]);
        spells[0] = builder.Build(core, modsOfSpells[0]);
    }

    public void AddModifier(Modifier mod)
    {
        spells[current_spell].modifiers.Add(mod);
        RebuildSpell();
    }

    public void RemoveModifier(Modifier mod)
    {
        spells[current_spell].modifiers.Remove(mod);
        RebuildSpell();
    }

    public void RebuildSpell()
    {
        SpellBuilder builder = new SpellBuilder();
        core = new ArcaneBolt(this); // TODO need to change
        spells[current_spell] = builder.Build(core, core.GetModifiers());
    }

    public IEnumerator Cast(Vector3 where, Vector3 target)
    {        
        Spell spell = new Spell(this, spells[current_spell]);
        if (mana >= spell.GetManaCost() && spell.IsReady())
        {
            mana -= spells[current_spell].GetManaCost();
            yield return spells[current_spell].Cast(where, target, team);
        }
        yield break;
    }
}
