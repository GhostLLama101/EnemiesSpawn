using UnityEngine;

public class KillEnemyRelic : MonoBehaviour
{
    public KillEnemyRelic()
    {
        Debug.Log("Added OnNotMove to bus");
        EventBus.Instance.OnNotMove += OnEnemyKilled;
    }

    private void OnEnemyKilled()
    {
        Debug.Log("killed enemy plus 10 mana");
    }
}
