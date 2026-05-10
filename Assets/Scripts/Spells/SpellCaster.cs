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
    List<Modifier> modifiers = new List<Modifier>();
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
        //spell = new Spell(this, GameManager.Instance.SpellsDict["arcane_bolt"]);
        
        SpellBuilder builder = new SpellBuilder();
        Spell core = new ArcaneBolt(this);

        // Create a doubler modifier — inner gets set by Build(), so pass null for now
        ModifierInfo doublerInfo = new ModifierInfo();
        doublerInfo.delay = 0.5f;
        doubler d = new doubler(this, doublerInfo, core); // inner will be overwritten by Build

        modifiers.Add(d);
        spell = builder.Build(core, modifiers);
        
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
    public void AddModifier(SpellInfo spellInfo, Modifier mod)
    {
        modifiers.Add(mod);
        RebuildSpell(spellInfo);
    }

    public void RemoveModifier(SpellInfo spellInfo, Modifier mod)
    {
        modifiers.Remove(mod);
        RebuildSpell(spellInfo);
    }

    public void RebuildSpell(SpellInfo spellInfo)
    {
        SpellBuilder builder = new SpellBuilder();
        spell = builder.Build(new ArcaneBolt(this), modifiers); // already correct, just make sure modifiers isn't stale
    }
    

}
