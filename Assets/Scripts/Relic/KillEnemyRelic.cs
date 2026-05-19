using UnityEngine;

public class KillEnemyRelic : RelicInfo
{
    public KillEnemyRelic()
    {
        Debug.Log("Added OnEnemyKilled to bus");
        EventBus.Instance.OnEnemyKilled += OnEnemyKilled;
    }

    private void OnEnemyKilled()
    {
        Debug.Log("killed enemy plus 10 mana");
    }
}
