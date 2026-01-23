using UnityEngine;
using System.Collections.Generic;

public class ObjectPool : MonoBehaviour
{ 
    [SerializeField] [Tooltip("Prefab to pool")] GameObject prefab;
    [SerializeField] [Tooltip("Amount to pool")] int amount;
    private int index = 0;
    List<GameObject> pool = new();

    private void Start()
    {
        for(int i = 0; i < amount; i++)
        {
            GameObject o = Instantiate(prefab, transform);
            o.SetActive(false);
            pool.Add(o);
        }
    }

    public GameObject GetInstance()
    {
        if (index >= pool.Count) index = 0;
        return pool[index++];
    }
}
