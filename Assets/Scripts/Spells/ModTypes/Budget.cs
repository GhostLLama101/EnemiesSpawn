using System.Collections;
using UnityEngine;
using Newtonsoft.Json.Linq;

public class Budget : Modifier
{
    public Budget(SpellCaster owner, ModifierInfo spell, Spell inner) : base(owner, spell, inner)
    {
        

    }
    public override void SetAttributes(JObject mod)
    {
        base.SetAttributes(mod);
        this.ModifierInfo.mana_multiplier = mod["mana_multiplier"].ToString();
    }
    
    public override void ApplyModStats()
    {
        this.ModifierInfo.mana_multiplier = " 2 /";
        inner.spellInfo.mana_cost += this.ModifierInfo.mana_adder;
    }
    
    public override IEnumerator Cast(Vector3 where, Vector3 target, Hittable.Team team)
    {
        this.team = team;
        yield return inner.Cast(where, target, team);
    } 
}
