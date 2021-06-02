using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Building))]
public class TownCenter : MonoBehaviour
{
    public Building Building { get; private set; }

    [SerializeField]
    private SerializableDictionary<ResourceType, List<Tile>> _knownResources;
    public SerializableDictionary<ResourceType, List<Tile>> KnownResources { get => _knownResources; private set => _knownResources = value; }


    private void Awake()
    {
        Building = this.GetComponent<Building>();

        _knownResources = new SerializableDictionary<ResourceType, List<Tile>>();

        foreach (ResourceType r in (ResourceType[])Enum.GetValues(typeof(ResourceType)))
        {
            _knownResources.Add(r, new List<Tile>());
        }
    }
}
