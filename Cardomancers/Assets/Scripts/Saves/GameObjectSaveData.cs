using System;
using UnityEngine;

[Serializable]
public class GameObjectSaveData
{
    private bool enabled;
    
    public GameObjectSaveData(bool enabled)
    {
        this.enabled = enabled;
    }
}
