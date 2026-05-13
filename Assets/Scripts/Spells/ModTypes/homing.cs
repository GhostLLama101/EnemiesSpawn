using UnityEngine;
using Newtonsoft.Json.Linq;
using Unity.VisualScripting;
using System.Collections.Generic;
using static RPNEvaluator.RPNEvaluator;
public class homing : Modifier
{

    public homing(SpellCaster owner, Spell inner, ModifierInfo spell = null) : base(owner, spell, inner)
    {
        this.owner = owner;
        this.inner = inner;
        this.ModifierInfo = new ModifierInfo();
        ModifierInfo.name = "homing";
        ModifierInfo.description = 
        "The spell's projectile now homes in on the nearest target. Damage is decreased. Mana cost is increased.";
        ModifierInfo.damage_multiplier = " 9 * 10 /";
        ModifierInfo.mana_adder = " 5 +";
        ModifierInfo.projectile_trajectory = "homing";
    }
    public override void SetAttributes(JObject mod = null)
    {
        inner.spellInfo.damage.amount = inner.spellInfo.damage.amount+
        this.ModifierInfo.damage_multiplier;
        inner.spellInfo.mana_cost = inner.spellInfo.mana_cost+
        this.ModifierInfo.mana_adder;
        inner.spellInfo.projectile.trajectory = this.ModifierInfo.projectile_trajectory;
    }
    
    // need to add an edit values for when the player gets stronger
}
