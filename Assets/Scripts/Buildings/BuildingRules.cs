using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuildingRules : Singleton<BuildingRules>
{

    public Func<Tile, float> GetFunctionForBuildingType(BuildingType buildingType)
    {
        return buildingType switch
        {
            BuildingType.Home => Home(),
            BuildingType.Farm => Farm(),
            _ => Home()
        };
    }
    #region Farm
    [SerializeField, Header("Farm")]
    float isResource = -20f, neighbourTownCenterFarm = 15f, neighbourHomeFarm = -2f, neighbourFarmFarm = 3f, radiusSmallFarm = 5f, radiusLargeFarm = 15f;

    private Func<Tile, float> Farm()
    {
        return Sum(
            FunctionOfTile(isResource, IsOfTypes(TileType.Ore, TileType.Forest)),
            ForDirectNeighbours(FunctionOfBuilding(neighbourFarmFarm, IsOfTypes(BuildingType.Farm))),

            ForTilesInRadius(radiusSmallFarm, FunctionOfBuilding(neighbourHomeFarm, IsOfTypes(BuildingType.Home))),
            ForTilesInRadius(radiusLargeFarm, FunctionOfBuilding(neighbourTownCenterFarm, IsOfTypes(BuildingType.TownCenter)))
            );
    }
    #endregion

    #region Home

    [SerializeField, Header("Home")]
    float neighbourResource = -10f;
    [SerializeField]
    float isDefaultTile = 2f, neighbourTownCenter = 5f, neighbourHome = -2f, radius = 3f;

    private Func<Tile, float> Home()
    {
        return Sum(
            FunctionOfTile(isDefaultTile, IsOfTypes(TileType.Default)),
            ForDirectNeighbours(Sum(
                FunctionOfTile(neighbourResource, IsOfTypes(TileType.Forest, TileType.Ore)),
                FunctionOfBuilding(neighbourHome, IsOfTypes(BuildingType.Home))
                )),
            ForTilesInRadius(radius, FunctionOfBuilding(neighbourHome, IsOfTypes(BuildingType.Home)))
            );
    }
    #endregion

    #region Generic Rules

    private static Func<Tile, float> Mean(params Func<Tile, float>[] rules)
    {
        return Rule;

        float Rule(Tile t)
        {
            float result = 0;
            foreach (var f in rules)
            {
                result += f.Invoke(t);
            }
            return result / rules.Length;
        }
    }

    private static Func<Tile, float> Sum(params Func<Tile, float>[] rules)
    {
        return Rule;

        float Rule(Tile t)
        {
            float result = 0;
            foreach (var f in rules)
            {
                result += f.Invoke(t);
            }
            return result;
        }
    }

    private static Func<Tile, float> ForDirectNeighbours(Func<Tile, float> rule)
    {
        return Rule;

        float Rule(Tile t)
        {
            List<Tile> neighbours = t.GetAdjacentTiles();
            float result = 0;

            foreach (Tile n in neighbours)
            {
                result += rule(n);
            }

            return result;
        }
    }

    private static Func<Tile,float> Offset(float amount)
    {
        return Rule;

        float Rule(Tile t) => amount;
    }

    private static Func<Tile, float> ForTilesInRadius(float radius, Func<Tile, float> rule)
    {
        return Rule;

        float Rule(Tile t)
        {
            IEnumerable<Tile> neighbours = t.Grid.TilesInCircle(t, radius);
            float result = 0;

            foreach (Tile n in neighbours)
            {
                result += rule(n);
            }

            return result;
        }
    }

    #region Function of Building
    private static Func<Tile, float> FunctionOfBuilding(float ruleImportance, Func<Building, bool> function)
    {
        return Rule;

        float Rule(Tile t)
        {
            Building b = t.Building;
            if (t.HasBuilding && function(b))
            {
                return ruleImportance;
            }
            return 0f;
        }
    }

    private static Func<Building,bool> IsOfTypes(params BuildingType[] buildingType)
    {
        return Function;

        bool Function(Building b)
        {
            return buildingType.Contains(b.BuildingType);
        }
    }

    private static Func<Building, bool> IsDiscovered()
    {
        return Function;

        bool Function(Building b)
        {
            return b.Position.Discovered;
        }
    }

    private static Func<Building, bool> IsTownCenter()
    {
        return Function;

        bool Function(Building b)
        {
            return false; //TODO town hall
        }
    }
    #endregion

    #region Function of a Tile

    private static Func<Tile, float> FunctionOfTile(float ruleImportance, Func<Tile, bool> function)
    {
        return Rule;

        float Rule(Tile t)
        {
            if(function(t))
            {
                return ruleImportance;
            }
            return 0f;
        }
    }


    private static Func<Tile, bool> IsOfTypes(params TileType[] tileType)
    {
        return Function;

        bool Function(Tile t)
        {
            return tileType.Contains(t.TileType);
        }
    }

    [System.Obsolete]
    private static Func<Tile, bool> HasBuildingOfTypes(params BuildingType[] types)
    {
        return Function;

        bool Function(Tile t)
        {
            return t.HasBuilding && types.Contains(t.Building.BuildingType);
        }
    }

    #endregion

    #endregion


}
