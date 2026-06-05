using System.Collections;
using UnityEngine;
using Newtonsoft.Json.Linq;
using TMPro;

public class splitter : Modifier
{
    public splitter(SpellCaster owner, ModifierInfo spell, Spell inner) : base(owner, spell, inner)
    {
        
    }
    public override void SetAttributes(JObject attributes) {
        base.SetAttributes(attributes);
        ModifierInfo.angle = attributes["angle"].ToString();
        ModifierInfo.mana_multiplier = attributes["mana_multiplier"].ToString();
      
    }

    public override IEnumerator Cast(Vector3 where, Vector3 target, Hittable.Team team)
    {
        this.team = team;
        Vector3 direction = (target - where).normalized;
        int angle = int.Parse(ModifierInfo.angle);
        
        Vector3 target1 = where + (Quaternion.Euler(0, 0, angle) * direction);
        Vector3 target2 = where + (Quaternion.Euler(0, 0, -angle) * direction);
        
        GameManager.Instance.projectileManager.StartCoroutine(inner.Cast(where, target1, team));
        GameManager.Instance.projectileManager.StartCoroutine(inner.Cast(where, target2, team));

        yield break;
    }
}
