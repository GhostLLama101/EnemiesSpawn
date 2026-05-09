using UnityEngine;
using Newtonsoft.Json.Linq;
public class damageAmp : Modifier
{
    public damageAmp(SpellCaster owner, SpellInfo spell) : base(owner, spell)
    {
        
    }
    public override void SetAttributes(JObject mod)
    {
        base.SetAttributes(mod);
    
        this.ModifierInfo.damage_multiplier = mod["damage_multiplier"].ToString();
        this.ModifierInfo.mana_multiplier = mod["mana_multiplier"].ToString();
    }
    
    // need to add an edit values for when the player gets stronger
}
