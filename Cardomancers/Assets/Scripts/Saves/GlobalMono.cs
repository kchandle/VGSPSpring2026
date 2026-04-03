using UnityEngine;

internal class GlobalMono : MonoBehaviour
{
    private static GlobalMono _instance;

    internal static GlobalMono Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Object.FindFirstObjectByType<GlobalMono>();

                if (_instance == null)
                {
                    GameObject go = new GameObject("__RuntimeGlobalMono__Internal__");
                    _instance = go.AddComponent<GlobalMono>();
                    DontDestroyOnLoad(go);
                    go.hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector;
                }
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }
}