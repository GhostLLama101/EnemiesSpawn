using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using static RPNEvaluator.RPNEvaluator;
using Unity.Mathematics;

[Serializable] 
public class Spell 
{
    Dictionary<string, int> dictForRPN = new Dictionary<string, int>();
    
    public float last_cast;
    public SpellCaster owner;
    public Hittable.Team team;
    public SpellInfo spellInfo;
    
    public Spell(SpellCaster owner, SpellInfo spell)
    {
        this.owner = owner;
        this.spellInfo = spell;
        this.dictForRPN["power"] = owner.power;
    }

    public string GetName()
    {
        return this.spellInfo.name;
    }

    public int GetManaCost()
    {
        return Evaluate(this.spellInfo.mana_cost, this.dictForRPN);
    }

    public int GetDamage()
    {
        return Evaluate(this.spellInfo.damage.amount, this.dictForRPN);
    }

    public Damage.Type GetDamageType()
    {
        return Damage.TypeFromString(this.spellInfo.damage.type);
    }

    public float GetCooldown()
    {
        //Debug.LogError($"this is spell: {this.spellInfo}");
        return Evaluatef(this.spellInfo.cooldown, this.dictForRPN);
    }

    public virtual int GetIcon()
    {
        return this.spellInfo.icon;
    }

    public bool IsReady()
    {
        return (last_cast + GetCooldown() < Time.time);
    }

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
    // need to make a cast that takes in a 
    public virtual IEnumerator Cast(Vector3 where, Vector3 target, Hittable.Team team, Vector2 temp)
    {
        yield return null;
    }
    

    public void OnHit(Hittable other, Vector3 impact)
    {
        if (other.team != team)
        {
            other.Damage(new Damage(this.GetDamage(), this.GetDamageType()));
        }

    }
    

    public virtual void SetAttributes(JObject spell)
    {
        this.spellInfo.name = spell["name"].ToString();
        this.spellInfo.description = spell["description"].ToString();
        this.spellInfo.icon = spell["icon"].ToObject<int>();
        this.spellInfo.mana_cost = spell["mana_cost"].ToString();
        this.spellInfo.cooldown = spell["cooldown"].ToString();
        //damage
        this.spellInfo.damage.type = spell["type"].ToString();
        this.spellInfo.damage.amount = spell["amount"].ToString();
        //projectile
        this.spellInfo.projectile.speed = spell["speed"].ToString();
        this.spellInfo.projectile.trajectory = spell["trajectory"].ToString();
        this.spellInfo.projectile.sprite = spell["sprite"].ToObject<int>();

    }

}
