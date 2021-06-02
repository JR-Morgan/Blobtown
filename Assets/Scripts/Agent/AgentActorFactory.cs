using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public delegate BehaviourState AgentBehaviour(BehaviourState state);

public static class AgentActorFactory 
{
    
    public static AgentActor CreateActor(AgentAI agent, AgentType agentType = default)
    {
        return new AgentActor(CreateBehaviours(agent, agentType));
    }

    public static AgentBehaviour[] CreateBehaviours(AgentAI agent, AgentType agentType)
    {
        return agentType switch
        {
            AgentType.Jobless => JoblessAgent(agent),
            AgentType.Miner => MinerAgent(agent),
            AgentType.WoodCutter => WoodcutterAgent(agent),
            AgentType.Scout => Scout(agent),
            //AgentType.Farmer => Farmer(agent),
            AgentType.Builder => Builder(agent),
            _ => throw new System.NotImplementedException($"No behaviour exists for {agentType}"),
        };
    }


    #region Agent Types

    private static AgentBehaviour[] JoblessAgent(AgentAI agent)
    {
        return new AgentBehaviour[]
        {
            //GetJobFromMyTownHall
            //BuildTownHall
        };
    }

    private static AgentBehaviour[] MinerAgent(AgentAI agent)
    {
        return new AgentBehaviour[]
        {
            DropOffResources(agent),
            MineTarget(agent),
            //MoveToResource(agent, ResourceType.Ore),
            MoveRandomly(agent),
        };
    }

    private static AgentBehaviour[] WoodcutterAgent(AgentAI agent)
    {
        return new AgentBehaviour[]
        {
            DropOffResources(agent),
            ChopTarget(agent),
            //MoveToResource(agent, ResourceType.Wood),
            MoveRandomly(agent),
        };
    }

    private static AgentBehaviour[] Scout(AgentAI agent)
    {
        return new AgentBehaviour[]
        {
            DropOffScout(agent),
            //SurveyTiles()
            //MoveToAdjTile()
            MoveRandomly(agent),
        };
    }

    private static AgentBehaviour[] Builder(AgentAI agent)
    {
        return new AgentBehaviour[]
        {

            BuilderComplete(agent),
            //BuildHome
            //TestBuildingPlot
            //StartBuilding
            //MoveToBuildingPlot
            
        };
    }

    #endregion

    #region Helper Methods

    #endregion

    #region Behaviours

    private static AgentBehaviour DropOffResources(AgentAI agent) //Drop off resources at town Centre and get new job
    {
        return Action;

        BehaviourState Action(BehaviourState b)
        {

            if(!agent.Inventory.IsEmpty) //change to allow builders and scouts to return home
            {
                Tile homeTile = null;

                if (agent.Tile.Building == agent.Home)
                {
                    homeTile = agent.Tile;
                }
                else
                {
                    homeTile = agent.AdjacentTiles.Find(t => t.Building == agent.Home);
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

    private static AgentBehaviour DropOffScout(AgentAI agent)
    {
        return Action;
        BehaviourState Action(BehaviourState b)
        {
            return b;
        }
    }

    private static AgentBehaviour BuilderComplete(AgentAI agent)
    {
        return Action;
        BehaviourState Action(BehaviourState b)
        {
            return b;
        }
    }


    private static AgentBehaviour MoveRandomly(AgentAI agent)
    {
        return Action;

        BehaviourState Action(BehaviourState b)
        {
            var adjTiles = agent.AdjacentTiles;
            agent.Goal = adjTiles[Random.Range(0, adjTiles.Count - 1)];
            b.shouldTerminate = true;
            
            return b;
        }
    }

    private static AgentBehaviour MineTarget(AgentAI agent) //agent works on the nearest resource slowly increasing the progress
    {
        return Action;

        BehaviourState Action(BehaviourState b)
        {
            foreach (Tile t in agent.AdjacentTiles)
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

    private static AgentBehaviour ChopTarget(AgentAI agent) //agent works on the nearest resource slowly increasing the progress
    {
        return Action;

        BehaviourState Action(BehaviourState b)
        {
            foreach (Tile t in agent.AdjacentTiles)
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

    private static AgentBehaviour TestBuildingSpot(AgentAI agent) //when at the correct plot, check it for suitability
    {
        return Action;

        BehaviourState Action(BehaviourState b)
        {
            return b;
        }
    }

    private static AgentBehaviour StartBuildingHome(AgentAI agent) //when at the correct plot and it has been checked. Place the home plot
    {
        return Action;

        BehaviourState Action(BehaviourState b)
        {
            return b;
        }
    }

    private static AgentBehaviour BuildHome(AgentAI agent) //Continue the home building, increasing it's progress bar
    {
        return Action;

        BehaviourState Action(BehaviourState b)
        {

            if (agent.Home != null && (agent.Home.Inventory.HasResource(ResourceType.Ore, 5) || agent.Home.Inventory.HasResource(ResourceType.Wood, 5)))
            {
                agent.Inventory.SubtractResource(ResourceType.Ore, 5);
                agent.Inventory.SubtractResource(ResourceType.Wood, 5);

                agent.Home = BuildingFactory.Instance.CreateBuilding(BuildingType.Home, agent.TownCenter);

                b.shouldTerminate = true;
            }

            return b;
        }
    }

    private static AgentBehaviour MoveToResource(AgentAI agent, ResourceType resource) //go to the resource indicated by the town centre
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
}

public enum AgentType
{
    Jobless,
    Miner,
    WoodCutter,
    Scout,
    Farmer,
    Builder,
}