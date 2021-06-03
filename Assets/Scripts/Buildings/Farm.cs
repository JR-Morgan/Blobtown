using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Building))]
public class Farm : MonoBehaviour
{
    [SerializeField]
    private float seccondsToResource = 5f;
    [SerializeField]
    private int resourceAmount = 1;
    [SerializeField]
    private ResourceType resourceType = ResourceType.Food;

    public Building Building { get; private set; }



    private void Awake()
    {
        Building = this.GetComponent<Building>();
    }

    private float timer;
    private void Update()
    {
        timer += Time.deltaTime;
        if(timer > seccondsToResource)
        {
            timer = 0f;
            Building.Inventory.AddResource(resourceType, resourceAmount);
        }
    }

}
