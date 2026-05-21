using UnityEngine;
using System;

public class EventBus 
{
    private static EventBus theInstance;
    public static EventBus Instance
    {
        get
        {
            if (theInstance == null)
                theInstance = new EventBus();
            return theInstance;
        }
    }

    public event Action<Vector3, Damage, Hittable> OnDamage;

    public event Action OnNotMove;  // add more events that can happen like not moving for 3 seconds

    public event Action OnEnemyKilled;// killing and enemy

    public event Action <Hittable>OnTakeDamage;// Whenever you take damage, you gain 5 mana.
    public event Action<Hittable> OnTakeDamageSP;

    public event Action OnSpellCasted;
    
    // When you take damage, your next spell gets 100 spellpower.
    public event Action OnMove;

    public event Action OnReceiveSpell;

    public event Action OnScaledPlayer;


    public void DoDamage(Vector3 where, Damage dmg, Hittable target)
    {
        OnDamage?.Invoke(where, dmg, target);
    }
    public void DoNotMove() // if the event is active do it
    {
        //Debug.Log("Invoking onNotMove");
        OnNotMove?.Invoke(); 
    }

    public void DoKilledEnemy()
    {
        Debug.Log("Invoking OnEnemyKilled");
        OnEnemyKilled?.Invoke();
    }

    public void DoTakeDamage(Hittable target)
    {
        Debug.Log("Invoking OnTakeDamage");
        OnTakeDamage?.Invoke(target);
        OnTakeDamageSP?.Invoke(target);
    }

    public void DoSpellCasted()
    {
        Debug.Log("Invoking OnSpellCasted");
        OnSpellCasted?.Invoke();
    }
    public void DoOnMove() // if the event is active do it
    {
        //Debug.Log("Invoking onMove");
        OnMove?.Invoke(); 
    }
    public void DoOnReceiveSpell()
    {
        Debug.Log("Invoking OnReceiveSpell");
        OnReceiveSpell?.Invoke(); 
    }
    public void DoOnScaledPlayer()
    {
        Debug.Log("Invoking OnScaledPlayer");
        OnScaledPlayer?.Invoke(); 
    }
    
    // need to do spell power next

}
