using UnityEngine;

public class SaveTest : MonoBehaviour
{
    public GameObject player;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SaveSystem.Load(player);
    }

}
