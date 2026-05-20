using UnityEngine;

public class PersistEnabledData : MonoBehaviour
{
    private void Start()
    {
        GameObjectSaveData[] gameObjectSaveDatas = SaveSystem.GetEnabledData();
        foreach (GameObjectSaveData gameObjectSaveData in gameObjectSaveDatas)
        {
            if (gameObjectSaveData.name == this.name)
            {
                gameObject.SetActive(gameObjectSaveData.enabled);
                break;
            }
        }
    }
}
