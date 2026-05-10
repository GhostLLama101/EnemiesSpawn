using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpellCaster 
{
    public int mana;
    public int max_mana;
    public int mana_reg;
    public Hittable.Team team;
    public Spell spell;
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
        core = new ArcaneBolt(this);
        spell = builder.Build(core, core.GetModifiers());
    }

    public void AddModifier(Modifier mod)
    {
        spell.modifiers.Add(mod);
        RebuildSpell();
    }

    public void RemoveModifier(Modifier mod)
    {
        spell.modifiers.Remove(mod);
        RebuildSpell();
    }

    public void RebuildSpell()
    {
        SpellBuilder builder = new SpellBuilder();
        core = new ArcaneBolt(this); // TODO need to change
        spell = builder.Build(core, core.GetModifiers());
    }

    public IEnumerator Cast(Vector3 where, Vector3 target)
    {        
        if (mana >= spell.GetManaCost() && spell.IsReady())
        {
            mana -= spell.GetManaCost();
            yield return spell.Cast(where, target, team);
        }
        yield break;
    }
}
