using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
            MineTarget(agent),
            GoHome(agent),
            //MoveToResource(agent, ResourceType.Ore),
            MoveRandomly(agent),
        };
    }

    private static AgentBehaviour[] WoodcutterAgent(AgentAI agent)
    {
        return new AgentBehaviour[]
        {
            ChopTarget(agent),
            GoHome(agent),
            //MoveToResource(agent, ResourceType.Wood),
            MoveRandomly(agent),
        };
    }

    private static AgentBehaviour[] Scout(AgentAI agent)
    {
        return new AgentBehaviour[]
        {
            MoveRandomly(agent),
        };
    }

    #endregion

    #region Helper Methods

    #endregion

    #region Behaviours

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

            if(!agent.Inventory.IsEmpty)
            {
                Tile homeTile = null;

                if (agent.Tile.Building == agent.Home)
                {
                    homeTile = agent.Tile;
                }
                else
                {
                    homeTile = b.NeighbourTiles.Find(t => t.Building == agent.Home);
                }


                if (homeTile != null)
                {
                    agent.Home.Inventory.AddResources(agent.Inventory.Contents);
                    agent.Inventory.Clear();
                }
                else
                {
                    b.shouldTerminate = true;

                    agent.Goal = TileGrid.Instance.TileAtWorldPosition(agent.Home.transform.position);
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
            List<Tile> adjTiles = b.NeighbourTiles;
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
            List<Tile> adjTiles = b.NeighbourTiles;
            foreach (Tile t in adjTiles)
            {
                if (t.TileType == TileType.Ore)
                {
                    agent.Inventory.AddResource(t.TileData.resourceType, t.TileData.amount);

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
            List<Tile> adjTiles = b.NeighbourTiles;
            foreach (Tile t in adjTiles)
            {
                if (t.TileType == TileType.Forest)
                {
                    agent.Inventory.AddResource(t.TileData.resourceType, t.TileData.amount);

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
            if (agent.Home == null)
            {
                if (BuildingFactory.Instance.TownCenter == null)
                {
                    agent.Home = BuildingFactory.Instance.CreateBuilding(BuildingType.Home, agent.Tile);
                }
                else
                {
                    agent.Home = BuildingFactory.Instance.CreateBuilding(BuildingType.Home);
                }
            }

            if (agent.Home != null && (agent.Home.Inventory.HasResource(ResourceType.Ore, 5) || agent.Home.Inventory.HasResource(ResourceType.Wood, 5)))
            {
                agent.Inventory.SubtractResource(ResourceType.Ore, 5);
                agent.Inventory.SubtractResource(ResourceType.Wood, 5);

                agent.Home = BuildingFactory.Instance.CreateBuilding(BuildingType.Home);

                b.shouldTerminate = true;
            }

            return b;
        }
    }

    private static AgentBehaviour MoveToResource(AgentAI agent, ResourceType resource)
    {
        return Action;

        BehaviourState Action(BehaviourState b)
        {
            //nearest resource
            return b;
        }
    }

    #endregion

}

public class BehaviourState
{
    public bool shouldTerminate = false;
    public List<Tile> NeighbourTiles { get; set; }

    public BehaviourState(List<Tile> neighbourTiles)
    {
        NeighbourTiles = neighbourTiles;
    }
}