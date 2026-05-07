using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using static RPNEvaluator.RPNEvaluator;

[System.Serializable] 
public class Spell 
{
    // add the spell shit 
    Dictionary<string, int> dictForRPN = new Dictionary<string, int>();
    /*public string name;
    public string description;
    public int icon;
    public SpellDamage damage;
    public string mana_cost;
    public string cooldown;

    public Projectile projectile;
    */

    /*public class projectile
    {
        // the stuff
    }*/
    
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
        return Evaluate(this.spellInfo.cooldown, this.dictForRPN);
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
        GameManager.Instance.projectileManager.CreateProjectile(0, "straight", where, target - where, 15f, OnHit);
        yield return new WaitForEndOfFrame();
    }

    void OnHit(Hittable other, Vector3 impact)
    {
        //Debug 1
        if (other == null)
        {
            Debug.LogError($"Other doesnt exist");
            return;
        }
        // Debug 2
        if (owner == null) {
            Debug.LogError($"Spell '{name}' is missing its owner! Did you assign it after loading JSON?");
            return;
        }
        //Debug 3
        if (this.spellInfo.damage == null) {
            Debug.LogError($"Spell '{name}' has no damage data! Check your JSON mapping.");
            return;
        }   

        if (other.team != team)
        {
            other.Damage(new Damage(this.GetDamage(), this.GetDamageType()));
        }

    }

}
