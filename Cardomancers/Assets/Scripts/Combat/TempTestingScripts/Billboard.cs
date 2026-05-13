using UnityEngine;

public class Billboard : MonoBehaviour
{
    Transform camTransform;

    public void Start()
    {
        camTransform = Camera.main.transform;
    }

    void Update()
    {
        transform.rotation = camTransform.rotation;
    }
}