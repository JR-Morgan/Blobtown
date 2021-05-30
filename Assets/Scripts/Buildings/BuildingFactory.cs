using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[DisallowMultipleComponent]
[AddComponentMenu("Simulation/Managers/Building Manager")]
public class BuildingFactory : Singleton<BuildingFactory>
{

    [SerializeField, Tooltip("Mapping between BuildingType and BuildingData")]
    private BuildingData[] buildingDataMapping;

    public Dictionary<BuildingType, List<Building>> Buildings { get; private set; }

    public Building TownCenter { get; private set; }


    protected override void Awake()
    {
        base.Awake();
        Buildings = new Dictionary<BuildingType, List<Building>>();
    }


    public Building CreateBuilding(BuildingType buildingType) => CreateBuilding(buildingType, FindAGoodSpotForBuilding(buildingType));
    public Building CreateBuilding(BuildingType buildingType, Tile position)
    {
        if (position == null) return null;


        BuildingData buildingData = ToBuildingData(buildingType);

        GameObject buildingGo = Instantiate(buildingData.buildingPrefab, position.transform.position, Quaternion.identity, this.transform);
        
        Building building = buildingGo.GetComponent<Building>();
        foreach (Tile tile in TileGrid.Instance.TilesInRect(position, building.Size))
        {
            tile.Building = building;
        }


        if (!Buildings.ContainsKey(buildingType))
        {
            Buildings.Add(buildingType, new List<Building> { building });

            //Special condition for town halls
            if (buildingType == BuildingType.Home)
            {
                TownCenter = building;
                building.IsTownCenter = true;
            }
        }
        else
        {
            Buildings[buildingType].Add(building);
        }

        
        return buildingGo.GetComponent<Building>();
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

    public bool IsBuildingSpaceFree(Tile originTile, BuildingType buildingType)
    {
        BuildingData buildingData = ToBuildingData(buildingType);

        IEnumerable<Tile> buildingSpace = TileGrid.Instance.TilesInRect(originTile, buildingData.buildingPrefab.GetComponent<Building>().Size);

        return buildingSpace.All(t => !t.HasBuilding);
    }

    public Tile FindAGoodSpotForBuilding(BuildingType buildingType)
    {
        Debug.Assert(TownCenter != null, $"Town Center was null!. {typeof(BuildingFactory)} needs a Town Center to find a good spot for a {buildingType}", this);

        Func<Tile, float> costFunction = BuildingRules.Instance.GetFunctionForBuildingType(buildingType);
        IEnumerable<Tile> candidates = FindCandidates(TownCenter.Position, buildingType, 10f);

        Tile bestCandidate = null;
        float bestCost = float.NegativeInfinity;


        foreach(Tile candidate in candidates)
        {
            float cost = costFunction(candidate);
            if (cost > bestCost)
            {
                bestCandidate = candidate;
                bestCost = cost;
            }
        }

        return bestCandidate;
    }

    private IEnumerable<Tile> FindCandidates(Tile center, BuildingType buildingType, float radius, int minNumberOfCandidates = 10, int radiusIncreaseIfFailed = 3, int itteration = 0, int maxItterations = 4)
    {
        IEnumerable<Tile> candidates = center.Grid.TilesInCircle(center, radius).Where(t => IsBuildingSpaceFree(t, buildingType));

        if(candidates.Count() < minNumberOfCandidates && itteration < maxItterations)
        {
            Debug.Log(itteration + "Trying again with radius " + radius + radiusIncreaseIfFailed);
            candidates = FindCandidates(center, buildingType, radius + radiusIncreaseIfFailed, minNumberOfCandidates, radiusIncreaseIfFailed, itteration + 1, maxItterations);
        }
        return candidates;
    }





}

public enum BuildingType
{
    Home,
    Farm,
}