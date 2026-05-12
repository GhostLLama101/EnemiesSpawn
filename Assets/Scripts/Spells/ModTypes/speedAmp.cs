using UnityEngine;
using Newtonsoft.Json.Linq;

public class speedAmp : Modifier
{
    public speedAmp(SpellCaster owner, ModifierInfo spell, Spell inner) : base(owner, spell, inner)
    {
        
    }
    
    public override void SetAttributes(JObject mod)
    {
        base.SetAttributes(mod);
    
        this.ModifierInfo.damage_multiplier = mod["damage_multiplier"].ToString();
        this.ModifierInfo.mana_multiplier = mod["mana_multiplier"].ToString();
    }
}
