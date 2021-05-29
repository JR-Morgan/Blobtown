using System.Collections.Generic;

public class Inventory
{
    public Dictionary<ResourceType, int> Contents { get; private set; }

    public bool IsEmpty => Contents.Count == 0;

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
        if (desiredAmount > 0)
        {
            if (Contents.ContainsKey(resourceType))
            {
                Contents[resourceType] = desiredAmount;
            }
            else
            {
                Contents.Add(resourceType, desiredAmount);
            }
        }
        else if(desiredAmount == 0)
        {
            Contents.Remove(resourceType);
        }
        else
        {
            return false;
        }

        return true;
    }

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

    public Inventory(Dictionary<ResourceType, int> contents)
    {
        this.Contents = contents;
    }

    public Inventory() : this(new Dictionary<ResourceType, int>())
    { }

    public int this[ResourceType r] => Contents[r];
}

public enum ResourceType
{
    Wood,
    Ore
}
