using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class TriggerSetup : MonoBehaviour
{
    private BoxCollider trigger;
    [SerializeField] private QuestForTriggerSetup expression;
    
    private void Awake()
    {
        trigger = GetComponent<BoxCollider>();
        trigger.isTrigger = true;
        Transform originalParent = transform.parent;
        
        Transform shopTransform = GameObject.FindGameObjectWithTag("StoreLocation").transform;
        transform.SetParent(shopTransform);
        this.transform.position = shopTransform.position;
        transform.SetParent(originalParent);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            switch (expression)
            {
                case QuestForTriggerSetup.HIRING:
                    FindFirstObjectByType<HiringQuestStep>().GetComponent<HiringQuestStep>().FinishMe();
                    break;
                case QuestForTriggerSetup.INGREDIENTS:
                    FindFirstObjectByType<ReturnToCardShop>().GetComponent<ReturnToCardShop>().FinishMe();
                    break;
                case QuestForTriggerSetup.ADVERTISING:
                    FindFirstObjectByType<ReturnToCardShop>().GetComponent<ReturnToCardShop>().FinishMe();
                    break;
                case QuestForTriggerSetup.ESCAPE:
                    FindFirstObjectByType<EscapeToCardShop>().GetComponent<EscapeToCardShop>().PlayerEntersCardShop();
                    break;
            }
        }
    }
}

[Serializable]
public enum QuestForTriggerSetup
{
    HIRING,
    ADVERTISING,
    INGREDIENTS,
    ESCAPE
}