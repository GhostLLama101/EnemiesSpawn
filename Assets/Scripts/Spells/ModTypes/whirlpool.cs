using System.Collections;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class whirlpool : Modifier
{
    public whirlpool(SpellCaster owner, ModifierInfo spell, Spell inner) : base(owner, spell, inner)
    {
        
    }

    public override void SetAttributes(JObject attributes)
    {
        base.SetAttributes(attributes);
        ModifierInfo.angle = attributes["angle"].ToString();
        ModifierInfo.mana_multiplier = attributes["mana_multiplier"].ToString();
        ModifierInfo.count = attributes["count"].ToObject<int>();
        ModifierInfo.cooldown_multiplier = attributes["cooldown_multiplier"].ToString();
        ModifierInfo.projectile_trajectory = attributes["projectile_trajectory"].ToString();

    }

    public override IEnumerator Cast(Vector3 where, Vector3 target, Hittable.Team team)
    {
        this.team = team;
        float angleOffset = float.Parse(ModifierInfo.angle); // optional extra rotation offset
        float angleStep = 360f / ModifierInfo.count;         // evenly divide the circle 
        Vector3 direction = (target - where).normalized;
        //Debug.Log($"where:{where}  target:{target}");

        for (int i = 0; i < ModifierInfo.count; i++)
        {
            yield return new WaitForSeconds(ModifierInfo.delay);
            float currentAngle = angleStep * i + angleOffset;
            Vector3 target1 = where + (Quaternion.Euler(0, 0,currentAngle) * direction);
            GameManager.Instance.projectileManager.StartCoroutine(inner.Cast(where, target1, team));
        }

        yield return new WaitForEndOfFrame();
    }
}
