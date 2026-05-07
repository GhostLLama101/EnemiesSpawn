using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

[System.Serializable] 
public class Spell 
{
    // add the spell shit 
    public string name;
    public string description;
    public int icon;
    public Damage damage;
    public string mana_cost;
    public string cooldown;

    public Projectile projectile;

    /*public class projectile
    {
        // the stuff
    }*/
    
    public float last_cast;
    public SpellCaster owner;
    public Hittable.Team team;

    public Spell(SpellCaster owner)
    {
        this.owner = owner;
    }

    public string GetName()
    {
        return this.name;
    }

    public int GetManaCost()
    {
        return 10;
    }

    public int GetDamage()
    {
        return 100;
    }



    public float GetCooldown()
    {
        return 0.75f;
    }

    public virtual int GetIcon()
    {
        return this.icon;
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
        if (other.team != team)
        {
            other.Damage(new Damage(100, damage.TypeFromString("physical")));//make a wrapper for damage here
        }

    }

}
