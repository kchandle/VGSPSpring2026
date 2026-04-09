using System;
using UnityEngine;
using System.Collections.Generic;

[Serializable]
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
     [SerializeField] private List<TKey> keys;
     [SerializeField] private List<TValue> values;

     //Save dictionary to lists
     public void OnBeforeSerialize()
     {
          if(Keys.Count > 0) keys.Clear();
          values.Clear();
          foreach (KeyValuePair<TKey, TValue> pair in this)
          {
               keys.Add(pair.Key);
               values.Add(pair.Value);
          }
     }

     //Load dictionary from lists
     public void OnAfterDeserialize()
     {
          this.Clear();

          if (keys.Count != values.Count)
          {
               throw new Exception($"There are {keys.Count} keys and {values.Count} values after deserialization. Make sure both key and value types are serializable.");
          }
          
          for (int i = 0; i < keys.Count; i++)
               this.Add(keys[i], values[i]);
     }
}

[Serializable]
public class DictionaryOfCardSOandInt : SerializableDictionary<Card_SO, int>
{
}

[Serializable]
public class DictionaryOfHackSOandInt : SerializableDictionary<Hack_SO, int>
{
}
