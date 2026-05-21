using UnityEngine;

public class TakeDamageSpellPower : RelicInfo
{
    PlayerController player;
    public TakeDamageSpellPower(PlayerController player)
    {
        Debug.Log("Added OnTakeDamageMana to event bus");
        this.player = player;
        EventBus.Instance.OnTakeDamageSP += OnTakeDamageSP;
        //EventBus.Instance.OnSpellCasted += OnSpellCasted;
    }

    private void OnTakeDamageSP(Hittable target)
    {
        Debug.Log("You took damage you get 100 spellPower");
        Effects.AddSpellPower(100, player);
        GameManager.Instance.AddedSpellpower = true;
    }

}
