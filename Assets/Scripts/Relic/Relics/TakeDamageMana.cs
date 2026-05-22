using System;
using UnityEngine;

public class TakeDamageMana : RelicInfo
{
    public TakeDamageMana()
    {
        Debug.Log("Added OnTakeDamageMana to event bus");
        EventBus.Instance.OnTakeDamage += OnTakeDamage;
    }

    private void OnTakeDamage(Hittable target)
    {
        Debug.Log("You took damage you get 5 mana");
    }
}
