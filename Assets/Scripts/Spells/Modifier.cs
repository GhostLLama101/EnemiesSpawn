using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using static RPNEvaluator.RPNEvaluator;
using Unity.Mathematics;

[Serializable]
public class Modifier : Spell
{
    public ModifierInfo ModifierInfo;
    public Spell inner; // whatever is wrapped inside this modifier
    public Modifier (SpellCaster owner, ModifierInfo mod, Spell inner) : base(owner, inner.spellInfo) {
        this.owner = owner;
        this.inner = inner;
        this.ModifierInfo = mod;
    }
    public virtual void ApplyModStats() { }
    // add the getters
    public override string GetName()
    {
        return inner.GetName() + " " + this.ModifierInfo.name;
    }
    public override string GetDescription()
    {
        return inner.GetDescription() + " " + this.ModifierInfo.description;
    }
    public override int GetDamage()
    {
        return inner.GetDamage();
        
    }
    public override float GetSpeed()
    {
        return inner.GetSpeed();
    }
    public override int GetManaCost()
    {
        return inner.GetManaCost();
    }

    public override float GetCooldown()
    {
        return inner.GetCooldown();
    }
    public override float GetLifeTime()
    {
        return inner.GetLifeTime();
    }
    
    public override Damage.Type GetDamageType()
    {
        return inner.GetDamageType();
    }
    
    public override IEnumerator Cast(Vector3 where, Vector3 target, Hittable.Team team)
    {
        this.team = team;
        yield return inner.Cast(where, target, team); // pass down the chain
    }
    
    public override void SetAttributes(JObject mod)
    {
        this.ModifierInfo.name = mod["name"].ToString();
        this.ModifierInfo.description = mod["description"].ToString();
    }

    public Spell AddModifier(SpellCaster owner ,Spell currentspell, string modName)
    {
        Modifier mod  = SpellBuilder.CreateModifier(owner, GameManager.Instance.ModDict[modName], currentspell);
        currentspell.modifiers.Add(mod);
        Spell changedSpell = SpellBuilder.Build(currentspell, GetModifiers());
        return changedSpell;
    }

    public Spell RemoveModifier( Spell currentspell, string modName)
    {
        Modifier toRemove = currentspell.modifiers.Find(m => m.ModifierInfo.name == modName);
        if (toRemove == null)
        {
            Debug.Log("modification Not found");
            return currentspell; // modifier not found, return unchanged
        }
        
        currentspell.modifiers.Remove(toRemove);
        Spell changedSpell = SpellBuilder.Build(currentspell, GetModifiers());
        return changedSpell;
    }
}
