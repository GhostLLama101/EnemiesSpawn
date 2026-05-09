using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using static RPNEvaluator.RPNEvaluator;
using Unity.Mathematics;

[Serializable]
public class Modifier : Spell
{
    public ModifierInfo ModifierInfo;
    Dictionary<string, int> dictForRPN = new Dictionary<string, int>();
    public Modifier (SpellCaster owner, SpellInfo spell) : base(owner, spell) {

    }
    
    // add the getters
    
    public virtual IEnumerator Cast(Vector3 where, Vector3 target, Hittable.Team team)
    {
        this.team = team;
        GameManager.Instance.projectileManager.CreateProjectile(
            0,//spellInfo.icon, 
            spellInfo.projectile.trajectory, 
            where, 
            target - where, 
            Evaluatef(spellInfo.projectile.speed, dictForRPN), 
            OnHit);
        yield return new WaitForEndOfFrame();
    }
    
    public void OnHit(Hittable other, Vector3 impact)
    {
        if (other.team != team)
        {
            other.Damage(new Damage(this.GetDamage(), this.GetDamageType()));
        }

    }
    public override void SetAttributes(JObject mod)
    {
        this.ModifierInfo.name = mod["name"].ToString();
        this.ModifierInfo.description = mod["description"].ToString();
    }
    
    
}
