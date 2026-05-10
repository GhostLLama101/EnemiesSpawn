using Newtonsoft.Json.Linq;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using static RPNEvaluator.RPNEvaluator;

public class doubler : Modifier
{
    
    
    public doubler(SpellCaster owner, ModifierInfo spell, Spell inner) : base(owner, spell, inner)
    {
        
    }
    
    
    public override void SetAttributes(JObject mod)
    {
        base.SetAttributes(mod);
        this.ModifierInfo.delay = mod["delay"].ToObject<float>();
        
    }

    public override IEnumerator Cast(Vector3 where, Vector3 target, Hittable.Team team)
    {
        this.team = team;
        yield return inner.Cast(where, target, team);
        yield return new WaitForSeconds(ModifierInfo.delay);
        yield return inner.Cast(where, target, team);
        yield return new WaitForEndOfFrame();
    }
}
