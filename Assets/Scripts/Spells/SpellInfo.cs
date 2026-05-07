using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpellInfo
{
    public string name = "Arcane Bolt";
    public string description = "A straight-flying bolt.";
    public int icon = 0;
    public SpellDamage damage;
    public string mana_cost = "10";
    public string cooldown = "2";
    public Projectile projectile;
}