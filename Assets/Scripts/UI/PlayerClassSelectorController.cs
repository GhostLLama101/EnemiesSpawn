using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class PlayerClassSelectorController : MonoBehaviour
{
    public Image class_selector;
    public TextMeshProUGUI label;
    public GameObject button;
    public string classText;
    Dictionary<string, PlayerClass> classes = GameManager.Instance.PlayerClasses;
    
    void Start()
    { 
        
        int totalClasses = classes.Count;
        float spacing = 50f;
        float startY = ((totalClasses - 1) * spacing) / 2f;
        float currentY = startY;
        foreach (var kvp in classes)
        {
            GameObject selector = Instantiate(button, class_selector.transform);
            selector.transform.localPosition = new Vector3(0, currentY, 0);
            //selector.GetComponent<MenuSelectorController>().spawner = this;
            selector.GetComponent<PlayerClassSelectorController>().SetClass(kvp.Key);
            currentY -= spacing;
        }
    }

    void Update()
    {
        
    }
    void TaskOnClick(int index)
    {
        Debug.Log("Button " + index + " clicked!");
    }

    public void SelectClass(string key)
    {
        //TODO:
        //spellcaster.class = dic[key](level);
    }

    
    public void SetClass(string text)
    {
        classText = text;
        label.text = text;
    }
}
