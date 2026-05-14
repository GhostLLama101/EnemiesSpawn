using UnityEngine;
using static RPNEvaluator.RPNEvaluator;
using Newtonsoft.Json.Linq;

public class speedAmp : Modifier
{
    public speedAmp(SpellCaster owner, ModifierInfo spell, Spell inner) : base(owner, spell, inner)
    {
        this.owner = owner;
        this.inner = inner;
        this.ModifierInfo = spell;
        this.spellInfo = inner.spellInfo;
    }
    
    public override void SetAttributes(JObject mod)
    {
        base.SetAttributes(mod);
        this.ModifierInfo.speed_multiplier= mod["speed_multiplier"].ToString();
    }
    
    public override float GetSpeed()
    {
        return inner.GetSpeed() * EvaluateStat(ModifierInfo.speed_multiplier);
    }
    public override void ApplyModStats()
    {
        this.ModifierInfo.speed_multiplier = " 7 * 4 /";

        this.spellInfo.projectile.speed += this.ModifierInfo.speed_multiplier;
        this.spellInfo.secondary_projectile.speed += this.ModifierInfo.speed_multiplier;
    }
}
