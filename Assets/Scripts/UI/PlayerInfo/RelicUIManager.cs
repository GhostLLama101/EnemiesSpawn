using UnityEngine;

public class RelicUIManager : MonoBehaviour
{
    public GameObject relicUIPrefab;
    public PlayerController player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Debug.Log("RelicUIManager Start called");
        EventBus.Instance.OnRelicPickup += OnRelicPickup;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnRelicPickup()
    {
        Debug.Log($"OnRelicPickup fired. PlayerRelics.Count: {player.PlayerRelics.Count}, UI children: {transform.childCount}");

        // make a new Relic UI representation
        GameObject rui = Instantiate(relicUIPrefab, transform);
        rui.transform.localPosition = new Vector3(-450 + 40 * (player.PlayerRelics.Count - 1), 0, 0);
        RelicUI ruic = rui.GetComponent<RelicUI>();
        ruic.player = player;
        ruic.index = player.PlayerRelics.Count - 1;
    }
    
}
