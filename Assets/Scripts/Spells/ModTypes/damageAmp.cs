using UnityEngine;
using Newtonsoft.Json.Linq;
using System.Collections;
public class damageAmp : Modifier
{
    //private bool casted = false;
    public damageAmp(SpellCaster owner, ModifierInfo spell, Spell inner) : base(owner, spell, inner)
    {
        this.ModifierInfo.damage_multiplier = "3 * 2 /";
        this.ModifierInfo.mana_multiplier = " 3 * 2 /";

        inner.spellInfo.damage.amount += this.ModifierInfo.damage_multiplier;
        inner.spellInfo.mana_cost += this.ModifierInfo.mana_adder;

    }
    public override void SetAttributes(JObject mod)
    {
        base.SetAttributes(mod);
    
        this.ModifierInfo.damage_multiplier = mod["damage_multiplier"].ToString();
        this.ModifierInfo.mana_multiplier = mod["mana_multiplier"].ToString();
    }
    
    public override int GetDamage()
    {
        return (int)(inner.GetDamage() * EvaluateStat(ModifierInfo.damage_multiplier));
        
    }

    public override int GetManaCost()
    {
        return (int)(inner.GetManaCost() * EvaluateStat(ModifierInfo.mana_multiplier));
    }
    
    public override IEnumerator Cast(Vector3 where, Vector3 target, Hittable.Team team)
    {
        this.team = team;

        
        

        yield return inner.Cast(where, target, team);
    }    
}
