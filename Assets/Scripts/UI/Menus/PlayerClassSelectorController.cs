using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.Animations;
using System;

public class PlayerClassSelectorController : MonoBehaviour
{
    public GameObject canvas;
    public GameObject buttonPrefab;
    public GameObject buttonParent;
    public string classText;
    Dictionary<string, PlayerClass> classes;
    
    public void OnEnable()
{
    classes = GameManager.Instance.PlayerClasses;
    List<string> classKeys = new List<string>(classes.Keys);

    for (int i = 0; i < classes.Count; i++)
    {
        //spacing stuff is done in the inspector in the scene
        GameObject newButton = Instantiate(buttonPrefab, buttonParent.transform);
        
        int index = i;
        newButton.GetComponent<ClassButton>().classText.text = classKeys[index];
        newButton.GetComponent<Button>().onClick.AddListener(() => SelectClass(classKeys[index]));
    }
}
    public void ShowUI()
    {
        canvas.SetActive(true);
    }
    public void HideUI()
    {
        canvas.SetActive(false);
    }

    public void SelectClass(string key)
    {
        GameManager.Instance.playerClass = GameManager.Instance.PlayerClasses[key];
        //Debug.Log(key);
    }
}
