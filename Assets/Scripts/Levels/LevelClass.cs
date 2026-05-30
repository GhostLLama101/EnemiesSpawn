using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelClass
{
    public string name;
    public int waves = 5;
    public List<Spawns> spawns;
}
[System.Serializable]
public class Spawns
{
    public string enemy = "zombie";
    public string count = "1";
    public string hp = "base";
    public string speed = "base";
    public string damage = "base";
    public string delay = "2";
    public int[] sequence = {1};
    public string location = "random";

}