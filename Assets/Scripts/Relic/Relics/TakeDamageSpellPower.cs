using System.Collections.Generic;
using UnityEngine;
using static RPNEvaluator.RPNEvaluator;

public class TakeDamageSpellPower : RelicBaseClass
{
    public TakeDamageSpellPower(PlayerController player) : base(player) { }

    /*protected override void Subscribe()
    {
        EventBus.Instance.OnTakeDamageSP += OnTakeDamageSP;
    }

    public override void Unsubscribe()
    {
        EventBus.Instance.OnTakeDamageSP -= OnTakeDamageSP;
    }

    private void OnTakeDamageSP(Hittable target)
    {
        int amount = Evaluate(Effects.GetAmount("Golden Mask"), RPNDict);
        Effects.AddSpellPower(amount, player);
        GameManager.Instance.AddedSpellpower = true;
    }*/

}
