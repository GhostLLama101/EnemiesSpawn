using System;
using UnityEngine;


[Serializable]
public class Enemy : MonoBehaviour
{
    public string name { get; set; } = "DEFAULT";
    public int sprite { get; set; } = 0;
    public int health { get; set; } = 20;
    public int speed { get; set; } = 1;
    public int damage { get; set; } = 1;
    
    /*{
    "name": "zombie",
    "sprite": 0,
    "hp": 20,
    "speed": 5,
    "damage": 5
}*/
}
