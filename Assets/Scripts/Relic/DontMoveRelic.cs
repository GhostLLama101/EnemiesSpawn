using System;
using UnityEngine;

public class DontMoveRelic : RelicInfo
{
    public DontMoveRelic()
    {
        Debug.Log("Added OnNotMove to bus");
        EventBus.Instance.OnNotMove += OnNotMove;
    }

    void OnNotMove()
    {
        Debug.Log("You are not moving");
    }
}
