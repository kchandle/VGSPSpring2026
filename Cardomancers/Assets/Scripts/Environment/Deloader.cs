using System;
using UnityEngine;

public class Deloader : MonoBehaviour
{
    private Deloader instance;
    
    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RegionToggleActive(GameObject region)
    {
        region.SetActive(!region.activeSelf);
    }
}
