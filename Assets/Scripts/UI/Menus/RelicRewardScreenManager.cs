using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using Random = System.Random;

public class RelicRewardScreenManager : MonoBehaviour
{
    public GameObject screen;
    public GameObject relic1;
    public GameObject relic2;
    public GameObject relic3;
    public Button nextButton;
    public List<RelicInfo> availableRelics = new List<RelicInfo>();
    //private bool rewarded = false;
    //private bool accepted = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        screen.SetActive(true);
    }

    // Update is called once per frame
    public void OnEnable()
    {
        //do rewards
        //Show them
        //Activate the buttons maybe?
        nextButton.onClick.AddListener(Next);

        Random rng = new Random();
        int index = rng.Next(0, GameManager.Instance.Relics.Count);
        Debug.Log("index "+index);
        

        
        //rewarded = true;
        
    }
    public void Next()
    {
        nextButton.onClick.RemoveListener(Next);
        screen.SetActive(false);
    }
}
