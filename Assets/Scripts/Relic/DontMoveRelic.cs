using System;
using UnityEngine;

public class DontMoveRelic : MonoBehaviour
{
    void Start()
    {
        Debug.Log("calling the relic");
        EventBus.Instance.OnNotMove += OnNotMove;
    }

    void OnNotMove()
    {
        Debug.Log("You are not moving");
    }
}
