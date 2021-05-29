using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{

    [SerializeField]
    AgentActor[] occupents;

    [SerializeField]
    List<AgentActor> currentOccupents;

    [SerializeField]
    float breedValue;

    /*
    public void BreedingCheck()
    {
        if(currentOccupents.Count > 1)
        {
            if(Random.Range(0,1) > breedValue)
            {
                Breed();
            }
        }
    }
    */

    /*
    public void Breed()
    {
        List<AgentActor> pool = new List<AgentActor>(currentOccupents);
        int index = Random.Range(0, pool.Count);
        AgentActor parentA = pool[index];
        pool.RemoveAt(index);
        index = Random.Range(0, pool.Count);
        AgentActor parentB = pool[index];

        GeneticAlgorithm.Instance.CrossOver(parentA, parentB);

    }
    */



}
