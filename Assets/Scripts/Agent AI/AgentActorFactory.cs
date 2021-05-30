using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate BehaviourState AgentBehaviour(BehaviourState state);

public static class AgentActorFactory 
{
    
    public static AgentActor CreateActor(AgentAI agent)
    {
        return new AgentActor(CreateBehaviours(agent));
    }

    public static List<AgentBehaviour> CreateBehaviours(AgentAI agent)
    {
        var behaviours = new List<AgentBehaviour>();

        //add behaviours depending on agent type?
        behaviours.AddRange(MinerAgent(agent));

        return behaviours;
    }


    #region Agent Types
    private static AgentBehaviour[] BasicAgent(AgentAI agent)
    {
        return new AgentBehaviour[]
        {

            GoHome(agent),
            Wait(agent)
        };
    }

    private static AgentBehaviour[] MinerAgent(AgentAI agent)
    {
        return new AgentBehaviour[]
        {
            //DestinationCheck(agent),
            MineTarget(agent),
            ChopTarget(agent),
            BuildHome(agent),
            GoHome(agent),
            MoveRandomly(agent),
            
        };
    }

    #endregion

    #region Helper Methods

    #endregion

    #region Behaviours

    //private static AgentBehaviour DestinationCheck(AgentAI agent)
    //{
    //    return Action;

    //    BehaviourState Action(BehaviourState b)
    //    {
    //        if (agent.HasDestination)
    //        {
    //            b.shouldTerminate = true;
    //        }
    //        return b;
    //    }
    //}


    private static AgentBehaviour Wait(AgentAI agent)
    {
        return Action;

        BehaviourState Action(BehaviourState b)
        {
            b.shouldTerminate = true;
            return b;
        }
        
       
    }

    private static AgentBehaviour GoHome(AgentAI agent)
    {
        return Action;

        BehaviourState Action(BehaviourState b)
        {
            //Someone needs to fix this insanity

            if(!agent.inventory.IsEmpty)
            {
                List<Tile> adjTiles = TileGrid.Instance.GetAdjacentTiles(TileGrid.Instance.TileAtWorldPosition(agent.transform.position));
                Tile neighbourHome = null;
                foreach (Tile t in adjTiles)
                { 
                    if (t.Building == agent.home)
                    {
                        neighbourHome = t;
                    }
                }

                if (neighbourHome != null)
                {
                    foreach (KeyValuePair<ResourceType, int> entry in agent.inventory.Contents)
                    {
                        
                        agent.home.inventory.AddResource(entry.Key, entry.Value);
                        Debug.Log(agent.home.inventory.Contents);
                        agent.inventory.SubtractResource(entry.Key, entry.Value);


                    }

                }
                else
                {
                    b.shouldTerminate = true;

                    agent.Goal = (TileGrid.Instance.TileAtWorldPosition(agent.home.transform.position));
                }

            }

            return b;
        }
    }

    private static AgentBehaviour MoveRandomly(AgentAI agent)
    {
        return Action;

        BehaviourState Action(BehaviourState b)
        {
            
            //currently moves randomly
            List<Tile> adjTiles = TileGrid.Instance.GetAdjacentTiles(TileGrid.Instance.TileAtWorldPosition(agent.transform.position));
            //agent.SetDestination(adjTiles[Random.Range(0, adjTiles.Count - 1)].transform.position);
            agent.Goal = adjTiles[Random.Range(0, adjTiles.Count - 1)];
            b.shouldTerminate = true;
            
            return b;
        }
    }

    private static AgentBehaviour MineTarget(AgentAI agent)
    {
        return Action;

        BehaviourState Action(BehaviourState b)
        {
            List<Tile> adjTiles = TileGrid.Instance.GetAdjacentTiles(TileGrid.Instance.TileAtWorldPosition(agent.transform.position));
            foreach (Tile t in adjTiles)
            {
                if (t.TileType == TileType.Ore)
                {
                    agent.inventory.AddResource(t.TileData.resourceType, t.TileData.amount);

                    t.TileType = TileType.Default;

                    b.shouldTerminate = true;
                    break;
                }
            }


            return b;
        }
    }

    private static AgentBehaviour ChopTarget(AgentAI agent)
    {
        return Action;

        BehaviourState Action(BehaviourState b)
        {
            List<Tile> adjTiles = TileGrid.Instance.GetAdjacentTiles(TileGrid.Instance.TileAtWorldPosition(agent.transform.position));
            foreach (Tile t in adjTiles)
            {
                if (t.TileType == TileType.Forest)
                {
                    agent.inventory.AddResource(t.TileData.resourceType, t.TileData.amount);

                    t.TileType = TileType.Default;

                    b.shouldTerminate = true;
                    break;
                }
            }


            return b;
        }
    }

    private static AgentBehaviour BuildHome(AgentAI agent)
    {
        return Action;

        BehaviourState Action(BehaviourState b)
        {

            //Debug.Log(agent.home.inventory);

            if (agent.home == null || (agent.home != null && (agent.home.inventory.HasResource(ResourceType.Ore, 5) || agent.home.inventory.HasResource(ResourceType.Wood, 5))))
            {
                //TODO: find location to place house
                Tile tile = HomeManager.Instance.NextHomeTile();
                if (tile == null)
                {
                    TileGrid.Instance.TryGetTileAtWorldPosition(agent.transform.position, out tile);
                }
                agent.home = HomeManager.Instance.BuildHome(tile);
                b.shouldTerminate = true;
            }

            return b;
        }

        
    }

    #endregion

}

public class BehaviourState
{
    public bool shouldTerminate = false;
}