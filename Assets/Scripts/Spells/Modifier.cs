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
    
    // add the getters
    public override string GetName()
    {
        return inner.GetName() + " " + this.ModifierInfo.name;
    }
    public override string GetDescription()
    {
        return inner.GetDescription() + " " + this.ModifierInfo.description;
    }
    
    public override IEnumerator Cast(Vector3 where, Vector3 target, Hittable.Team team)
    {
        yield return inner.Cast(where, target, team); // pass down the chain
    }
    
    public override void SetAttributes(JObject mod)
    {
        this.ModifierInfo.name = mod["name"].ToString();
        this.ModifierInfo.description = mod["description"].ToString();
    }
    
    
}
