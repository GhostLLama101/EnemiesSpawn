using System;
using UnityEngine;

public class MoveGainSpellpower : RelicInfo
{
    public int addedSpellpower = 0;
    public MoveGainSpellpower()
    {
        Debug.Log("Added MoveGainSpellpower");
        EventBus.Instance.OnMove += OnMove;
    }

    void OnMove()
    {
        Debug.Log("You are moving");
        //maybe here we do a check?
        int curr_added = (int)GameManager.Instance.totalDistance / 50;
        if (curr_added > addedSpellpower)
        {
            addedSpellpower = curr_added;
        }
        
    }
}
