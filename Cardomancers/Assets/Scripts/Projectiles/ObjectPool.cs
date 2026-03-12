using UnityEngine;
using System.Collections.Generic;

public class ObjectPool : MonoBehaviour
{ 
    [SerializeField] [Tooltip("Prefab to pool")] GameObject prefab;
    [SerializeField] [Tooltip("Amount to pool")] int amount;
    private int index = 0;
    List<GameObject> pool = new();

    //instantiates the amount of cards you set at the beginning, sets them inactive and adds them to the list
    private void Start()
    {
        for(int i = 0; i < amount; i++)
        {
            GameObject o = Instantiate(prefab, transform);
            o.SetActive(false);
            pool.Add(o);
        }
    }

    //returns the next instance at the index in the list and increments the index
    public GameObject GetInstance()
    {
        if (index >= pool.Count) index = 0;
        return pool[index++];
    }
}
