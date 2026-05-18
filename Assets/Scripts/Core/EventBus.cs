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
   
    // killing and enemy
    // damaging an enemy
    
    public void DoDamage(Vector3 where, Damage dmg, Hittable target)
    {
        OnDamage?.Invoke(where, dmg, target);
    }
    public void DoNotMove() // if the event is active do it
    {
        Debug.Log("Invoking onNotMove");
        OnNotMove?.Invoke(); 
    }

}
