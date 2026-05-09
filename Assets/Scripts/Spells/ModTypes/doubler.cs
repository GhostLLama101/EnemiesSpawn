using Newtonsoft.Json.Linq;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using static RPNEvaluator.RPNEvaluator;

public class doubler : Modifier
{
    
    
    public doubler(SpellCaster owner, SpellInfo spell) : base(owner, spell)
    {
        
    }
    
    Dictionary<string, int> dictForRPN = new Dictionary<string, int>();
    public override void SetAttributes(JObject mod)
    {
        base.SetAttributes(mod);
        this.ModifierInfo.delay = mod["delay"].ToObject<float>();
        
    }

    public virtual IEnumerator Cast(Vector3 where, Vector3 target, Hittable.Team team)
    {
        this.team = team;
        GameManager.Instance.projectileManager.CreateProjectile(0, spellInfo.projectile.trajectory, where, 
            target - where, Evaluatef(spellInfo.projectile.speed, dictForRPN), OnHit);
        yield return new WaitForSeconds(ModifierInfo.delay); // this is where the delay goes
        //create the projectile
        GameManager.Instance.projectileManager.CreateProjectile(0, spellInfo.projectile.trajectory, where, 
            target - where, Evaluatef(spellInfo.projectile.speed, dictForRPN), OnHit);
    }
}
