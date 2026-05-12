using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using static RPNEvaluator.RPNEvaluator;
using Unity.Mathematics;


public class Spell 
{
    Dictionary<string, int> dictForRPN = new Dictionary<string, int>();
    
    public List<Modifier> modifiers = new List<Modifier>(); // this might be the solution for the stuff
    
    public float last_cast;
    public SpellCaster owner;
    public Hittable.Team team;
    public SpellInfo spellInfo;
    
    public Spell() { }
    //TODO: change the get methods to dynamically pull the player's info, not just on creation
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

    public virtual int GetManaCost()
    {
        return Evaluate(this.spellInfo.mana_cost, this.dictForRPN);
    }

    public virtual int GetDamage()
    {
        return Evaluate(this.spellInfo.damage.amount, this.dictForRPN);
    }

    public Damage.Type GetDamageType()
    {
        return Damage.TypeFromString(this.spellInfo.damage.type);
    }

    public virtual float GetCooldown()
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
    
    public void AddModifier(Modifier mod)
    {
        modifiers.Add(mod);
        Rebuild();
    }

    public void RemoveModifier(Modifier mod)
    {
        modifiers.Remove(mod);
        Rebuild();
    }

    private void Rebuild()
    {
        // tell the owner to rebuild this spell
        owner.RebuildSpell(this);
    }

    public List<Modifier> GetModifiers()
    {
        return this.modifiers;
    }

    public virtual IEnumerator Cast(Vector3 where, Vector3 target, Hittable.Team team)
    {
        this.team = team;
        GameManager.Instance.projectileManager.CreateProjectile(
            spellInfo.projectile.sprite,
            spellInfo.projectile.trajectory, 
            where, 
            target - where, 
            Evaluatef(spellInfo.projectile.speed, dictForRPN), 
            OnHit);
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
    


}
