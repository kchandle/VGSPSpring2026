using UnityEngine;
using System.Collections.Generic;
using Random = System.Random;

public static class ShuffleList
{
    private static Random rng = new Random();
    
    public static List<T> Shuffle<T>(List<T> input)
    {
        List<T> copyInput = input;
        int length = copyInput.Count;
        while (length > 1)
        {
            length--;
            int k =  rng.Next(length + 1);
            T value = input[k];
            copyInput[k] = copyInput[length];
            copyInput[length] = value;
        }
        return copyInput;
    }
}
