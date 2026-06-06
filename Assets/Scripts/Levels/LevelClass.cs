using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelClass
{
    public string name;
    public int waves = 5;
    public List<Spawn> spawns;
}
