using UnityEngine;
using Newtonsoft.Json;
using System.Collections.Generic;

public static class JSONReader
{
    public static List<T> Load<T>(string filename)
    {
        TextAsset json = Resources.Load<TextAsset>(filename);
        return JsonConvert.DeserializeObject<List<T>>(json.text);
    }

    public static Dictionary<string, T> LoadDictionary<T>(string filename)
    {
        TextAsset json = Resources.Load<TextAsset>(filename);
        return JsonConvert.DeserializeObject<Dictionary<string, T>>(json.text);
    }
}

//enemyDefs = JSONReader.Load<ClassName>("JSON File Name ie enemies)");

