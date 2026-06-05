using UnityEngine;
using Newtonsoft.Json.Linq;
using System.Collections;

public class chaotic : Modifier
{
    //private bool casted = false;
    public chaotic(SpellCaster owner, ModifierInfo spell, Spell inner) : base(owner, spell, inner)
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
            Debug.LogError("chaotic: SetAttributes received a null JObject!");
            return;
        }

        if (this.ModifierInfo != null)
        {
            this.ModifierInfo.name = attributes["name"]?.ToString() ?? "Chaotic";
            this.ModifierInfo.description = attributes["description"]?.ToString() ?? "";
            this.ModifierInfo.damage_multiplier = attributes["damage_multiplier"]?.ToString() ?? "0";
            this.ModifierInfo.projectile_trajectory = attributes["projectile_trajectory"]?.ToString() ?? "spiraling";
        }
    }
    public override void ApplyModStats()
    {
        this.ModifierInfo.damage_multiplier = " 3 2 / wave 5 / + *";
        this.ModifierInfo.projectile_trajectory = "spiraling";

        this.spellInfo.damage.amount += this.ModifierInfo.damage_multiplier;
        this.spellInfo.projectile.trajectory = this.ModifierInfo.projectile_trajectory;
    }
}
