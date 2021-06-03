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


    protected override void Awake()
    {
        base.Awake();
        Buildings = new Dictionary<BuildingType, List<Building>>();

        foreach (BuildingType r in (BuildingType[])Enum.GetValues(typeof(BuildingType)))
        {
            Buildings.Add(r, new List<Building>());
        }
    }


    public Building NearestBuildingOfType(Vector3 worldPosition, BuildingType type)
    {
        Building closest = null;
        float distance = float.PositiveInfinity;

        foreach(Building b in Buildings[type])
        {
            float d = Vector3.Distance(worldPosition, b.transform.position);
            if (d < distance)
            {
                closest = b;
                distance = d;
            }
        }
        return closest;
    }

    public TownCenter CreateTownCenter(Tile position) => CreateBuilding(BuildingType.TownCenter, position, null).TownCenter;

    public Building CreateBuilding(BuildingType buildingType, TownCenter townCenter) => CreateBuilding(buildingType, FindAGoodSpotForBuilding(buildingType, townCenter), townCenter);
    public Building CreateBuilding(BuildingType buildingType, Tile position, TownCenter townCenter)
    {
        if (position == null) return null;


        BuildingData buildingData = ToBuildingData(buildingType);

        GameObject buildingGo = Instantiate(buildingData.buildingPrefab, position.transform.position, Quaternion.identity, this.transform);

        buildingGo.name = $"{buildingData.buildingPrefab.name} {Buildings[buildingType].Count}";

        Building building = buildingGo.GetComponent<Building>();
        building.Position = position;
        building.BuildingType = buildingType;

        foreach (Tile buildingTile in TileGrid.Instance.TilesInRect(position, building.Size))
        {
            buildingTile.Building = building;

            foreach (Tile radiusTile in TileGrid.Instance.TilesInCircle(buildingTile, buildingData.discoveredRadius))
            {
                radiusTile.Discovered = true;
            }
        }



        switch (buildingType)
        {
            case BuildingType.Home:
                Home home = building.gameObject.AddComponent<Home>();
                break;
            case BuildingType.TownCenter:
                TownCenter tc = building.gameObject.AddComponent<TownCenter>();
                townCenter = tc;
                break;
            case BuildingType.Farm:
                Farm farm = building.gameObject.AddComponent<Farm>();
                break;
            default:
                throw new NotImplementedException();
        }

        building.TownCenter = townCenter;

        Buildings[buildingType].Add(building);

        return building;
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


    public Tile FindAGoodSpotForBuilding(BuildingType buildingType, TownCenter townCenter)
    {
        Debug.Assert(townCenter != null, $"Town Center was null!. {typeof(BuildingFactory)} needs a Town Center to find a good spot for a {buildingType}", this);

        Func<Tile, float> costFunction = BuildingRules.Instance.GetFunctionForBuildingType(buildingType);
        IEnumerable<Tile> candidates = FindCandidates(townCenter.Building.Position, buildingType, 15f);

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

    private IEnumerable<Tile> FindCandidates(Tile center, BuildingType buildingType, float radius, int minNumberOfCandidates = 10, int radiusIncreaseIfFailed = 3, int itteration = 0, int maxItterations = 20)
    {
        IEnumerable<Tile> candidates = center.Grid.TilesInCircle(center, radius).Where(t => IsBuildingSpaceFree(t, buildingType));

        if(candidates.Count() < minNumberOfCandidates && itteration < maxItterations)
        {
            //Debug.Log(itteration + "Trying again with radius " + (radius + radiusIncreaseIfFailed));
            candidates = FindCandidates(center, buildingType, radius + radiusIncreaseIfFailed, minNumberOfCandidates, radiusIncreaseIfFailed, itteration + 1, maxItterations);
        }
        return candidates;
    }





}

public enum BuildingType
{
    Home,
    TownCenter,
    Farm,
}