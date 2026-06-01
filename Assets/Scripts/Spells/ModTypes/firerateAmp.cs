using System.Collections;
using UnityEngine;
using Newtonsoft.Json.Linq;
using TMPro;

public class firerateAmp : Modifier
{
    public firerateAmp(SpellCaster owner, ModifierInfo spell, Spell inner) : base(owner, spell, inner)
    {
        
    }
    public override void SetAttributes(JObject mod)
    {
        base.SetAttributes(mod);

        this.ModifierInfo.damage_multiplier = mod["damage_multiplier"].ToString();
        this.ModifierInfo.cooldown_multiplier = mod["cooldown_multiplier"].ToString();
    }


    public override void ApplyModStats()
    {
        this.ModifierInfo.damage_multiplier = "0.75";
        this.ModifierInfo.cooldown_multiplier = "0.5";

        inner.spellInfo.damage.amount += this.ModifierInfo.damage_multiplier;
        inner.spellInfo.cooldown += this.ModifierInfo.cooldown_multiplier;

    }

    public override IEnumerator Cast(Vector3 where, Vector3 target, Hittable.Team team)
    {
        this.team = team;
        yield return inner.Cast(where, target, team);
    }
}
