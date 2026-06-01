using System.Collections;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class octupled : Modifier
{
    public octupled(SpellCaster owner, ModifierInfo spell, Spell inner) : base(owner, spell, inner)
    {
        
    }

    public override void SetAttributes(JObject attributes)
    {
        base.SetAttributes(attributes);
        ModifierInfo.angle = attributes["angle"].ToString();
        ModifierInfo.mana_multiplier = attributes["mana_multiplier"].ToString();
        ModifierInfo.count = attributes["count"].ToObject<int>();
    }

    public override IEnumerator Cast(Vector3 where, Vector3 target, Hittable.Team team)
    {
        this.team = team;
        int angle = int.Parse(ModifierInfo.angle);
        
        //Debug.Log($"count:{count}");
        for (int i = 0; i < ModifierInfo.count; i++)
        {
            yield return new WaitForSeconds(ModifierInfo.delay);
            GameManager.Instance.projectileManager.StartCoroutine(inner.Cast(where, Quaternion.Euler(0, 0, angle * i) * (target - where), team));
        }
     
        yield return new WaitForEndOfFrame();
    }
}
