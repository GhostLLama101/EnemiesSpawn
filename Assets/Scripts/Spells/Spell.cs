using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using static RPNEvaluator.RPNEvaluator;
using Unity.Mathematics;


public class Spell 
{
    Dictionary<string, int> dictForRPN = new Dictionary<string, int>(); //used for RPN; 
    // overwriting power from owner whenever we need it in our get Methods
    
    public List<Modifier> modifiers = new List<Modifier>(); // this might be the solution for the stuff
    
    public float last_cast;
    public SpellCaster owner;
    public Hittable.Team team;
    public SpellInfo spellInfo;    
    public Spell() { }

    public Spell(SpellCaster owner, SpellInfo spellInfo)
    {
        this.owner = owner;
        this.spellInfo = spellInfo.Duplicate(); //makes a deep copy of the old spellinfo
    }
    

    public virtual string GetName()
    {
        return this.spellInfo.name;
    }
    public virtual string GetDescription()
    {
        return this.spellInfo.description;
    }

    public virtual int GetManaCost()
    {
        this.dictForRPN["power"] = owner.power;
        return Evaluate(this.spellInfo.mana_cost, this.dictForRPN);
    }

    public virtual int GetDamage()
    {
        this.dictForRPN["power"] = owner.power;
        return Evaluate(this.spellInfo.damage.amount, this.dictForRPN);
    }

    public virtual Damage.Type GetDamageType()
    {
        this.dictForRPN["power"] = owner.power;
        return Damage.TypeFromString(this.spellInfo.damage.type);
    }

    public virtual float GetCooldown()
    {
        this.dictForRPN["power"] = owner.power;
        return Evaluatef(this.spellInfo.cooldown, this.dictForRPN);
    }
    public virtual float GetSpeed()
    {
        this.dictForRPN["power"] = owner.power;
        return Evaluatef(this.spellInfo.projectile.speed, this.dictForRPN);
    }

    public virtual float GetLifeTime()
    {
        this.dictForRPN["power"] = owner.power;
        return int.Parse(spellInfo.projectile.lifetime);
    }

    public virtual int GetIcon()
    {
        return this.spellInfo.icon;
    }

    public bool IsReady()
    {
        return (last_cast + GetCooldown() < Time.time);
    }
    

    public List<Modifier> GetModifiers()
    {
        return this.modifiers;
    }

    public virtual IEnumerator Cast(Vector3 where, Vector3 target, Hittable.Team team)
    {
        this.team = team;
        this.dictForRPN["power"] = owner.power;
        GameManager.Instance.projectileManager.CreateProjectile(
            spellInfo.projectile.sprite,
            spellInfo.projectile.trajectory, 
            where, 
            target - where, 
            GetSpeed(),
            OnHit,
            GetLifeTime());
        //GetDamageType() == Damage.Type.PIERCE
        yield return new WaitForEndOfFrame();
    }

    public virtual void OnHit(Hittable other, Vector3 impact)
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
    protected float EvaluateStat(string expression)
    {
        this.dictForRPN["power"] = owner.power;
        return Evaluatef(expression, dictForRPN);
    }
    


}
