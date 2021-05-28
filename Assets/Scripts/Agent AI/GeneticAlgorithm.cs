using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneticAlgorithm : MonoBehaviour
{
    public int mutation;

    public int[] CrossOver(int[] parentA, int[] parentB)
    {
        int[] offSpring = new int[parentA.Length];
        for(int i = 0; i < parentA.Length; i++)
        {
            int newValue = (parentA[i] + parentB[i]) / 2;
            if (Random.Range(0,1) < mutation)
            {
                int max = 5;
                int min = 0;
                newValue = Mutate(max, min);
            }
            offSpring[i] = newValue;
        }
        
        return offSpring;
    }


    public int Mutate(int maxValue, int minValue)
    {
        int newValue = Random.Range(minValue, maxValue);
        return newValue;
    }



}
