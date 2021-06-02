using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Copied largly from permafrosts pathfinder
/// </summary>
public static class Pathfinder
{
    private const bool DEFAULT_IGNORE = false;
    public static List<Tile> Find(Vector3 start, Vector3 goal, bool ignoreBuildings = DEFAULT_IGNORE)
    {
        if (!TileGrid.IsSingletonInitialised) return null;

        TileGrid t = TileGrid.Instance;
        Tile startT = t.TileAtWorldPosition(start);
        Tile goalT = t.TileAtWorldPosition(goal);

        return Find(startT, goalT, ignoreBuildings);
    }

    public static List<Tile> Find(Vector3 start, Tile goal, bool ignoreUndiscovered = DEFAULT_IGNORE)
    {
        if (!TileGrid.IsSingletonInitialised) return null;

        TileGrid t = TileGrid.Instance;
        Tile startT = t.TileAtWorldPosition(start);

        return Find(startT, goal, ignoreUndiscovered);
    }

    public static List<Tile> Find(Tile start, Tile goal, bool ignoreBuildings = DEFAULT_IGNORE)
    {
        Dictionary<Tile, TileCost> openDictionary = new Dictionary<Tile, TileCost>() { { start, new TileCost(0, CalculateDistance(start, goal)) } }; ;
        Dictionary<Tile, TileCost> closedDictionary = new Dictionary<Tile, TileCost>(); ;

        while (openDictionary.Count != 0)
        {
            Tile current = null;

            float lowest = float.PositiveInfinity;
            TileCost lowestTileCost = null;

            foreach (TileCost i in openDictionary.Values)
            {
                if (i.GetTotalCost() < lowest)
                {
                    lowest = i.GetTotalCost();
                    lowestTileCost = i;
                }
            }

            current = openDictionary.FirstOrDefault(x => x.Value.Equals(lowestTileCost)).Key;

            TileCost currentTotalCost = openDictionary[current];
            openDictionary.Remove(current);

            closedDictionary.Add(current, currentTotalCost);

            if (current == goal)
            {
                return BuildPath(current, closedDictionary);
            }

            Dictionary<Tile, TileCost> neighbourTiles = GetAdjacentTiles(current, goal, currentTotalCost, ignoreBuildings);


            foreach (Tile neighbour in neighbourTiles.Keys)
            {
                //If the neighbour is in the closed dictionary
                if (closedDictionary.ContainsKey(neighbour))
                {
                    continue;
                }

                if (!openDictionary.ContainsKey(neighbour))
                {
                    //Add the neighbour to the open dictionary</summary>
                    openDictionary.Add(neighbour, neighbourTiles[neighbour]);
                }

                //Else we have potentially found a shorter path from the start to this neighbour tile in the open list
                else
                {
                    //Version of neighbour already in the open dictionary
                    Tile openNeighbour = openDictionary.FirstOrDefault(x => x.Key == neighbour).Key;

                    //If the new fromStartTileCost for neighbour is less than the old cost
                    if (neighbourTiles[neighbour].FromStart < openDictionary[openNeighbour].FromStart)
                    {
                        //Set the old cost to the new fromStartTileCost
                        openDictionary[openNeighbour].FromStart = neighbourTiles[neighbour].FromStart;

                        //>Update the parent in the old version
                        openDictionary[openNeighbour].Parent = neighbourTiles[neighbour].Parent;
                    }
                }
            }
        }

        return null;
    }

    private static Dictionary<Tile, TileCost> GetAdjacentTiles(Tile current, Tile goal, TileCost currentTotalCost, bool ignoreUnwalkable)
    {
        Dictionary<Tile, TileCost> tileList = new Dictionary<Tile, TileCost>();

        foreach (Tile t in current.GetAdjacentTiles().Where(t => t.Discovered || ignoreUnwalkable))
        {
            float costValue = currentTotalCost.FromStart + CalculateDistance(current, t);
            TileCost cost = new TileCost(costValue, CalculateDistance(t, goal))
            {
                Parent = current
            };

            tileList.Add(t, cost);
        }
        return tileList;
    }

    private static float CalculateDistance(Component a, Component b) => Vector3.Distance(a.transform.position, b.transform.position);

    private static List<Tile> BuildPath(Tile tile, Dictionary<Tile, TileCost> closedTiles)
    {

        Stack<Tile> tileStack = new Stack<Tile>();
        List<Tile> tileQueueFromStart = new List<Tile>();

        if (closedTiles != null)
        {

            //Current equals the end tile
            Tile current = tile;
            tileStack.Push(current);

            //This will stop once current is the start
            while (closedTiles[current].HasParent())
            {
                tileStack.Push(closedTiles[current].Parent);
                current = closedTiles[current].Parent;

            };

            int count = tileStack.Count();

            //Since the start is on top of the stack we can just pop and enqueue till the stack is empty
            for (int i = 0; i < count; i++)
            {
                tileQueueFromStart.Add(tileStack.Pop());
            }

        }
        return tileQueueFromStart;
    }
}

public class TileCost
{
    ///<summary>Cost from this tile going to start through discovered paths</summary>
    public float FromStart { get; set; }
    ///<summary>Cost from this tile directly to goal</summary>
    public float ToGoal { get; }
    public Tile Parent { get; set; }

    public TileCost(float FromStart, float ToGoal)
    {
        Parent = null;
        this.FromStart = FromStart;
        this.ToGoal = ToGoal;
    }

    public float GetTotalCost()
    {
        return (FromStart + ToGoal);
    }

    public bool HasParent()
    {
        return (Parent != null);
    }
}
