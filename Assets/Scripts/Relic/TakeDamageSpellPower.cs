using UnityEngine;

public class TakeDamageSpellPower : RelicInfo
{
    public TakeDamageSpellPower()
    {
        Debug.Log("Added OnTakeDamageMana to event bus");
        EventBus.Instance.OnTakeDamageSP += OnTakeDamageSP;
    }

    private void OnTakeDamageSP(Hittable target)
    {
        Debug.Log("You took damage you get 100 spellPower");
    }
}
