using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[AddComponentMenu("Simulation/Managers/Tile Manager")]
public class TileManager : Singleton<TileManager>
{
    [SerializeField, Tooltip("Offset for non-resource tiles")]
    private int offset = 1; 

    [SerializeField, Tooltip("Mapping of Tiletype (by index) to a resource data")]
    private ResourceData[] tileToResourceMapping;


    /// <returns>The <see cref="ResourceData"/> associated with the <paramref name="tileType"/>. <c>null</c> if <see cref="TileType"/> has no resource</returns>
    public ResourceData GetResourceData(TileType tileType)
    {
        int i = (int)tileType - offset;
        if (i >= 0 && i < tileToResourceMapping.Length)
        {
            return tileToResourceMapping[i];
        }
        return null;
    }
}
