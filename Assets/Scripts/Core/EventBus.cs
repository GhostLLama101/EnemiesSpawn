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

    public event Action OnTakeDamage;// Whenever you take damage, you gain 5 mana.
    
    // When you take damage, your next spell gets 100 spellpower.
    
    public event Action OnMove;
    public void DoDamage(Vector3 where, Damage dmg, Hittable target)
    {
        OnDamage?.Invoke(where, dmg, target);
    }
    public void DoNotMove() // if the event is active do it
    {
        Debug.Log("Invoking onNotMove");
        OnNotMove?.Invoke(); 
    }

    public void DoKilledEnemy()
    {
        Debug.Log("Invoking OnEnemyKilled");
        OnEnemyKilled?.Invoke();
    }

    public void DoTakeDamage()
    {
        Debug.Log("Invoking OnTakeDamage");
        OnTakeDamage?.Invoke();
    }
    public void DoOnMove() // if the event is active do it
    {
        Debug.Log("Invoking onMove");
        OnMove?.Invoke(); 
    }
    
    // need to do spell power next

}
