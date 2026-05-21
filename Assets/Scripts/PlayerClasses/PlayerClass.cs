using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class PlayerClass
{
    public int sprite;
    public string health;
    public string mana;
    public string mana_regeneration;
    public string spellpower;
    public string speed;

    public PlayerClass Duplicate()
    {
        PlayerClass newClass = new PlayerClass();
        newClass.sprite = this.sprite;
        newClass.health = this.health;
        newClass.mana = this.mana;
        newClass.mana_regeneration = this.mana_regeneration;
        newClass.spellpower = this.spellpower;
        newClass.speed = this.speed;

        return newClass;
    }
}