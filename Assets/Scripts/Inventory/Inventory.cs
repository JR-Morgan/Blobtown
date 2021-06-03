using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Inventory
{
    [SerializeField]
    private SerializableDictionary<ResourceType, int> _contents;
    public SerializableDictionary<ResourceType, int> Contents { get => _contents; private set => _contents = value; }

    public bool IsEmpty {
        get
        {
            foreach(var v in Contents.Values)
            {
                if (v > 0) return false;
            }
            return true;
        }
    }

    /// <summary>
    /// Attempts to add <paramref name="amount"/> of specified <paramref name="resourceType"/> to the <see cref="Inventory"/><br/>
    /// Will return <c>false</c> if the result of the operation would lead to an invalid (i.e negative) total amount.
    /// </summary>
    /// <param name="resourceType">The <see cref="ResourceType"/></param>
    /// <param name="amount">The amount of a resource to add</param>
    /// <returns><c>true</c> if the <paramref name="amount"/> successfuly was added; otherwise, <c>false</c></returns>
    public bool AddResource(ResourceType resourceType, int amount)
    {
        int currentAmount = Contents.ContainsKey(resourceType)? Contents[resourceType] : 0;

        int desiredAmount = currentAmount + amount;
        if (desiredAmount >= 0)
        {
            if (Contents.ContainsKey(resourceType))
            {
                Contents[resourceType] = amount;
            }
            else
            {
                Contents.Add(resourceType, amount);
            }
        }
        //else if(desiredAmount == 0)
        //{
        //    Contents.Remove(resourceType);
        //}
        else
        {
            return false;
        }

        return true;
    }

    public void AddResources(IEnumerable<KeyValuePair<ResourceType, int>> resources)
    {
        foreach (var r in resources)
        {
            if (Contents.ContainsKey(r.Key))
            {
                Contents[r.Key] += r.Value;
            }
            else
            {
                Contents.Add(r.Key, r.Value);
            }
        }
    }

    public void Clear() => Contents.Clear();

    /// <summary>
    /// Attempts to subtract <paramref name="amount"/> of specified <paramref name="resourceType"/> to the <see cref="Inventory"/><br/>
    /// Will return <c>false</c> if the result of the operation would lead to an invalid (i.e negative) total amount.
    /// </summary>
    /// <param name="resourceType">The <see cref="ResourceType"/></param>
    /// <param name="amount">The amount of a resource to subtract</param>
    /// <returns><c>true</c> if the <paramref name="amount"/> successfuly was subtracted; otherwise, <c>false</c></returns>
    public bool SubtractResource(ResourceType resourceType, int amount) => AddResource(resourceType, -amount);

    public bool HasResource(ResourceType resourceType, int amount)
    {
        return Contents.ContainsKey(resourceType) && Contents[resourceType] >= amount;
    }

    public Inventory(SerializableDictionary<ResourceType, int> contents)
    {
        this.Contents = contents;
    }

    public Inventory() : this(new SerializableDictionary<ResourceType, int>())
    { }

    public int this[ResourceType r] => Contents[r];
}

public enum ResourceType
{
    Wood,
    Ore,
    Food,
}
