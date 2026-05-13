using UnityEngine;

public class BattleTutorial : MonoBehaviour
{
    [SerializeField] int i = 0;
    public GameObject tabIcons;
    public GameObject attackIcons;
    public void ShowTutorial()
    {
        if (GameObject.FindWithTag("BattleManager") != null)
        {
            if (i == 0) tabIcons.SetActive(true);
            else
            {
                attackIcons.SetActive(true);
            }
            i++;
        }
        

    }
}