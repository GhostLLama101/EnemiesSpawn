using UnityEngine;
using static RPNEvaluator.RPNEvaluator;
using Newtonsoft.Json.Linq;

public class mine : Modifier
{
    public mine(SpellCaster owner, ModifierInfo spell, Spell inner) : base(owner, spell, inner)
    {
        this.owner = owner;
        this.inner = inner;
        this.ModifierInfo = spell;
        this.spellInfo = inner.spellInfo;
    }
    
    public override void SetAttributes(JObject mod)
    {
        base.SetAttributes(mod);
        this.ModifierInfo.damage_multiplier = mod["damage_multiplier"].ToString();
        this.ModifierInfo.speed_multiplier= mod["speed_multiplier"].ToString();
    }
    
    
    public override void ApplyModStats()
    {
        inner.spellInfo.projectile.lifetime = "999";

        this.ModifierInfo.damage_multiplier = " 5 *";
        inner.spellInfo.damage.amount += this.ModifierInfo.damage_multiplier;

        this.ModifierInfo.speed_multiplier = " 0 *";

        this.spellInfo.projectile.speed += this.ModifierInfo.speed_multiplier;
        if (this.spellInfo.secondary_projectile != null) 
        {
            this.spellInfo.secondary_projectile.speed += this.ModifierInfo.speed_multiplier;
        }
    }
}
