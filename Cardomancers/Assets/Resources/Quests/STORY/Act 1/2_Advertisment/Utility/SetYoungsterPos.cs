using UnityEngine;

public class SetYoungsterPos : MonoBehaviour
{
    void Start()
    {
        this.transform.position = GameObject.FindGameObjectWithTag("Player").transform.position +  new Vector3(0, 1, 0);
    }
}
