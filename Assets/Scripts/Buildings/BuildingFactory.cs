using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[AddComponentMenu("Simulation/Managers/Building Manager")]
public class BuildingFactory : Singleton<BuildingFactory>
{

    [SerializeField, Tooltip("Mapping between BuildingType and BuildingData")]
    private BuildingData[] buildingDataMapping;

    public Dictionary<BuildingType, Building> Buildings { get; private set; }

    public Building CreateBuilding(BuildingType buildingType, Tile position)
    {
        BuildingData buildingData = ToBuildingData(buildingType);

        GameObject newBuilding = Instantiate(buildingData.buildingPrefab, position.transform.position, Quaternion.identity, this.transform);

        foreach (Tile tile in TileGrid.Instance.TilesInBounds(position, newBuilding.GetComponent<Building>().size))
        {
            tile.Building = newBuilding.GetComponent<Building>();
        }

        Buildings.Add(buildingType, newBuilding.GetComponent<Building>());
        return newBuilding.GetComponent<Building>();
    }

    private BuildingData ToBuildingData(BuildingType buildingType)
    {
        int i = (int)buildingType;
        if (i >= 0 && i < buildingDataMapping.Length)
        {
            return buildingDataMapping[i];
        }
        return null;
    }

}

public enum BuildingType
{
    Home,
}