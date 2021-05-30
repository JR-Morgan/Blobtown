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
            BuildingType.Farm => Test(),
            _ => Home()
        };
    }
    #region Farm
    [SerializeField, Header("Farm")]
    float isOre;
    [SerializeField]
    float isForest;
    private Func<Tile, float> Test()
    {
        return Sum(FunctionOfTile(isOre, IsOfTypes(TileType.Ore)),
            FunctionOfTile(isForest, IsOfTypes(TileType.Forest)));
    }
    #endregion

    #region Home

    [SerializeField, Header("Home")]
    float neighbourResource = -1f;
    [SerializeField]
    float neighbourTownCenter = 5f, neighbourHome = 4f;

    private Func<Tile, float> Home()
    {
        return Sum(
            Offset(3f),
            ForNeighbours(Sum(
                //FunctionOfTile(2f, HasBuildingOfTypes(BuildingType.Home)),
                FunctionOfTile(neighbourResource, IsOfTypes(TileType.Forest, TileType.Ore)),
                FunctionOfBuilding(neighbourTownCenter, IsTownCenter()),
                FunctionOfBuilding(neighbourHome, IsOfTypes(BuildingType.Home))
                )
            
            ));
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

    private static Func<Tile, float> ForNeighbours(Func<Tile, float> rule)
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

    private static Func<Tile, float> ProximityToBuildingType(float ruleImportance, BuildingType b)
    {
        return Rule;

        float Rule(Tile t)
        {
            float distance = 0f; //TODO
            return distance * ruleImportance;
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
