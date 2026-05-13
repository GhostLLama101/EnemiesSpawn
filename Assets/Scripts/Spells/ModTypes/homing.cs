using UnityEngine;
using Newtonsoft.Json.Linq;
using Unity.VisualScripting;
using System.Collections;
using static RPNEvaluator.RPNEvaluator;
public class homing : Modifier
{

    public homing(SpellCaster owner,  ModifierInfo spell, Spell inner) : base(owner, spell, inner)
    {
        this.owner = owner;
        this.inner = inner;
        this.ModifierInfo = spell;
    }
    public override void SetAttributes(JObject attributes)
    {
        base.SetAttributes(attributes);
        this.ModifierInfo.name = attributes["name"].ToString();
        this.ModifierInfo.description = attributes["description"].ToString();
        this.ModifierInfo.damage_multiplier = attributes["damage_multiplier"].ToString();
        this.ModifierInfo.mana_adder = attributes["mana_adder"].ToString();
        this.ModifierInfo.projectile_trajectory = attributes["projectile_trajectory"].ToString();
    }

    public override IEnumerator Cast(Vector3 where, Vector3 target, Hittable.Team team)
    {
        this.team = team;

        inner.spellInfo.damage.amount += this.ModifierInfo.damage_multiplier;
        inner.spellInfo.mana_cost += this.ModifierInfo.mana_adder;
        inner.spellInfo.projectile.trajectory = "homing";

        yield return inner.Cast(where, target, team);
    }    
}
