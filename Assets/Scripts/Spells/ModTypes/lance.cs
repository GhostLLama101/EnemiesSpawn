using UnityEngine;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;

public class lance : Modifier 
{
    public lance(SpellCaster owner, ModifierInfo spell, Spell inner) : base(owner, spell, inner)
    {
            

    }
    /*public override void SetAttributes(JObject mod)
    {
        base.SetAttributes(mod);
        this.spellInfo.damage.type = mod["type"].ToObject<string>();
    }
    
    public override void ApplyModStats()
    {
        this.ModifierInfo.pierce = "true";
        
        this.spellInfo.damage.type = this.ModifierInfo.pierce;
    }*/
    /*public override Damage.Type GetDamageType()
    {
        return inner.Damage.Type.PIERCE;
    }*/
    public override IEnumerator Cast(Vector3 where, Vector3 target, Hittable.Team team)
    {
        inner.spellInfo.damage.type = "pierce";
        yield return inner.Cast(where, target, team);
    }
}
