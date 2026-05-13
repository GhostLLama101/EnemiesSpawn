using UnityEngine;
using Newtonsoft.Json.Linq;
using Unity.VisualScripting;
using System.Collections.Generic;
using static RPNEvaluator.RPNEvaluator;
public class homing : Modifier
{

    public homing(SpellCaster owner, Spell inner, ModifierInfo spell = null) : base(owner, spell, inner)
    {
        
    }
    public override void SetAttributes(JObject attributes)
    {
        base.SetAttributes(attributes);
        ModifierInfo.name = attributes["name"].ToString();
        ModifierInfo.description = attributes["description"].ToString();
        ModifierInfo.damage_multiplier = attributes["damage_multiplier"].ToString();
        ModifierInfo.mana_adder = attributes["mana_adder"].ToString();
        ModifierInfo.projectile_trajectory = attributes["projectile_trajectory"].ToString();
    }

    public override IEnumerator Cast(Vector3 where, Vector3 target, Hittable.Team team)
    {
        this.team = team;

        inner.spellInfo.damage.amount += this.ModifierInfo.damage_multiplier;
        inner.spellInfo.mana_cost += this.ModifierInfo.mana_adder;
        inner.spellInfo.projectile.trajectory = this.ModifierInfo.projectile_trajectory;

        yield return inner.Cast(where, target, team);
    }
    /* To Go in cast
        
    */
    
}
