using UnityEngine;
using Newtonsoft.Json.Linq;
using System.Collections;

public class homing : Modifier
{
    //private bool casted = false;
    public homing(SpellCaster owner, ModifierInfo spell, Spell inner) : base(owner, spell, inner)
    {
        this.owner = owner;
        this.inner = inner;
        this.ModifierInfo = spell;
        this.spellInfo = inner.spellInfo;

        
    }

    public override void SetAttributes(JObject attributes)
    {
        base.SetAttributes(attributes);

        if (attributes == null)
        {
            Debug.LogError("homing: SetAttributes received a null JObject!");
            return;
        }

        if (this.ModifierInfo != null)
        {
            this.ModifierInfo.name = attributes["name"]?.ToString() ?? "Homing";
            this.ModifierInfo.description = attributes["description"]?.ToString() ?? "";
            this.ModifierInfo.damage_multiplier = attributes["damage_multiplier"]?.ToString() ?? "0";
            this.ModifierInfo.mana_adder = attributes["mana_adder"]?.ToString() ?? "0";
            this.ModifierInfo.projectile_trajectory = attributes["projectile_trajectory"]?.ToString() ?? "homing";
        }
    }
    public override void ApplyModStats()
    {
        this.ModifierInfo.damage_multiplier = " 3 * 4 /";
        this.ModifierInfo.mana_adder = " 10 +";
        this.ModifierInfo.projectile_trajectory = "homing";

        this.spellInfo.damage.amount += this.ModifierInfo.damage_multiplier;
        this.spellInfo.mana_cost += this.ModifierInfo.mana_adder;
        this.spellInfo.projectile.trajectory = this.ModifierInfo.projectile_trajectory;
    }
}
