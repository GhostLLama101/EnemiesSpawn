using UnityEngine;
using TMPro;

public class PlayerClassSelectorController : MonoBehaviour
{
    public TextMeshProUGUI label;
    public string level;
    public EnemySpawner spawner;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetClass(string text)
    {
        level = text;
        label.text = text;
    }

    public void SelectClass(string key)
    {
        //TODO:
        //spellcaster.class = dic[key](level);
    }
}
