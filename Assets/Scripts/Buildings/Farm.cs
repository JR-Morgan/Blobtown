using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Building))]
public class Farm : MonoBehaviour
{
    public Building Building { get; private set; }


    public void Awake()
    {
        Building = this.GetComponent<Building>();
    }

}
