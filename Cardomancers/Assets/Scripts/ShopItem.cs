/* Author: DerjenigeUberMensch
 *
 * Contact Group 1 For help or questions relating to this script.
 */
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : ScriptableObject
{
    [SerializeField] private Card_SO so;

    public int Stock { get; set; } = 0;

    public float SellPrice 
    { 
        get
        {   
            if(so == null)
            {   
                Debug.LogError("Shop Item SO is null");
                return 0f;
            }

            return so.sellValue;
        }
        set
        {   
            if(so == null)
            {   
                Debug.LogError("Shop Item SO is null");
                return;
            }

            so.sellValue = (int)value;
        }
    }

    public float PurchasePrice 
    { 
        get
        {   
            if(so == null)
            {   
                Debug.LogError("Shop Item SO is null");
                return 0f;
            }

            return so.price;
        }

        set
        {   
            if(so == null)
            {   
                Debug.LogError("Shop Item SO is null");
                return;
            }
            
            so.price = (int)value;
        } 
    }

    public string DisplayName
    {   
        get
        {   
            if(so == null)
            {   
                Debug.LogError("Shop Item SO is null");
                return "";
            }
            
            return so.displayName;
        }
        set
        {   
            if(so == null)
            {   
                Debug.LogError("Shop Item SO is null");
                return;
            }

            so.displayName = value;
        }
    }

    public string Description
    {
        get
        {   
            if(so == null)
            {   
                Debug.LogError("Shop Item SO is null");
                return "";
            }

            return so.description;
        }
        set
        {  
            if(so == null)
            {   
                Debug.LogError("Shop Item SO is null");
                return;
            }
            
            so.description = value;
        }
    }

    public Sprite Image
    {   
        get
        {   
            if(so == null)
            {   
                Debug.LogError("Shop Item SO is null");
                return null;
            }
            
            return so.image;
        }
        set
        {   
            if(so == null)
            {   
                Debug.LogError("Shop Item SO is null");
                return;
            }
            
            so.image = value;
        }
    }

    public string Type
    {
        get
        {   
            if(so == null)
            {   
                Debug.LogError("Shop Item SO is null");
                return "";
            }

            return so.type;
        }
    }

    public Card_SO SO
    {
        get
        {   return so;
        }
    }

    public void Init(Card_SO so)
    {
        if(so == null)
        {   
            Debug.LogError("Argument \"so\" is null");
            return;
        }
        this.so = so;
        this.Image = so.image;
    } 


}