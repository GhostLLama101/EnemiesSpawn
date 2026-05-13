using System;
using UnityEngine;

[System.Serializable]
public class Projectile 
{
    public string trajectory = "straight";
    public string speed = "8 power 5 / +";
    public int sprite = 0;
    public string lifetime = "-1";

    public Projectile Duplicate()
    {
        Projectile newProj = new Projectile();
        newProj.trajectory = this.trajectory;
        newProj.speed = this.speed;
        newProj.sprite = this.sprite;
        newProj.lifetime = this.lifetime;
        return newProj;
    }
}
