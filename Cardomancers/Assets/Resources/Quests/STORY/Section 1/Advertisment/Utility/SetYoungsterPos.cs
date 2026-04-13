using UnityEngine;

public class SetYoungsterPos : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.transform.position = GameObject.FindGameObjectWithTag("Player").transform.position +  new Vector3(0, 1, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
