using System.Collections;
using UnityEngine;

public class splitter : Modifier
{
    public splitter(SpellCaster owner, ModifierInfo spell, Spell inner) : base(owner, spell, inner)
    {
        
    }

    public override IEnumerator Cast(Vector3 where, Vector3 target, Hittable.Team team)
    {
        return base.Cast(where, target, team);
    }
}
