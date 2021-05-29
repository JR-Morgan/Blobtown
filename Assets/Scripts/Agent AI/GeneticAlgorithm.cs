using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneticAlgorithm : Singleton<GeneticAlgorithm>
{
    public int mutation;
    /*
    public AgentActor CrossOver(AgentActor parentA, AgentActor parentB)
    {
        float[] parentAAttributes = parentA.getAttributes();
        float[] parentBAttributes = parentB.getAttributes();


        float[] offSpring = new float[parentAAttributes.Length];
        for(int i = 0; i < parentAAttributes.Length; i++)
        {
            float newValue = (parentAAttributes[i] + parentBAttributes[i]) / 2;
            if (Random.Range(0,1) < mutation)
            {
                int max = 5;
                int min = 0;
                newValue = Mutate(max, min);
            }
            offSpring[(int)i] = newValue;
        }

        //return offSpring;
        return null;
    }
    */


    private int Mutate(int maxValue, int minValue)
    {
        int newValue = Random.Range(minValue, maxValue);
        return newValue;
    }



}
