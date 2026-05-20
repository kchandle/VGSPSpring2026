using System;
using UnityEngine;

[Serializable]
public class GameObjectSaveData
{
    public bool enabled;
    public string name;
    
    public GameObjectSaveData(bool enabled, string name)
    {
        this.enabled = enabled;
        this.name = name;
    }
}
